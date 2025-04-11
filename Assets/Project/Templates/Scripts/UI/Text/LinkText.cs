using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
public class LinkText : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private UnityEvent<string> _onClickLink;
    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(_text, eventData.position, eventData.pressEventCamera);

        if (linkIndex == -1) return;
        TMP_LinkInfo linkInfo = _text.textInfo.linkInfo[linkIndex];
        Debug.Log(linkInfo.GetLinkID());
        _onClickLink?.Invoke(linkInfo.GetLinkID());
    }
}
