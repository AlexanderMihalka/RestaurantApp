using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace RestaurantApp.Localization;

/// <summary>
/// Поточна мова інтерфейсу та доступ до текстів.
/// XAML звертається через індексатор (див. TrExtension), код — через Loc.Get(...).
/// Коли мова змінюється, індексатор сповіщає всі прив'язки, і тексти оновлюються на льоту.
/// </summary>
public sealed class Loc : INotifyPropertyChanged
{
    public const string Ukrainian = "uk";
    public const string English = "en";

    public static Loc Instance { get; } = new();

    private string _language = Ukrainian;

    private Loc() { }

    public event PropertyChangedEventHandler? PropertyChanged;

    public string Language => _language;

    public CultureInfo Culture =>
        new(_language == English ? "en-US" : "uk-UA");

    /// <summary>Індексатор для прив'язок у XAML: {Binding [Ключ]}.</summary>
    public string this[string key] => Get(key);

    public static string GetIn(string language, string key)
    {
        var table = language == English ? Strings.En : Strings.Uk;
        return table.TryGetValue(key, out var text) ? text : key;
    }

    public static string Get(string key) => GetIn(Instance._language, key);

    /// <summary>Текст із підстановкою значень: Loc.Get("Msg_Paid", total).</summary>
    public static string Get(string key, params object?[] args) =>
        string.Format(Instance.Culture, GetIn(Instance._language, key), args);

    public void SetLanguage(string language, bool save = true)
    {
        if (language != Ukrainian && language != English) return;
        if (language == _language) return;

        _language = language;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Language)));
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Item[]"));

        if (save) Save();
    }

    // --- Запам'ятовуємо вибір, щоб наступного разу програма стартувала тією ж мовою ---

    private static string SettingsFile => Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "RestaurantApp", "language.txt");

    public void LoadSaved()
    {
        try
        {
            if (File.Exists(SettingsFile))
                SetLanguage(File.ReadAllText(SettingsFile).Trim(), save: false);
        }
        catch
        {
            // немає доступу до файлу — просто лишаємо мову за замовчуванням
        }
    }

    private void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsFile)!);
            File.WriteAllText(SettingsFile, _language);
        }
        catch
        {
            // не вдалося зберегти — не критично
        }
    }
}
