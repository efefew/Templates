/// <summary>
///     У меня несколько языков
/// </summary>
public interface IMultipleLanguage
{
    /// <summary>
    ///     При изменении языка
    /// </summary>
    /// <param name="language">Выбранный язык</param>
    void OnChangeLanguage(Language.LanguageType language);
}

public static class Language
{
    public delegate void DelegateChangeLanguage(LanguageType language);

    public enum LanguageType
    {
        English,
        Russian,
        Spanish,
        French,
        German,
        Japan,
        Chinese
    }

    public const int COUNT_LANGUAGE = 7;

    private static LanguageType _languageValue;

    public static LanguageType Type
    {
        get => _languageValue;
        set
        {
            OnChangeLanguage(value);
            _languageValue = value;
        }
    }

    public static event DelegateChangeLanguage EventChangeLanguage;


    /// <summary>
    ///     При изменении языка
    /// </summary>
    private static void OnChangeLanguage(LanguageType newLanguage)
    {
        if (newLanguage == Type)
            return;
        EventChangeLanguage?.Invoke(newLanguage);
    }
}