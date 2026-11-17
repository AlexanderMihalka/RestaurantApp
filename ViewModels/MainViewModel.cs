using System.Windows.Input;
using RestaurantApp.Localization;
using RestaurantApp.Models;
using RestaurantApp.Services;

namespace RestaurantApp.ViewModels;

public class MainViewModel : BaseViewModel
{
    private readonly RestaurantContext _ctx;
    private readonly OrderService _service;

    private int _selectedTabIndex = 0;

    public MenuViewModel  MenuVM  { get; }
    public OrderViewModel OrderVM { get; }
    public WaiterViewModel WaiterVM { get; }

    public ICommand SetLanguageCommand { get; }

    public int SelectedTabIndex
    {
        get => _selectedTabIndex;
        set => SetField(ref _selectedTabIndex, value);
    }

    // Статистика для Dashboard
    public int ActiveOrdersCount  => _ctx.ActiveOrders.Count();
    public int TotalDishesCount   => _ctx.Menu.Count;
    public int WaitersCount       => _ctx.Waiters.Count;
    public decimal TodayRevenue   => _service.TodayRevenue();

    public MainViewModel()
    {
        _ctx     = RestaurantContext.Instance;
        _service = new OrderService(_ctx);

        MenuVM   = new MenuViewModel(_ctx.Menu);
        OrderVM  = new OrderViewModel(_ctx, _service);
        WaiterVM = new WaiterViewModel(_ctx);

        SetLanguageCommand = new RelayCommand<string>(SetLanguage);
    }

    /// <summary>Перемикач мови: "uk" або "en".</summary>
    private void SetLanguage(string? language)
    {
        if (language is null || language == Loc.Instance.Language) return;

        // Спершу переводимо демо-дані (поки ще активна стара мова — так видно, що саме замінювати),
        // потім перемикаємо мову, і лише тоді оновлюємо ViewModel-и.
        var categoryMap = _ctx.ApplyLanguage(language);
        Loc.Instance.SetLanguage(language);

        MenuVM.OnLanguageChanged(categoryMap);
        OrderVM.OnLanguageChanged();
        WaiterVM.OnLanguageChanged();
        RefreshStats();
    }

    public void RefreshStats()
    {
        OnPropertyChanged(nameof(ActiveOrdersCount));
        OnPropertyChanged(nameof(TotalDishesCount));
        OnPropertyChanged(nameof(WaitersCount));
        OnPropertyChanged(nameof(TodayRevenue));
    }
}
