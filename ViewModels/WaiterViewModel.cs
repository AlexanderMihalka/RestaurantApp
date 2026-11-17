using System.Collections.ObjectModel;
using System.Windows.Input;
using RestaurantApp.Localization;
using RestaurantApp.Models;

namespace RestaurantApp.ViewModels;

public class WaiterViewModel : BaseViewModel
{
    private readonly RestaurantContext _ctx;

    private Waiter? _selected;
    private string  _name  = string.Empty;
    private string  _phone = string.Empty;
    private string  _emoji = "👨";
    private string  _statusMessage = string.Empty;

    public ObservableCollection<Waiter> Waiters { get; } = new();

    public Waiter? Selected
    {
        get => _selected;
        set
        {
            SetField(ref _selected, value);
            OnPropertyChanged(nameof(HasSelection));
            OnPropertyChanged(nameof(SelectedOrders));
            OnPropertyChanged(nameof(TotalRevenue));
            OnPropertyChanged(nameof(CompletedCount));
            if (value is not null) LoadForEdit(value);
        }
    }

    public bool HasSelection => _selected is not null;

    public System.Collections.Generic.IEnumerable<Order> SelectedOrders =>
        _selected?.Orders ?? System.Linq.Enumerable.Empty<Order>();

    public decimal TotalRevenue => _selected?.TotalRevenue ?? 0;
    public int CompletedCount   => _selected?.CompletedOrdersCount ?? 0;

    public string Name  { get => _name;  set => SetField(ref _name,  value); }
    public string Phone { get => _phone; set => SetField(ref _phone, value); }
    public string Emoji { get => _emoji; set => SetField(ref _emoji, value); }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetField(ref _statusMessage, value);
    }

    public string[] EmojiOptions { get; } = { "👨", "👩", "👨‍🦱", "👩‍🦱", "👨‍🦳", "👩‍🦳" };

    public ICommand AddWaiterCommand    { get; }
    public ICommand RemoveWaiterCommand { get; }
    public ICommand ClearCommand        { get; }
    public ICommand SetEmojiCommand     { get; }

    public WaiterViewModel(RestaurantContext ctx)
    {
        _ctx = ctx;

        AddWaiterCommand    = new RelayCommand(AddWaiter,    () => !string.IsNullOrWhiteSpace(_name));
        RemoveWaiterCommand = new RelayCommand(RemoveWaiter, () => HasSelection);
        ClearCommand        = new RelayCommand(Clear);
        SetEmojiCommand     = new RelayCommand<string>(e => Emoji = e ?? "👨");

        Refresh();
    }

    public void Refresh()
    {
        Waiters.Clear();
        foreach (var w in _ctx.Waiters) Waiters.Add(w);
    }

    /// <summary>Мову змінено: ім'я вибраного офіціанта у формі могло перекластися.</summary>
    public void OnLanguageChanged()
    {
        if (_selected is not null) LoadForEdit(_selected);
    }

    private void AddWaiter()
    {
        var w = new Waiter(_name, _phone, _emoji);
        _ctx.Waiters.Add(w);
        Refresh();
        Clear();
        StatusMessage = Loc.Get("Msg_WaiterAdded", w.Name);
    }

    private void RemoveWaiter()
    {
        if (_selected is null) return;
        _ctx.Waiters.Remove(_selected);
        Selected = null;
        Refresh();
        StatusMessage = Loc.Get("Msg_WaiterRemoved");
    }

    private void LoadForEdit(Waiter w)
    {
        Name  = w.Name;
        Phone = w.Phone;
        Emoji = w.Emoji;
    }

    private void Clear()
    {
        Name  = Phone = string.Empty;
        Emoji = "👨";
        Selected = null;
    }
}
