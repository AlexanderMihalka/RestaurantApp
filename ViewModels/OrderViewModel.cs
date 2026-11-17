using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using RestaurantApp.Localization;
using RestaurantApp.Models;
using RestaurantApp.Services;

namespace RestaurantApp.ViewModels;

public class OrderViewModel : BaseViewModel
{
    private readonly RestaurantContext _ctx;
    private readonly OrderService _service;

    // --- Стан ---
    private Order? _selectedOrder;
    private Dish?  _selectedDish;
    private Waiter? _selectedWaiter;
    private int    _selectedTable = 1;
    private int    _dishQty = 1;
    private string _statusMessage = string.Empty;
    private decimal _discountPercent = 0;

    public ObservableCollection<Order>  AllOrders   { get; } = new();
    public ObservableCollection<Waiter> Waiters     { get; } = new();
    public ObservableCollection<Dish>   MenuDishes  { get; } = new();
    public ObservableCollection<int>    Tables      { get; } = new();

    public Order? SelectedOrder
    {
        get => _selectedOrder;
        set
        {
            SetField(ref _selectedOrder, value);
            OnPropertyChanged(nameof(HasOrder));
            OnPropertyChanged(nameof(OrderItems));
            OnPropertyChanged(nameof(OrderTotal));
            OnPropertyChanged(nameof(SelectedOrderStatus));
        }
    }

    public Dish? SelectedDish
    {
        get => _selectedDish;
        set => SetField(ref _selectedDish, value);
    }

    public Waiter? SelectedWaiter
    {
        get => _selectedWaiter;
        set => SetField(ref _selectedWaiter, value);
    }

    public int SelectedTable
    {
        get => _selectedTable;
        set => SetField(ref _selectedTable, value);
    }

    public int DishQty
    {
        get => _dishQty;
        set => SetField(ref _dishQty, value < 1 ? 1 : value);
    }

    public decimal DiscountPercent
    {
        get => _discountPercent;
        set => SetField(ref _discountPercent, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetField(ref _statusMessage, value);
    }

    public bool HasOrder => _selectedOrder is not null;

    public ObservableCollection<OrderItem> OrderItems =>
        _selectedOrder is null
            ? new()
            : new(_selectedOrder.Items);

    public decimal OrderTotal =>
        _selectedOrder?.CalculateTotalWithDiscount(_discountPercent) ?? 0;

    public string SelectedOrderStatus =>
        _selectedOrder?.Status.ToDisplayString() ?? "—";

    // --- Команди ---
    public ICommand CreateOrderCommand  { get; }
    public ICommand AddDishCommand      { get; }
    public ICommand RemoveDishCommand   { get; }
    public ICommand SetInProgressCommand{ get; }
    public ICommand SetServedCommand    { get; }
    public ICommand PayOrderCommand     { get; }
    public ICommand CancelOrderCommand  { get; }

    public OrderViewModel(RestaurantContext ctx, OrderService service)
    {
        _ctx     = ctx;
        _service = service;

        CreateOrderCommand   = new RelayCommand(CreateOrder,   () => _selectedWaiter is not null);
        AddDishCommand       = new RelayCommand(AddDish,       () => _selectedOrder is not null && _selectedDish is not null);
        RemoveDishCommand    = new RelayCommand<OrderItem>(RemoveItem);
        SetInProgressCommand = new RelayCommand(() => ChangeStatus(OrderStatus.InProgress), () => _selectedOrder?.Status == OrderStatus.Pending);
        SetServedCommand     = new RelayCommand(() => ChangeStatus(OrderStatus.Served),     () => _selectedOrder?.Status == OrderStatus.InProgress);
        PayOrderCommand      = new RelayCommand(Pay,    () => _selectedOrder?.Status == OrderStatus.Served);
        CancelOrderCommand   = new RelayCommand(Cancel, () => _selectedOrder?.IsActive == true);

        Refresh();
    }

    public void Refresh()
    {
        AllOrders.Clear();
        foreach (var o in _service.GetAllOrders()) AllOrders.Add(o);

        Waiters.Clear();
        foreach (var w in _ctx.Waiters) Waiters.Add(w);

        MenuDishes.Clear();
        foreach (var d in _ctx.Menu.Dishes) MenuDishes.Add(d);

        Tables.Clear();
        foreach (var t in _ctx.Tables) Tables.Add(t.Number);

        if (_selectedWaiter is null && Waiters.Any())
            SelectedWaiter = Waiters.First();

        NotifyOrderChanged();
    }

    private void CreateOrder()
    {
        if (_selectedWaiter is null) return;
        var order = _service.CreateOrder(_selectedTable, _selectedWaiter);
        AllOrders.Insert(0, order);
        SelectedOrder = order;
        ShowStatus(Loc.Get("Msg_OrderCreated", order.Id, _selectedTable));
    }

    private void AddDish()
    {
        if (_selectedOrder is null || _selectedDish is null) return;
        _service.AddDishToOrder(_selectedOrder, _selectedDish, _dishQty);
        NotifyOrderChanged();
        ShowStatus(Loc.Get("Msg_DishAdded", _selectedDish.Name, _dishQty));
    }

    private void RemoveItem(OrderItem? item)
    {
        if (_selectedOrder is null || item is null) return;
        _service.RemoveDishFromOrder(_selectedOrder, item.Dish.Id);
        NotifyOrderChanged();
        ShowStatus(Loc.Get("Msg_DishRemoved", item.Dish.Name));
    }

    private void ChangeStatus(OrderStatus status)
    {
        if (_selectedOrder is null) return;
        _service.UpdateStatus(_selectedOrder, status);
        NotifyOrderChanged();
        ShowStatus(Loc.Get("Msg_Status", status.ToDisplayString()));
    }

    private void Pay()
    {
        if (_selectedOrder is null) return;
        var total = _service.PayOrder(_selectedOrder, _discountPercent);
        NotifyOrderChanged();
        ShowStatus(Loc.Get("Msg_Paid", total));
        Refresh();
    }

    private void Cancel()
    {
        if (_selectedOrder is null) return;
        _service.CancelOrder(_selectedOrder);
        NotifyOrderChanged();
        ShowStatus(Loc.Get("Msg_Cancelled"));
        Refresh();
    }

    /// <summary>Мову змінено: оновлюємо тексти, які VM формує сам (статус вибраного замовлення).</summary>
    public void OnLanguageChanged() => OnPropertyChanged(nameof(SelectedOrderStatus));

    private void NotifyOrderChanged()
    {
        OnPropertyChanged(nameof(OrderItems));
        OnPropertyChanged(nameof(OrderTotal));
        OnPropertyChanged(nameof(HasOrder));
        OnPropertyChanged(nameof(SelectedOrderStatus));
    }

    private void ShowStatus(string message)
    {
        StatusMessage = message;
    }
}

// Утилітний клас для команди з параметром
public class RelayCommand<T> : System.Windows.Input.ICommand
{
    private readonly System.Action<T?> _execute;
    private readonly System.Func<T?, bool>? _canExecute;

    public RelayCommand(System.Action<T?> execute, System.Func<T?, bool>? canExecute = null)
    {
        _execute    = execute;
        _canExecute = canExecute;
    }

    public event System.EventHandler? CanExecuteChanged
    {
        add    => System.Windows.Input.CommandManager.RequerySuggested += value;
        remove => System.Windows.Input.CommandManager.RequerySuggested -= value;
    }

    public bool CanExecute(object? p) => _canExecute?.Invoke((T?)p) ?? true;
    public void Execute(object? p)    => _execute((T?)p);
}
