using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using RestaurantApp.Models;

namespace RestaurantApp.Localization;

/// <summary>
/// Статичний текст у XAML: Text="{loc:Tr Menu_Title}".
/// Під капотом — прив'язка до індексатора Loc, тому текст змінюється разом із мовою.
/// </summary>
public class TrExtension : MarkupExtension
{
    public string Key { get; set; } = string.Empty;

    public TrExtension() { }
    public TrExtension(string key) => Key = key;

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var binding = new Binding($"[{Key}]")
        {
            Source = Loc.Instance,
            Mode = BindingMode.OneWay
        };
        return binding.ProvideValue(serviceProvider);
    }
}

/// <summary>
/// База для розширень, які залежать і від даних, і від мови:
/// значення береться з Path, а друга прив'язка до Loc.Language змушує перерахувати текст при зміні мови.
/// </summary>
public abstract class LocBindingExtension : MarkupExtension
{
    public string Path { get; set; } = ".";
    public string Key { get; set; } = string.Empty;

    protected abstract IMultiValueConverter CreateConverter();

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        var binding = new MultiBinding
        {
            Converter = CreateConverter(),
            ConverterParameter = Key,
            Mode = BindingMode.OneWay
        };
        binding.Bindings.Add(new Binding(Path));
        binding.Bindings.Add(new Binding(nameof(Loc.Language))
        {
            Source = Loc.Instance,
            Mode = BindingMode.OneWay
        });
        return binding.ProvideValue(serviceProvider);
    }
}

/// <summary>Текст із шаблону: Text="{loc:Fmt Key=Order_Number, Path=Id}".</summary>
public class FmtExtension : LocBindingExtension
{
    protected override IMultiValueConverter CreateConverter() => new FormatConverter();
}

/// <summary>Назва статусу замовлення: Text="{loc:Status}" (за замовчуванням бере властивість Status).</summary>
public class StatusExtension : LocBindingExtension
{
    public StatusExtension() => Path = "Status";
    protected override IMultiValueConverter CreateConverter() => new StatusTextConverter();
}

/// <summary>«В наявності» / «Немає»: Text="{loc:Avail}" (бере властивість IsAvailable).</summary>
public class AvailExtension : LocBindingExtension
{
    public AvailExtension() => Path = "IsAvailable";
    protected override IMultiValueConverter CreateConverter() => new AvailabilityTextConverter();
}

internal class FormatConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        var key = parameter as string ?? string.Empty;
        var value = values.Length > 0 && values[0] != DependencyProperty.UnsetValue ? values[0] : null;
        return Loc.Get(key, value);
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

internal class StatusTextConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => values.Length > 0 && values[0] is OrderStatus status ? status.ToDisplayString() : "—";

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}

internal class AvailabilityTextConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        => Loc.Get(values.Length > 0 && values[0] is true ? "Dish_Available" : "Dish_Unavailable");

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
