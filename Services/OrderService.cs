using System;
using System.Collections.Generic;
using System.Linq;
using RestaurantApp.Models;

namespace RestaurantApp.Services;

/// <summary>
/// Сервісний шар: бізнес-логіка замовлень, відокремлена від UI.
/// </summary>
public class OrderService
{
    private readonly RestaurantContext _ctx;

    public OrderService(RestaurantContext context) => _ctx = context;

    /// <summary>Створити нове замовлення і призначити офіціанта.</summary>
    public Order CreateOrder(int tableNumber, Waiter waiter)
    {
        var order = new Order { TableNumber = tableNumber };
        waiter.AssignOrder(order);
        _ctx.Orders.Add(order);

        var table = _ctx.Tables.FirstOrDefault(t => t.Number == tableNumber);
        if (table is not null)
        {
            table.IsOccupied = true;
            table.CurrentOrder = order;
        }

        return order;
    }

    /// <summary>Додати страву до замовлення.</summary>
    public void AddDishToOrder(Order order, Dish dish, int qty = 1)
    {
        order.AddItem(dish, qty);
    }

    /// <summary>Видалити страву із замовлення.</summary>
    public bool RemoveDishFromOrder(Order order, int dishId) =>
        order.RemoveItem(dishId);

    /// <summary>Змінити статус замовлення.</summary>
    public void UpdateStatus(Order order, OrderStatus status)
    {
        order.SetStatus(status);

        if (!order.IsActive)
        {
            var table = _ctx.Tables.FirstOrDefault(t => t.Number == order.TableNumber);
            if (table is not null)
            {
                table.IsOccupied = false;
                table.CurrentOrder = null;
            }
        }
    }

    /// <summary>Сплатити замовлення (з можливою знижкою).</summary>
    public decimal PayOrder(Order order, decimal discountPercent = 0)
    {
        var total = order.CalculateTotalWithDiscount(discountPercent);
        UpdateStatus(order, OrderStatus.Paid);
        return total;
    }

    /// <summary>Скасувати замовлення.</summary>
    public void CancelOrder(Order order)
    {
        UpdateStatus(order, OrderStatus.Cancelled);
        order.Waiter?.RemoveOrder(order);
    }

    /// <summary>Активні замовлення.</summary>
    public IEnumerable<Order> GetActiveOrders() =>
        _ctx.Orders.Where(o => o.IsActive).OrderByDescending(o => o.CreatedAt);

    /// <summary>Усі замовлення.</summary>
    public IEnumerable<Order> GetAllOrders() =>
        _ctx.Orders.OrderByDescending(o => o.CreatedAt);

    /// <summary>Статистика виручки за сьогодні.</summary>
    public decimal TodayRevenue() =>
        _ctx.Orders
            .Where(o => o.IsPaid && o.CreatedAt.Date == DateTime.Today)
            .Sum(o => o.CalculateTotal());
}
