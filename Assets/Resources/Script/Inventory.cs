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
            // 1. 인벤토리 슬롯에 아이템의 이미지가 없을 때
            // 2. 획득한 아이템의 이미지와 슬롯 아이템 이미지가 같으면서, 아이템의 갯수가 최대 갯수 이하일 때
            if (slots[i].Icon.sprite == null || IsSameItem(slots[i]))
            {
                // 아이템 추가 또는 갯수 증가
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
