using System.Windows.Controls;
using RestaurantApp.ViewModels;

namespace RestaurantApp.Views;

public partial class MenuView : Page
{
    public MenuView()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.Main.MenuVM;
    }
}
