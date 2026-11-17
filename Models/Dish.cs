using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RestaurantApp.Models;

/// <summary>
/// Страва — базова одиниця меню.
/// Name, Description і Category сповіщають інтерфейс про зміни: так назви
/// демо-страв перекладаються при перемиканні мови без перебудови списків.
/// </summary>
public class Dish : INotifyPropertyChanged
{
    private string _name = string.Empty;
    private string _description = string.Empty;
    private string _category = string.Empty;

    public event PropertyChangedEventHandler? PropertyChanged;

    public int Id { get; set; }
    public string Name        { get => _name;        set => Set(ref _name, value); }
    public string Description { get => _description; set => Set(ref _description, value); }
    public decimal Price { get; set; }
    public string Category    { get => _category;    set => Set(ref _category, value); }
    public string Emoji { get; set; } = "🍽️";
    public bool IsAvailable { get; set; } = true;

    public Dish() { }

    public Dish(int id, string name, string description, decimal price, string category, string emoji = "🍽️")
    {
        Id = id;
        Name = name;
        Description = description;
        Price = price;
        Category = category;
        Emoji = emoji;
    }

    private void Set(ref string field, string value, [CallerMemberName] string? name = null)
    {
        if (field == value) return;
        field = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    public override string ToString() => $"{Name} — {Price:C}";
}
