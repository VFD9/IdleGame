using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class ItemInfo : MonoBehaviour
{
    [SerializeField] private Image canvasInItem;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            gameObject.SetActive(false);
    }
}
