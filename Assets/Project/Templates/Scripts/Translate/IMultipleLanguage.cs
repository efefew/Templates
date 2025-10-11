/// <summary>
/// у меня несколько языков
/// </summary>
public interface IMultipleLanguage
{
    /// <summary>
    /// При изменении языка
    /// </summary>
    /// <param name="language">Выбранный язык</param>
    void OnChangeLanguage(Language.LanguageType language);
}

public static class Language
{
    public static event DelegateChangeLanguage EventChangeLanguage;
    public delegate void DelegateChangeLanguage(LanguageType language);

    public enum LanguageType
    {
        ENGLISH,
        RUSSIAN,
        SPANISH,
        FRENCH,
        GERMAN,
        JAPAN,
        CHINESE
    }
    public static LanguageType Type
    {
        get => _languageValue;
        set
        {
            OnChangeLanguage(value);
            _languageValue = value;
        }
    }

    private static LanguageType _languageValue;
    public const int COUNT_LANGUAGE = 2;


    /// <summary>
    /// При изменении языка
    /// </summary>
    private static void OnChangeLanguage(LanguageType newLanguage)
    {
        if (newLanguage == Type)
            return;
        EventChangeLanguage?.Invoke(newLanguage);
    }
}