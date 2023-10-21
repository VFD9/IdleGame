using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private string itemName;
    [SerializeField] private int maxCount;
    [Multiline]
    [SerializeField] private string description;
    [SerializeField] private ItemType itemType;
    [SerializeField] private AbilityType itemAbilityType;
    [SerializeField] private int itemAbility;
    [SerializeField] private Sprite itemImage;

    public enum ItemType
    {
        Gold, Potion, Weapon, Misc
    }

    public enum AbilityType
    {
        GoldUp, Heal, PowerUp, None
    }

    public int MaxCount { get { return maxCount; } }
    public int ItemAbility { get { return itemAbility; } }
    public ItemType GetItemType { get { return itemType; } }
    public AbilityType ItemAbilityType { get { return itemAbilityType; } }
    public Sprite ItemImage { get { return itemImage; } }
}
