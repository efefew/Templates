using TMPro;
using UnityEngine;
using AYellowpaper.SerializedCollections;

[RequireComponent(typeof(TMP_Text))]
public class MultipleLanguageLabel : MonoBehaviour, IMultipleLanguage
{
    private TMP_Text _label;
    public SerializedDictionary<Language.LanguageType, string> TranslatedText  { get; private set; } = new ();

    private void Awake() => _label = GetComponent<TMP_Text>();

    private void OnEnable()
    {
        OnChangeLanguage(Language.Type);
        Language.EventChangeLanguage += OnChangeLanguage;
    }

    private void OnDisable() => Language.EventChangeLanguage -= OnChangeLanguage;

    public void OnChangeLanguage(Language.LanguageType language)
    {
        if (!TranslatedText.ContainsKey(language))
        {
            _label.text = TranslatedText[Language.LanguageType.ENGLISH];
            return;
        }
        _label.text = TranslatedText[language];
    }
}