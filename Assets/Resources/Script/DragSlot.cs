using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;
    public InventorySlot dragSlot;
    [SerializeField] private Image imageItem;

    void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image _itemImage)
    {
        imageItem.sprite = _itemImage.sprite;
        SetColor(1);
    }

    public void SetColor(float _alpha)
    {
        Color color = imageItem.color;
        color.a = _alpha;
        imageItem.color = color;
    }
}
