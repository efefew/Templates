using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Toggle))]
public class ToggleSwap : MonoBehaviour
{
    [field: SerializeField]
    public Toggle Toggle { get; private set; }
    public GameObject ObjOn, ObjOff;

    private void Start() => Toggle.onValueChanged.AddListener(Swap);

    // Update is called once per frame
    public void Swap(bool on)
    {
        ObjOn.SetActive(on);
        ObjOff.SetActive(!on);
    }
}
