using System.Windows.Controls;
using RestaurantApp.ViewModels;

namespace RestaurantApp.Views;

public partial class WaiterView : Page
{
    public WaiterView()
    {
        InitializeComponent();
        DataContext = ViewModelLocator.Main.WaiterVM;
    }
}
