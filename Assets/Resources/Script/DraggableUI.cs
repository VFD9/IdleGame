using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Transform canvas;
    [SerializeField] private Transform prevParent;
    [SerializeField] private RectTransform rect;
    [SerializeField] private CanvasGroup canvasGroup;   // ������ UI�� ���İ��� �����ϱ� ���� canvasGroup

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        rect = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        prevParent = transform.parent;  // �巡�� ���۽� �θ��� ��ġ�� ����
        transform.SetParent(canvas);    // ���� �ֻ�ܿ� ��ġ�� UI�� �θ� ����
        transform.SetAsLastSibling();   // �� ������ ������ ������Ʈ�� ������ ����

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� ���۽� ���� �ֻ�� UI�� �θ�� ������
        // �巡�� ����� ���� �ֻ�� UI�� �θ��� ��� �巡�װ� ����� ���� ���� ��
        // �׷��� �巡�� ������ �����س��� prevParent�� ��ġ�� �̵��ؾ���
        if (transform.parent == canvas)
        {
            transform.SetParent(prevParent);
            rect.position = prevParent.GetComponent<RectTransform>().position;
        }

        canvasGroup.alpha = 1.0f;
        canvasGroup.blocksRaycasts = true;
    }
}
