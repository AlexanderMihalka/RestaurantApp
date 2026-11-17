namespace RestaurantApp.Models;

/// <summary>
/// Позиція замовлення: страва + кількість.
/// Subtotal рахується автоматично.
/// </summary>
public class OrderItem
{
    public Dish Dish { get; }
    public int Quantity { get; set; }
    public decimal Subtotal => Dish.Price * Quantity;

    public string Note { get; set; } = string.Empty;

    public OrderItem(Dish dish, int quantity = 1)
    {
        Dish = dish;
        Quantity = quantity;
    }

    public override string ToString() =>
        $"{Dish.Name} × {Quantity} = {Subtotal:C}";
}
