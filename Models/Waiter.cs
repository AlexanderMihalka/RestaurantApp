using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace RestaurantApp.Models;

/// <summary>
/// Офіціант — управляє своїми замовленнями.
/// Name сповіщає інтерфейс про зміни (ім'я демо-офіціантів залежить від мови).
/// </summary>
public class Waiter : INotifyPropertyChanged
{
    private static int _nextId = 1;

    private string _name = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    private readonly List<Order> _orders = new();

    public int Id { get; } = _nextId++;
    public string Name
    {
        get => _name;
        set
        {
            if (_name == value) return;
            _name = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
        }
    }
    public string Phone { get; set; } = string.Empty;
    public string Emoji { get; set; } = "👨‍🍳";

    public IReadOnlyList<Order> Orders => _orders.AsReadOnly();

    public IEnumerable<Order> ActiveOrders =>
        _orders.Where(o => o.IsActive);

    public decimal TotalRevenue =>
        _orders.Where(o => o.IsPaid).Sum(o => o.CalculateTotal());

    public int CompletedOrdersCount =>
        _orders.Count(o => o.Status == OrderStatus.Paid);

    public Waiter() { }

    public Waiter(string name, string phone = "", string emoji = "👨‍🍳")
    {
        Name = name;
        Phone = phone;
        Emoji = emoji;
    }

    public void AssignOrder(Order order)
    {
        if (!_orders.Contains(order))
            _orders.Add(order);
        order.Waiter = this;
    }

    public void RemoveOrder(Order order)
    {
        _orders.Remove(order);
    }

    public override string ToString() => $"{Emoji} {Name}";
}
