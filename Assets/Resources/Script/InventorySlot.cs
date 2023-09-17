using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private bool isSlot;
    [SerializeField] private int itemCount;
    [SerializeField] private Image icon;
    [SerializeField] private Text text;
    [SerializeField] private Button itemClick;

    public bool IsSlot { get { return isSlot; } }
    public int ItemCount { get { return itemCount; } }
    public Image Icon { get { return icon; } }
    Item item;
    Vector3 pos;

    void Start()
    {
        pos = GameManager.Instance.gameGold.Goldtext.transform.position;
        isSlot = false;
    }

    void Update()
    {
        GetClickButton();
        ClearSlot();
    }

    public void AddItem(Item newItem)
    {
        isSlot = true;
        item = newItem;
        itemCount += 1;
        icon.sprite = item.ItemImage;
        icon.enabled = true;
        text.text = item.GetItemType != ItemType.Weapon ? itemCount.ToString() : null;
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
                GameManager.Instance.numberText.TakeGold(
                    item.ItemAbility, GameManager.Instance.numberText.gameObject,
                    GameManager.Instance.player.transform.position,
                    new Color(255.0f, 200.0f, 0.0f, 255.0f), 12);
            }
            else if (item.GetAbilityType == AbilityType.Heal)
                GameManager.Instance.player.currentHp(item.ItemAbility);
            else if (item.GetAbilityType == AbilityType.PowerUp)
                GameManager.Instance.player.currentAtk(item.ItemAbility);
        }
    }

    void GetClickButton()
    {
        if (icon.sprite == null)
            itemClick.enabled = false;
        else
            itemClick.enabled = true;
    }
}
