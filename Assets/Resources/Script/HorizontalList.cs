using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalList : MonoBehaviour
{
    [SerializeField] private GameObject slotPrefab;
    [SerializeField] private int slotCount;

    void Start()
    {
        AddSlot();
    }

    public void AddSlot()
    {
        for (int i = 0; i < slotCount; ++i)
        {
            GameObject Obj = Instantiate(slotPrefab, transform);
            Obj.name = "Slot";
        }
    }
}
