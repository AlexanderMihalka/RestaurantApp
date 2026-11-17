using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Input;
using RestaurantApp.Localization;
using RestaurantApp.Models;

namespace RestaurantApp.ViewModels;

public class MenuViewModel : BaseViewModel
{
    private readonly Menu _menu;

    private ObservableCollection<Dish> _dishes = new();
    private Dish? _selectedDish;
    private string _searchText = string.Empty;
    // Підпис пункту «усі категорії» залежить від мови, тому тримаємо його окремо
    private string _allLabel = Loc.Get("Filter_All");
    private string _selectedCategory = Loc.Get("Filter_All");

    private string _newName = string.Empty;
    private string _newDescription = string.Empty;
    private string _newPriceText = string.Empty;
    private string _newCategory = string.Empty;
    private string _newEmoji = "🍽️";

    public ObservableCollection<Dish> Dishes
    {
        get => _dishes;
        set => SetField(ref _dishes, value);
    }

    public Dish? SelectedDish
    {
        get => _selectedDish;
        set
        {
            SetField(ref _selectedDish, value);
            OnPropertyChanged(nameof(HasSelection));
            if (value is not null) LoadDishForEdit(value);
        }
    }

    public bool HasSelection => _selectedDish is not null;

    public string SearchText
    {
        get => _searchText;
        set { SetField(ref _searchText, value); ApplyFilter(); }
    }

    public string SelectedCategory
    {
        get => _selectedCategory;
        set { SetField(ref _selectedCategory, value); ApplyFilter(); }
    }

    public ObservableCollection<string> Categories { get; } = new();

    // Форма додавання
    public string NewName        { get => _newName;        set => SetField(ref _newName, value); }
    public string NewDescription { get => _newDescription; set => SetField(ref _newDescription, value); }
    public string NewPriceText   { get => _newPriceText;   set => SetField(ref _newPriceText, value); }
    public string NewCategory    { get => _newCategory;    set => SetField(ref _newCategory, value); }
    public string NewEmoji       { get => _newEmoji;       set => SetField(ref _newEmoji, value); }

    private int _nextId = 100;

    public ICommand AddDishCommand    { get; }
    public ICommand RemoveDishCommand { get; }
    public ICommand ClearFormCommand  { get; }

    public MenuViewModel(Menu menu)
    {
        _menu = menu;

        AddDishCommand    = new RelayCommand(AddDish,    CanAddDish);
        RemoveDishCommand = new RelayCommand(RemoveDish, () => HasSelection);
        ClearFormCommand  = new RelayCommand(ClearForm);

        RefreshAll();
    }

    /// <summary>
    /// Мову змінено. categoryMap — «стара назва категорії → нова» (від RestaurantContext.ApplyLanguage).
    /// Зберігаємо обраний фільтр, переклавши його, і перебудовуємо списки.
    /// </summary>
    public void OnLanguageChanged(IReadOnlyDictionary<string, string> categoryMap)
    {
        var oldSelected = _selectedCategory;
        var wasAll = string.IsNullOrEmpty(oldSelected) || oldSelected == _allLabel;

        _allLabel = Loc.Get("Filter_All");
        var target = wasAll
            ? _allLabel
            : categoryMap.TryGetValue(oldSelected, out var mapped) ? mapped : oldSelected;

        RefreshCategories();      // ComboBox може скинути вибір під час перебудови списку,
        SelectedCategory = target; // тому виставляємо його знову (це ще й оновить список страв)
        OnPropertyChanged(nameof(SelectedCategory));

        if (_selectedDish is not null) LoadDishForEdit(_selectedDish);
    }

    private void RefreshAll()
    {
        ApplyFilter();
        RefreshCategories();
    }

    private void ApplyFilter()
    {
        var source = _menu.Dishes.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(_searchText))
            source = source.Where(d =>
                d.Name.Contains(_searchText, System.StringComparison.OrdinalIgnoreCase));

        if (_selectedCategory != _allLabel)
            source = source.Where(d => d.Category == _selectedCategory);

        Dishes = new ObservableCollection<Dish>(source);
    }

    private void RefreshCategories()
    {
        Categories.Clear();
        Categories.Add(_allLabel);
        foreach (var c in _menu.Categories)
            Categories.Add(c);
    }

    private bool CanAddDish() =>
        !string.IsNullOrWhiteSpace(_newName) &&
        !string.IsNullOrWhiteSpace(_newCategory) &&
        decimal.TryParse(_newPriceText, out _);

    private void AddDish()
    {
        if (!decimal.TryParse(_newPriceText, out var price)) return;

        var dish = new Dish(_nextId++, _newName, _newDescription, price, _newCategory, _newEmoji);
        _menu.AddDish(dish);
        ClearForm();
        RefreshAll();
    }

    private void RemoveDish()
    {
        if (_selectedDish is null) return;
        _menu.RemoveDish(_selectedDish.Id);
        SelectedDish = null;
        RefreshAll();
    }

    private void LoadDishForEdit(Dish dish)
    {
        NewName        = dish.Name;
        NewDescription = dish.Description;
        NewPriceText   = dish.Price.ToString();
        NewCategory    = dish.Category;
        NewEmoji       = dish.Emoji;
    }

    private void ClearForm()
    {
        NewName = NewDescription = NewPriceText = NewCategory = string.Empty;
        NewEmoji = "🍽️";
        SelectedDish = null;
    }
}
