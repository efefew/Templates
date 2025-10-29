using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Joystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{

    #region Properties

    public Vector2 InputVector { get; private set; }

    #endregion Properties

    #region Fields

    [FormerlySerializedAs("joystick")] [SerializeField]
    private Image _joystick;

    [FormerlySerializedAs("joystickBackground")] [SerializeField]
    private Image _joystickBackground;

    [FormerlySerializedAs("joystickArea")] [SerializeField]
    private Image _joystickArea;

    private Vector2 _joystickBackgroundStartPosition;
    //[SerializeField] private Color inActiveJoystickColor;
    //[SerializeField] private Color activeJoystickColor;

    //private bool joystickIsActive = false;

    #endregion Fields

    #region Methods

    private void Start() =>
        //ClickEffect();
        _joystickBackgroundStartPosition = _joystickBackground.rectTransform.anchoredPosition;

    //private void ClickEffect()
    //{
    //    if (!joystickIsActive)
    //    {
    //        joystick.color = activeJoystickColor;
    //        joystickIsActive = true;
    //    }
    //    else
    //    {
    //        joystick.color = inActiveJoystickColor;
    //        joystickIsActive = false;
    //    }
    //}

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickBackground.rectTransform, eventData.position, null, out Vector2 joystickPosition))
        {
            joystickPosition.x *= 2 / _joystickBackground.rectTransform.sizeDelta.x;
            joystickPosition.y *= 2 / _joystickBackground.rectTransform.sizeDelta.y;

            InputVector = joystickPosition.magnitude > 1f ? joystickPosition.normalized : joystickPosition;

            _joystick.rectTransform.anchoredPosition = new Vector2(InputVector.x * _joystickBackground.rectTransform.sizeDelta.x / 2, InputVector.y * _joystickBackground.rectTransform.sizeDelta.y / 2);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //ClickEffect();
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_joystickArea.rectTransform, eventData.position, null, out Vector2 joystickBackgroundPosition))
            _joystickBackground.rectTransform.anchoredPosition = joystickBackgroundPosition;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _joystickBackground.rectTransform.anchoredPosition = _joystickBackgroundStartPosition;

        //ClickEffect();

        InputVector = Vector2.zero;
        _joystick.rectTransform.anchoredPosition = Vector2.zero;
    }

    #endregion Methods

}