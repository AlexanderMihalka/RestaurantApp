using System.Collections.Generic;
using System.Linq;

namespace RestaurantApp.Models;

/// <summary>
/// Меню ресторану — містить колекцію страв і методи роботи з ними.
/// </summary>
public class Menu
{
    private readonly List<Dish> _dishes = new();

    public IReadOnlyList<Dish> Dishes => _dishes.AsReadOnly();

    public IEnumerable<string> Categories =>
        _dishes.Select(d => d.Category).Distinct().OrderBy(c => c);

    public void AddDish(Dish dish) => _dishes.Add(dish);

    public bool RemoveDish(int dishId)
    {
        var dish = _dishes.FirstOrDefault(d => d.Id == dishId);
        if (dish is null) return false;
        _dishes.Remove(dish);
        return true;
    }

    public Dish? FindById(int id) =>
        _dishes.FirstOrDefault(d => d.Id == id);

    public IEnumerable<Dish> GetByCategory(string category) =>
        _dishes.Where(d => d.Category == category && d.IsAvailable);

    public IEnumerable<Dish> Search(string query) =>
        _dishes.Where(d =>
            d.Name.Contains(query, System.StringComparison.OrdinalIgnoreCase) ||
            d.Category.Contains(query, System.StringComparison.OrdinalIgnoreCase));

    public int Count => _dishes.Count;
}
