using RestaurantApp.Localization;

namespace RestaurantApp.Models;

/// <summary>
/// Статус замовлення у вигляді enum для типобезпечного порівняння.
/// </summary>
public enum OrderStatus
{
    Pending,      // Очікує
    InProgress,   // Готується
    Served,       // Подано
    Paid,         // Сплачено
    Cancelled     // Скасовано
}

public static class OrderStatusExtensions
{
    /// <summary>Назва статусу мовою інтерфейсу.</summary>
    public static string ToDisplayString(this OrderStatus status) => status switch
    {
        OrderStatus.Pending    => Loc.Get("Status_Pending"),
        OrderStatus.InProgress => Loc.Get("Status_InProgress"),
        OrderStatus.Served     => Loc.Get("Status_Served"),
        OrderStatus.Paid       => Loc.Get("Status_Paid"),
        OrderStatus.Cancelled  => Loc.Get("Status_Cancelled"),
        _                      => status.ToString()
    };

    public static string ToColor(this OrderStatus status) => status switch
    {
        OrderStatus.Pending    => "#FFA500",
        OrderStatus.InProgress => "#2196F3",
        OrderStatus.Served     => "#4CAF50",
        OrderStatus.Paid       => "#9C27B0",
        OrderStatus.Cancelled  => "#F44336",
        _                      => "#888888"
    };
}
