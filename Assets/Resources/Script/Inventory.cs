using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform content;

    InventorySlot[] slots;
    Item item;

    void Start()
    {
        slots = content.GetComponentsInChildren<InventorySlot>();
    }

    public void SlotItemsUI()
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            // 1. �κ��丮 ���Կ� �������� �̹����� ���� ��
            // 2. ȹ���� �������� �̹����� ���� ������ �̹����� �����鼭, �������� ������ �ִ� ���� ������ ��
            if (slots[i].Icon.sprite == null || IsSameItem(slots[i]))
            {
                // ������ �߰� �Ǵ� ���� ����
                if (slots[i].Icon.sprite == null)
                    slots[i].AddItem(item);
                else
                    slots[i].SetItemCount(1);

                return;
            }
        }
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
