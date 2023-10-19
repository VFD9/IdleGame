using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform canvas;
    [SerializeField] private Transform prevParent;
    [SerializeField] private RectTransform rect;
    [SerializeField] private CanvasGroup canvasGroup;   // 아이템 UI의 알파값을 제어하기 위한 canvasGroup

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        prevParent = transform.parent;  // 드래그 시작시 부모의 위치를 저장
        transform.SetParent(canvas);    // 가장 최상단에 위치한 UI로 부모를 설정
        transform.SetAsLastSibling();   // 맨 앞으로 나오게 오브젝트위 순위를 변경

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 시작시 가장 최상단 UI가 부모로 설정됨
        // 드래그 종료시 가장 최상단 UI가 부모일 경우 드래그가 제대로 되지 않은 것
        // 그래서 드래그 직전에 저장해놓은 prevParent의 위치로 이동해야함
        if (transform.parent == canvas)
        {
            transform.SetParent(prevParent);
            rect.position = prevParent.GetComponent<RectTransform>().position;
        }

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}
