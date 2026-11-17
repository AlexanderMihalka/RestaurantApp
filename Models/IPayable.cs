namespace RestaurantApp.Models;

/// <summary>
/// Інтерфейс для об'єктів, які можна сплатити.
/// Демонструє принцип поліморфізму.
/// </summary>
public interface IPayable
{
    decimal CalculateTotal();
    decimal CalculateTotalWithDiscount(decimal discountPercent);
    bool IsPaid { get; }
}
