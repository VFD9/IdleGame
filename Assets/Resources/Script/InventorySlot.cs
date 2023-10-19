using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private bool isSlot;
    [SerializeField] private int itemCount;
    [SerializeField] private Image icon;
    [SerializeField] private Button itemClick;
    [SerializeField] private Text text;

    public bool IsSlot { get { return isSlot; } }
    public int ItemCount { get { return itemCount; } }
    public Image Icon { get { return icon; } }

    Item item;
    Vector3 goldPos;

    void Start()
    {
        goldPos = GameManager.Instance.gameGold.Goldtext.transform.position;
        isSlot = false;
    }

    void Update()
    {
        ClearSlot();
    }

    // TODO : 아이템 습득 후 인벤토리 슬롯에 이미지가 뜨도록 수정해야함
    public void AddItem(Item newItem)
    {
        isSlot = true;
        item = newItem;
        itemCount += 1;
        icon.sprite = item.ItemImage;
        text.text = itemCount.ToString();
    }

    void ClearSlot()
    {
        if (itemCount == 0)
        {
            isSlot = false;
            icon.sprite = null;
            icon.enabled = false;
            text.text = null;
        }
    }

    void ItemCountDown()
    {
        itemCount -= 1;
        text.text = ItemCount.ToString();
    }

    public int ReturnAbility()
    {
        return item.ItemAbility;
    }

    public AbilityType ReturnType()
    {
        return item.GetAbilityType;
    }

    public void UseItem()
    {
        if (item.GetAbilityType == AbilityType.None)
            itemCount -= 0;
        else
        {
            ItemCountDown();

            if (item.GetAbilityType == AbilityType.GoldUp)
            {
                GameManager.Instance.gameGold.curGold[0] += item.ItemAbility;
                GameManager.Instance.numberText.TakeName("Gold", item.ItemAbility,
                    GameManager.Instance.numberText.gameObject,
                    GameManager.Instance.player.transform.position,
                    new Color(255, 200, 0, 255), 13);
            }
            else if (item.GetAbilityType == AbilityType.Heal)
                GameManager.Instance.player.currentHp(item.ItemAbility);
            else if (item.GetAbilityType == AbilityType.PowerUp)
            {
                GameManager.Instance.player.currentAtk(item.ItemAbility);
                GameManager.Instance.numberText.TakeName("ATK", item.ItemAbility,
                    GameManager.Instance.numberText.gameObject,
                    GameManager.Instance.player.transform.position,
                    new Color(0, 0, 0, 255), 15);
            }
        }
    }
}
