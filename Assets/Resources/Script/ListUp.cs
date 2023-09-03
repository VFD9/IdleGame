using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ListUp : MonoBehaviour
{
    [SerializeField] private SerializableDictionary<Button, bool> CheckBool;
    [SerializeField] private Button[] ClickButton = new Button[2];
    [SerializeField] private Button[] GetUpgrade;
    [SerializeField] private RectTransform upgradeUI;

    private void Start()
    {
        CheckBool.Add(ClickButton[0], false);
        CheckBool.Add(ClickButton[1], false);
    }

    void Update()
    {
        MoveParts();
    }

    public void ClickStatus(string _name)
    {
        for (int i = 0; i < ClickButton.Length; ++i)
        {
            if (EventSystem.current.IsPointerOverGameObject() == ClickButton[i].gameObject
                && ClickButton[i].name == _name)
            {
                if (CheckBool.ContainsValue(false))
                    CheckBool[ClickButton[i]] = true;
                else if (CheckBool.ContainsValue(true))
                    CheckBool[ClickButton[i]] = false;
            }
        }
    }

    // TODO : 버튼 클릭시 UI가 나오지 않는 문제 수정
    void MoveParts()
    {
        if (CheckBool.ContainsValue(false))
        {
            Vector3 targetPos = new Vector3(-1125.0f, 230.0f, 0.0f);
            upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, targetPos, 0.016f);

            //if (_bool == true)
            //{
            //    Vector3 targetPos = new Vector3(-799.0f, 230.0f, 0.0f);
            //    upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, targetPos, 0.016f);
            //}
            //else
            //{
            //    Vector3 targetPos = new Vector3(-1125.0f, 230.0f, 0.0f);
            //    upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, targetPos, 0.016f);
            //}
        }
        else if (CheckBool.ContainsValue(true))
        {
            Vector3 targetPos = new Vector3(-799.0f, 230.0f, 0.0f);
            upgradeUI.localPosition = Vector3.Lerp(upgradeUI.localPosition, targetPos, 0.016f);
        }
    }
}
