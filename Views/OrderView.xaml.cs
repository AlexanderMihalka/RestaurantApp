using System.Windows.Controls;
using RestaurantApp.ViewModels;

namespace RestaurantApp.Views;

public partial class OrderView : Page
{
    public OrderView()
    {
        InitializeComponent();
        // DataContext прокидається через Frame -> MainViewModel -> OrderVM
        DataContext = ViewModelLocator.Main.OrderVM;
    }
}
