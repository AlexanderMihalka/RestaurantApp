using System.Windows;
using RestaurantApp.Localization;

namespace RestaurantApp;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        // Мова, обрана минулого разу, має бути відома ще до створення вікна й демо-даних
        Loc.Instance.LoadSaved();
        base.OnStartup(e);
    }
}
