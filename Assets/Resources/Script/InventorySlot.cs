using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public bool haveItem;
    [SerializeField] private Item item;
    [SerializeField] private int itemCount;
    [SerializeField] private Image icon;
    [SerializeField] private Button itemClick;
    [SerializeField] private Text itemCountText;

    public int ItemCount { get { return itemCount; } }
    public Image Icon { get { return icon; } }

    void Start()
    {
        haveItem = false;
    }

    public void AddItem(Item newItem, int count = 1)
    {
        haveItem = true;
        item = newItem;
        itemCount = count;
        icon.sprite = item.ItemImage;
        itemCountText.text = item.GetItemType != Item.ItemType.Weapon ? itemCount.ToString() : "";
        SetColor(1);
    }

    void ClearSlot()
    {
        haveItem = false;
        item = null;
        itemCount = 0;
        icon.sprite = null;
        itemCountText.text = "";
        SetColor(0);
    }

    void SetColor(float _alpha)
    {
        Color color = icon.color;
        color.a = _alpha;
        icon.color = color;
    }

    public void SetItemCount(int count)
    {
        itemCount += itemCount < item.MaxCount ? count : 0;     // �������� ���� �ִ밪���� ���� �� count�� ���Ѵ�
        itemCountText.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    public void UseItem()
    {
        if (item != null)
        {
            if (item.ItemAbilityType == Item.AbilityType.None)  // ������ �ɷ��� �ƹ��͵� ���ٸ� ������ �ʰ��Ѵ�
                itemCount -= 0;
            else
            {
                if (item.ItemAbilityType == Item.AbilityType.GoldUp)    // ������ �ɷ��� ��� ������ ��
                {
                    GameManager.Instance.gameGold.curGold[0] += item.ItemAbility;
                    GameManager.Instance.numberText.ItemTypeName("Gold", item.ItemAbility,
                        GameManager.Instance.numberText.gameObject,
                        GameManager.Instance.player.transform.position,
                        new Color(255, 200, 0, 255), 13);
                }
                else if (item.ItemAbilityType == Item.AbilityType.Heal) // ������ �ɷ��� ü�� ȸ���� ��
                    GameManager.Instance.player.currentHp(item.ItemAbility);
                else if (item.ItemAbilityType == Item.AbilityType.PowerUp)  // ������ �ɷ��� ���ݷ� ������ ��
                {
                    GameManager.Instance.player.currentAtk(item.ItemAbility);
                    GameManager.Instance.numberText.ItemTypeName("ATK", item.ItemAbility,
                        GameManager.Instance.numberText.gameObject,
                        GameManager.Instance.player.transform.position,
                        new Color(0, 0, 0, 255), 15);
                }
                SetItemCount(-1);
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;                          // �� ��ũ��Ʈ�� �� ��ü�� dragSlot�� �ִ´�
            DragSlot.instance.DragSetImage(icon);                       // �巡���� ��ü�� �̹����� ���̰� ����
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (item != null)
            DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        DragSlot.instance.SetColor(0);      // �巡�װ� ���� dragslot�� ������ �ʰ� ����
        DragSlot.instance.dragSlot = null;  // drag��lot�� ����
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null && DragSlot.instance.dragSlot != this)
            SwapSlot();
    }

    void SwapSlot()
    {
        Item tempItem = item;           // �������� �̹� �ִ� �������� �ű涧 ������ �ִ� �������� �ӽ÷� ����
        int tempItemCount = itemCount;  // ���������� ������ �ִ� �������� ������ �ӽ÷� ����

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (tempItem != null)   // �ٲٷ��� ���Կ� �������� ������ ���
        {
            if (tempItem.name != DragSlot.instance.dragSlot.item.name)
                DragSlot.instance.dragSlot.AddItem(tempItem, tempItemCount);
            else
            {
                int addItemCount = tempItemCount + DragSlot.instance.dragSlot.itemCount;

                if (addItemCount <= item.MaxCount)
                {
                    AddItem(DragSlot.instance.dragSlot.item, tempItemCount + DragSlot.instance.dragSlot.itemCount);
                    DragSlot.instance.dragSlot.ClearSlot();
                }
                else
                {
                    AddItem(DragSlot.instance.dragSlot.item, item.MaxCount);
                    DragSlot.instance.dragSlot.AddItem(DragSlot.instance.dragSlot.item, addItemCount - item.MaxCount);
                }
            }
        }
        else      // �ٲٷ��� ���Կ� �������� ���� ���
            DragSlot.instance.dragSlot.ClearSlot();
    }
}