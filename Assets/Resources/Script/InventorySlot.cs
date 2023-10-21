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
        itemCount += itemCount < item.MaxCount ? count : 0;     // 아이템이 가진 최대값보다 작을 때 count를 더한다
        itemCountText.text = itemCount.ToString();

        if (itemCount <= 0)
            ClearSlot();
    }

    public void UseItem()
    {
        if (item != null)
        {
            if (item.ItemAbilityType == Item.AbilityType.None)  // 아이템 능력이 아무것도 없다면 사용되지 않게한다
                itemCount -= 0;
            else
            {
                if (item.ItemAbilityType == Item.AbilityType.GoldUp)    // 아이템 능력이 골드 증가일 때
                {
                    GameManager.Instance.gameGold.curGold[0] += item.ItemAbility;
                    GameManager.Instance.numberText.ItemTypeName("Gold", item.ItemAbility,
                        GameManager.Instance.numberText.gameObject,
                        GameManager.Instance.player.transform.position,
                        new Color(255, 200, 0, 255), 13);
                }
                else if (item.ItemAbilityType == Item.AbilityType.Heal) // 아이템 능력이 체력 회복일 때
                    GameManager.Instance.player.currentHp(item.ItemAbility);
                else if (item.ItemAbilityType == Item.AbilityType.PowerUp)  // 아이템 능력이 공격력 증가일 때
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
            DragSlot.instance.dragSlot = this;                          // 이 스크립트가 들어간 객체를 dragSlot에 넣는다
            DragSlot.instance.DragSetImage(icon);                       // 드래그한 객체의 이미지가 보이게 수정
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
        DragSlot.instance.SetColor(0);      // 드래그가 끝난 dragslot을 보이지 않게 수정
        DragSlot.instance.dragSlot = null;  // dragㄴlot을 비운다
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.dragSlot != null && DragSlot.instance.dragSlot != this)
            SwapSlot();
    }

    void SwapSlot()
    {
        Item tempItem = item;           // 아이템이 이미 있는 슬롯으로 옮길때 기존에 있는 아이템을 임시로 저장
        int tempItemCount = itemCount;  // 마찬가지로 기존에 있는 아이템의 갯수를 임시로 저장

        AddItem(DragSlot.instance.dragSlot.item, DragSlot.instance.dragSlot.itemCount);

        if (tempItem != null)   // 바꾸려는 슬롯에 아이템이 존재할 경우
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
        else      // 바꾸려는 슬롯에 아이템이 없을 경우
            DragSlot.instance.dragSlot.ClearSlot();
    }
}