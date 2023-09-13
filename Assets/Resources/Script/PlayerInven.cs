using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInven : MonoBehaviour
{
    [SerializeField] private Transform itemLocker;
    [SerializeField] private List<GameObject> itemList;

    void Start()
    {
        itemList.Clear();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item"))
        {
            itemList.Add(collision.gameObject);
            collision.transform.SetParent(itemLocker);
        }
    }
}
