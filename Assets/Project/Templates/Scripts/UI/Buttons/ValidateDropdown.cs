using System.Collections;

using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(Dropdown))]
public class ValidateDropdown : MonoBehaviour
{
    #region Fields

    private Dropdown _dropdown;
    [FormerlySerializedAs("frames")] public int Frames;
    [FormerlySerializedAs("minCountOptions")] public int MinCountOptions;

    #endregion Fields

    #region Methods

    private void Awake() => _dropdown = GetComponent<Dropdown>();

    private void OnEnable() => StartCoroutine(NextFrame(Frames));

    private IEnumerator NextFrame(int countFrame)
    {
        for (int i = 0; i < countFrame; i++)
            yield return new WaitForFixedUpdate();
        _dropdown.interactable = _dropdown.options.Count >= MinCountOptions;
    }

    #endregion Methods
}