using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DroppableUI : MonoBehaviour, IPointerEnterHandler, IDropHandler, IPointerExitHandler
{
    [SerializeField] private Image image;
    [SerializeField] private RectTransform rect;

    private void Awake()
    {
        image = GetComponent<Image>();
        rect = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(0, 255, 0, 32);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null && rect.childCount != 0)
        {
            eventData.pointerDrag.transform.SetParent(transform);
            eventData.pointerDrag.GetComponent<RectTransform>().position = rect.position;
        }
    }
}
