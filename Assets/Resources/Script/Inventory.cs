using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform itemsParent;
    [SerializeField] private List<GameObject> getItems;
    [SerializeField] private bool nonEmptySlots;
     public bool NonEmptySlots { get { return nonEmptySlots; } }

    InventorySlot[] slots;
    Item item;
    int index = 0;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();
    }

    void Update()
    {
        if (item != null && index < slots.Length)
            UpdateSlotUI(index);
    }

    void UpdateSlotUI(int _index)
    {
        if (slots[_index].IsSlot == false)   // �κ��丮 ���Կ� �������� ���� ���
            slots[_index].AddItem(item);
        else
        {
            if (item.ItemImage == slots[_index].Icon.sprite) // ȹ���� �������� �̹����� ���Կ� �ִ� ������ �̹����� ���� ��
            {
                if (slots[_index].ItemCount < item.MaxCount)    // ȹ���� �������� ������ ������ �ִ� ���� ������ ��
                    slots[_index].AddItem(item);
                else
                    UpdateSlotUI(++_index);
            }
            else
                UpdateSlotUI(++_index);
        }
        
        SlotsEmptyCheck();
        item = null;
    }

    void SlotsEmptyCheck()
    {
        for (int i = 0; i < slots.Length; ++i)
        {
            if (slots[i].IsSlot == true)
                nonEmptySlots = true;
            else
                nonEmptySlots = false;
        }
    }

    public Item GetItem(Item _item)
    {
        item = _item;
        return _item;
    }
}
