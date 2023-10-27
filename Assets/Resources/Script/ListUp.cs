using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ListUp : MonoBehaviour
{
    [SerializeField] private Button[] getUpgrade;
    [SerializeField] private RectTransform upgradeUI;
    [SerializeField] private RectTransform invenUI;
    bool moveUI;
    bool moveInven;

    void Start()
    {
        moveUI = false;
        moveInven = false;
    }

    void Update()
    {
        MoveUpgradeParts();
        MoveInventory();
    }

    public void ClickStatus()
    {
        if (moveUI == false && moveInven == false)
            moveUI = true;
        else if (moveUI == true && moveInven == false)
            moveUI = false;
        else if (moveUI == false && moveInven == true)
            moveUI = false;
    }

    public void ClickInventory()
    {
        if (moveInven == false && moveUI == false)
            moveInven = true;
        else if (moveInven == true && moveUI == false)
            moveInven = false;
        else if (moveInven == false && moveUI == true)
            moveInven = false;
    }

    void MoveUpgradeParts()
    {
        if (moveUI == true)
        {
            Vector3 targetPos = new Vector3(-799.0f, 230.0f, 0.0f);
            upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, targetPos, 0.016f);
        }
        else
        {
            Vector3 targetPos = new Vector3(-1125.0f, 230.0f, 0.0f);
            upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, targetPos, 0.016f);
        }
    }

     void MoveInventory()
    {
        if (moveInven == true)
        {
            Vector3 targetPos = new Vector3(160.0f, -705.0f, 0.0f);
            invenUI.localPosition = Vector3.Lerp(invenUI.localPosition, targetPos, 0.016f);
        }
        else
        {
            Vector3 targetPos = new Vector3(160.0f, -1030.0f, 0.0f);
            invenUI.localPosition = Vector3.Lerp(invenUI.localPosition, targetPos, 0.016f);
        }
    }
}
