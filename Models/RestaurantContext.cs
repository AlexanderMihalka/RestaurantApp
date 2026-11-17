using System.Collections.Generic;
using System.Linq;
using RestaurantApp.Localization;

namespace RestaurantApp.Models;

/// <summary>
/// Центральний стан застосунку. Singleton.
/// Зберігає меню, офіціантів, замовлення та столи.
/// </summary>
public class RestaurantContext
{
    private static RestaurantContext? _instance;
    public static RestaurantContext Instance => _instance ??= new RestaurantContext();

    public Menu Menu { get; } = new();
    public List<Waiter> Waiters { get; } = new();
    public List<Order> Orders { get; } = new();
    public List<Table> Tables { get; } = new();

    private RestaurantContext()
    {
        SeedData();
    }

    // Демо-дані: id, ціна, ключ категорії, емодзі. Тексти беруться зі словника (Localization/Strings.cs)
    private static readonly (int Id, decimal Price, string CategoryKey, string Emoji)[] SeedDishes =
    {
        (1,   89m, "Cat_Soups",    "🍲"),
        (2,   95m, "Cat_Soups",    "🥣"),
        (3,  110m, "Cat_Main",     "🥟"),
        (4,  165m, "Cat_Main",     "🍗"),
        (5,  390m, "Cat_Grill",    "🥩"),
        (6,  285m, "Cat_Grill",    "🐟"),
        (7,  125m, "Cat_Salads",   "🥗"),
        (8,  105m, "Cat_Salads",   "🫒"),
        (9,   95m, "Cat_Desserts", "🍰"),
        (10,  85m, "Cat_Desserts", "🧁"),
        (11,  45m, "Cat_Drinks",   "☕"),
        (12,  65m, "Cat_Drinks",   "🍊"),
    };

    private static readonly string[] CategoryKeys =
        { "Cat_Soups", "Cat_Main", "Cat_Grill", "Cat_Salads", "Cat_Desserts", "Cat_Drinks" };

    // Демо-офіціанти (ім'я залежить від мови), щоб їх можна було перейменувати при перемиканні
    private readonly List<(Waiter Waiter, string NameKey)> _seedWaiters = new();

    private void SeedData()
    {
        // Страви меню
        foreach (var d in SeedDishes)
        {
            Menu.AddDish(new Dish(
                d.Id,
                Loc.Get($"Dish_{d.Id}_Name"),
                Loc.Get($"Dish_{d.Id}_Desc"),
                d.Price,
                Loc.Get(d.CategoryKey),
                d.Emoji));
        }

        // Офіціанти
        AddSeedWaiter("Waiter_1_Name", "+380501234567", "👩");
        AddSeedWaiter("Waiter_2_Name", "+380671234567", "👨");
        AddSeedWaiter("Waiter_3_Name", "+380931234567", "👩‍🦱");

        // Столи
        for (int i = 1; i <= 8; i++)
            Tables.Add(new Table(i, i <= 4 ? 2 : 4));
    }

    private void AddSeedWaiter(string nameKey, string phone, string emoji)
    {
        var waiter = new Waiter(Loc.Get(nameKey), phone, emoji);
        Waiters.Add(waiter);
        _seedWaiters.Add((waiter, nameKey));
    }

    /// <summary>
    /// Переводить демо-дані на нову мову. Викликається ДО зміни мови в Loc,
    /// поки ще активна стара: так видно, які саме категорії треба замінити.
    /// Повертає відповідність «стара назва категорії → нова», щоб меню оновило фільтр.
    /// </summary>
    public Dictionary<string, string> ApplyLanguage(string newLanguage)
    {
        var categoryMap = new Dictionary<string, string>();
        foreach (var key in CategoryKeys)
            categoryMap[Loc.Get(key)] = Loc.GetIn(newLanguage, key);

        var seedIds = SeedDishes.Select(d => d.Id).ToHashSet();
        foreach (var dish in Menu.Dishes)
        {
            // категорії міняємо і в страв, доданих користувачем, якщо вони обрали готову категорію
            if (categoryMap.TryGetValue(dish.Category, out var newCategory))
                dish.Category = newCategory;

            if (seedIds.Contains(dish.Id))
            {
                dish.Name = Loc.GetIn(newLanguage, $"Dish_{dish.Id}_Name");
                dish.Description = Loc.GetIn(newLanguage, $"Dish_{dish.Id}_Desc");
            }
        }

        foreach (var (waiter, nameKey) in _seedWaiters)
            waiter.Name = Loc.GetIn(newLanguage, nameKey);

        return categoryMap;
    }

    public IEnumerable<Order> ActiveOrders =>
        Orders.Where(o => o.IsActive);
}
