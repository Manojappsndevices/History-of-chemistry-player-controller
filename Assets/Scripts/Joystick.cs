using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Joystick : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private Image handle;
    [SerializeField] private float maxRadius = 100f;

    private Vector2 inputVector;
    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        handle.rectTransform.anchoredPosition = Vector2.zero;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out position
        );

        position = Vector2.ClampMagnitude(position, maxRadius);
        handle.rectTransform.anchoredPosition = position;

        inputVector = position / maxRadius;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        handle.rectTransform.anchoredPosition = Vector2.zero;
        inputVector = Vector2.zero;
    }

    public Vector2 GetInputDirection()
    {
        return new Vector2(inputVector.x, inputVector.y);
    }
}