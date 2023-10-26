using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private GameObject horizontalListPrefab;
    [SerializeField] private RectTransform addListButton;

    InventorySlot[] slots;
    Item item;

    void Start()
    {
        StartCoroutine(FindSlots());
    }

    public void SlotItemsUI()
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            // 1. �κ��丮 ���Կ� �������� �̹����� ���� ��
            // 2. ȹ���� �������� �̹����� ���� ������ �̹����� �����鼭, �������� ������ �ִ� ���� ������ ��
            if (!slots[i].haveItem || IsSameItem(slots[i]))
            {
                // ������ �߰� �Ǵ� ���� ����
                if (!slots[i].haveItem)
                    slots[i].AddItem(item);
                else
                    slots[i].SetItemCount(1);

                return;
            }
        }
    }

    public void MakeList()
    {
        GameObject Obj = Instantiate(horizontalListPrefab, content);
        Obj.name = "HorizontalList";
        addListButton.SetAsLastSibling();
        StartCoroutine(FindSlots());

        if (Mathf.Abs(addListButton.anchoredPosition.y) > content.rect.height - 117)
            addListButton.gameObject.SetActive(false);
    }

    IEnumerator FindSlots()
    {
        yield return null;
        slots = content.GetComponentsInChildren<InventorySlot>();
    }

    bool IsSameItem(InventorySlot slot)
    {
        return item.ItemImage == slot.Icon.sprite && slot.ItemCount < item.MaxCount;
    }

    public Item GetItem(Item _item)
    {
        item = _item;
        return _item;
    }
}
