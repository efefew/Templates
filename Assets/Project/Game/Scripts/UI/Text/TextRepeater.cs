using System.Collections;

using TMPro;

using UnityEngine;
[RequireComponent(typeof(TMP_Text))]
public class TextRepeater : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _labelTarget;
    private TMP_Text _label;
    private Coroutine _coroutine;
    private void Awake()
    {
        _label = GetComponent<TMP_Text>();
        if (!_labelTarget)
        {
            return;
        }

        _coroutine = StartCoroutine(CopyTextCoroutine());
    }
    private void OnDestroy() => StopCoroutine(_coroutine);
    private IEnumerator CopyTextCoroutine()
    {
        CopyText();
        while (true)
        {
            yield return new WaitWhile(EqualText);
            CopyText();
        }
    }

    private bool EqualText() => _label.text == _labelTarget.text;
    private void CopyText() => _label.text = _labelTarget.text;
}
