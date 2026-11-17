using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantApp.Localization;

namespace RestaurantApp.Models;

/// <summary>
/// Замовлення. Реалізує IPayable для підтримки поліморфізму.
/// </summary>
public class Order : IPayable
{
    private static int _nextId = 1;

    private readonly List<OrderItem> _items = new();

    public int Id { get; } = _nextId++;
    public DateTime CreatedAt { get; } = DateTime.Now;
    public DateTime? ClosedAt { get; private set; }

    public int TableNumber { get; set; }
    public Waiter? Waiter { get; set; }
    public OrderStatus Status { get; private set; } = OrderStatus.Pending;

    public IReadOnlyList<OrderItem> Items => _items.AsReadOnly();

    // IPayable
    public bool IsPaid => Status == OrderStatus.Paid;

    public decimal CalculateTotal() =>
        _items.Sum(item => item.Subtotal);

    public decimal CalculateTotalWithDiscount(decimal discountPercent)
    {
        if (discountPercent < 0 || discountPercent > 100)
            throw new ArgumentOutOfRangeException(nameof(discountPercent), Loc.Get("Err_Discount"));
        var total = CalculateTotal();
        return total * (1 - discountPercent / 100);
    }

    // Управління позиціями
    public void AddItem(Dish dish, int quantity = 1)
    {
        if (Status != OrderStatus.Pending && Status != OrderStatus.InProgress)
            throw new InvalidOperationException(Loc.Get("Err_OrderClosed"));

        var existing = _items.FirstOrDefault(i => i.Dish.Id == dish.Id);
        if (existing is not null)
            existing.Quantity += quantity;
        else
            _items.Add(new OrderItem(dish, quantity));
    }

    public bool RemoveItem(int dishId)
    {
        var item = _items.FirstOrDefault(i => i.Dish.Id == dishId);
        if (item is null) return false;
        _items.Remove(item);
        return true;
    }

    public void ClearItems() => _items.Clear();

    // Управління статусом
    public void SetStatus(OrderStatus status)
    {
        Status = status;
        if (status is OrderStatus.Paid or OrderStatus.Cancelled)
            ClosedAt = DateTime.Now;
    }

    public bool IsActive =>
        Status is OrderStatus.Pending or OrderStatus.InProgress or OrderStatus.Served;

    public int TotalItems => _items.Sum(i => i.Quantity);

    public override string ToString() =>
        Loc.Get("Order_Summary", Id, TableNumber, Status.ToDisplayString(), CalculateTotal());
}
