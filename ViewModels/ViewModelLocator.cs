using RestaurantApp.ViewModels;

namespace RestaurantApp.ViewModels;

/// <summary>
/// Статичний локатор ViewModels.
/// Дає Pages доступ до вже створених VM без повторної ініціалізації.
/// </summary>
public static class ViewModelLocator
{
    private static MainViewModel? _main;

    public static MainViewModel Main
    {
        get => _main ??= new MainViewModel();
        set => _main = value;
    }
}
