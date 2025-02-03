using System;

public class LanguageSetting : Setting
{
}
[Serializable]
public struct TextLanguage
{
    public enum LanguageType
    {
        Russian, English
    }
    public LanguageType Language;
    public string Text;
}