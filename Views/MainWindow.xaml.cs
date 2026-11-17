using System;
using System.Windows;
using System.Windows.Threading;
using RestaurantApp.Localization;
using RestaurantApp.ViewModels;

namespace RestaurantApp.Views;

public partial class MainWindow : Window
{
    private readonly MainViewModel _vm;
    private readonly DispatcherTimer _clock;
    private readonly DispatcherTimer _statsTimer;

    public MainWindow()
    {
        InitializeComponent();

        // Реєструємо VM у локаторі — всі Page зможуть його використати
        _vm = new MainViewModel();
        ViewModelLocator.Main = _vm;
        DataContext = _vm;

        // Годинник
        _clock = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
        _clock.Tick += (_, _) => UpdateClock();
        _clock.Start();
        UpdateClock();

        // Оновлення статистики
        _statsTimer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(5) };
        _statsTimer.Tick += (_, _) => _vm.RefreshStats();
        _statsTimer.Start();
    }

    // Дата й час відображаються мовою інтерфейсу
    private void UpdateClock() =>
        ClockText.Text = DateTime.Now.ToString("dddd, dd MMMM  HH:mm:ss", Loc.Instance.Culture);
}
