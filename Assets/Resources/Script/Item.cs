using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Gold, Potion, Weapon, Misc
}

public enum AbilityType
{
    GoldUp, Heal, PowerUp, None
}

public class Item : MonoBehaviour
{
    [SerializeField] private Transform itemParent;
    [SerializeField] private string itemName;
    [SerializeField] private int maxCount;
    [Multiline]
    [SerializeField] private string description;
    [SerializeField] private ItemType itemType;
    [SerializeField] private AbilityType itemAbilityType;
    [SerializeField] private int itemAbility;
    [SerializeField] private Sprite itemImage;

    public int MaxCount { get { return maxCount; } }
    public int ItemAbility { get { return itemAbility; } }
    public ItemType GetItemType { get { return itemType; } }
    public AbilityType GetAbilityType { get { return itemAbilityType; } }
    public Sprite ItemImage { get { return itemImage; } }

    void Start()
    {
        itemParent = FindObjectOfType<Inventory>().transform;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (GameManager.Instance.inventory.NonEmptySlots == false)
            {
                gameObject.SetActive(false);
                transform.SetParent(itemParent);
                GameManager.Instance.inventory.GetItem(gameObject.GetComponent<Item>());
            }
        }
    }
}
