using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ListUp : MonoBehaviour
{
    [SerializeField] private Button StateButton;
    [SerializeField] private RectTransform upgradeUI;
    [SerializeField] private bool isVisible;
    [SerializeField] private Button[] GetUpgrade;

    void Update()
    {
        MoveParts(isVisible);
    }

    public void ClickStatus()
    {
        if (EventSystem.current.IsPointerOverGameObject() == StateButton.gameObject)
        {
            if (isVisible == false)
                isVisible = true;
            else
                isVisible = false;
        }
    }

    void MoveParts(bool _bool)
    {
        if (_bool == false)
        {
            Vector3 targetPos = new Vector3(-799.0f, 275.0f, 0.0f);
            upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, targetPos, 0.016f);
        }
        else
        {
            Vector3 targetPos = new Vector3(-1125.0f, 275.0f, 0.0f);
            upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, targetPos, 0.016f);
        }
    }
}
