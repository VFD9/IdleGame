using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UpgradeStatus : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Text goldText;
    [SerializeField] private int[] spendGold;
    [SerializeField] private int goldIndex;
    [SerializeField] private int clickCount;
    [SerializeField] private int maxCount;
    Player playerStatus;

    void Start()
    {
        goldText.text = spendGold.ToString();
        playerStatus = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        UpIndex();
        DisplayGoldText();
    }

    void UpIndex()
    {
        if (spendGold[goldIndex] > 1000)
            goldIndex += 1;
    }

    void DisplayGoldText()
    {
        GameManager.Instance.goldTheorem(spendGold, goldIndex);
        GameManager.Instance.myMoneyToString(spendGold, goldIndex, goldText);

        if (clickCount == maxCount)
        {
            goldText.text = "max";
            button.enabled = false;
        }
    }

    public void SpendGold()
    {
        if (clickCount <= maxCount)
        {
            if (GameManager.Instance.index == goldIndex)
            {
                if (GameManager.Instance.curGold[goldIndex] >= spendGold[goldIndex])
                {
                    GameManager.Instance.curGold[goldIndex] -= spendGold[goldIndex];
                    IncreaseGold();
                }
                else
                    spendGold[goldIndex] += 0;
            }
            else if (GameManager.Instance.index > goldIndex)
            {
                if (GameManager.Instance.curGold[goldIndex] >= spendGold[goldIndex])
                    GameManager.Instance.curGold[goldIndex] -= spendGold[goldIndex];
                else
                {
                    GameManager.Instance.curGold[goldIndex] = GameManager.Instance.curGold[goldIndex] + 1000 - spendGold[goldIndex];
                    GameManager.Instance.curGold[goldIndex + 1] -= 1;
                }
                IncreaseGold();
            }
            else if (GameManager.Instance.index < goldIndex)
                spendGold[goldIndex] += 0;
        }
    }

    void IncreaseGold()
    {
        if (goldIndex >= 1)
        {
            for (int j = 1; j <= goldIndex; ++j)
            {
                if (spendGold[j] / 10 == 0)
                    spendGold[j] += spendGold[j] / 2;
                else
                    spendGold[j] += spendGold[j] / 6;

                spendGold[j - 1] += spendGold[j - 1] < 100 ? spendGold[j - 1] * 3 : spendGold[j - 1];
            }
        }
        else
            spendGold[goldIndex] += (spendGold[goldIndex] / 4) + (spendGold[goldIndex] / 5);
    }

    public void AddAtk()
    {
        if (GameManager.Instance.index == goldIndex)
        {
            if (GameManager.Instance.curGold[goldIndex] >= spendGold[goldIndex])
            {
                Click();
                playerStatus.currentAtk(0.24f);
            }
        }
        else if (GameManager.Instance.index > goldIndex)
        {
            Click();
            playerStatus.currentAtk(0.24f);
        }
    }

    public void AddHp()
    {
        if (GameManager.Instance.index == goldIndex)
        {
            if (GameManager.Instance.curGold[goldIndex] >= spendGold[goldIndex])
            {
                Click();
                playerStatus.HpUp(UnityEngine.Random.Range(16.0f, 20.0f));
            }
        }
        else if (GameManager.Instance.index > goldIndex)
        {
            Click();
            playerStatus.HpUp(UnityEngine.Random.Range(16.0f, 20.0f));
        }
    }

    public void AddAtkSpeed()
    {
        if (GameManager.Instance.index == goldIndex)
        {
            if (GameManager.Instance.curGold[goldIndex] >= spendGold[goldIndex])
            {
                Click();
                playerStatus.getAttackSpeed(0.02f);
            }
        }
        else if (GameManager.Instance.index > goldIndex)
        {
            Click();
            playerStatus.getAttackSpeed(0.02f);
        }
    }

    public void AddMoveSpeed()
    {
        if (GameManager.Instance.index == goldIndex)
        {
            if (GameManager.Instance.curGold[goldIndex] >= spendGold[goldIndex])
            {
                Click();
                playerStatus.getMoveSpeed(0.02f);
            }
        }
        else if (GameManager.Instance.index > goldIndex)
        {
            Click();
            playerStatus.getMoveSpeed(0.02f);
        }
    }

    public void AddSkillPower()
    {
        if (GameManager.Instance.index == goldIndex)
        {
            if (GameManager.Instance.curGold[goldIndex] >= spendGold[goldIndex])
            {
                Click();
                GameManager.Instance.skill.GetThunder().addPower(0.12f);
            }
        }
        else if (GameManager.Instance.index > goldIndex)
        {
            Click();
            GameManager.Instance.skill.GetThunder().addPower(0.12f);
        }
    }

    public void AddEarnGold()
    {
        if (GameManager.Instance.index == goldIndex)
        {
            if (GameManager.Instance.curGold[goldIndex] >= spendGold[goldIndex])
            {
                Click();
                GameManager.Instance.getGold += 4 * clickCount;
            }
        }
        else if (GameManager.Instance.index > goldIndex)
        {
            Click();
            GameManager.Instance.getGold += 4 * clickCount;
        }
    }

    public void Click()
    {
        GameManager.Instance.statusClickSound.Play();
        clickCount += 1;
    }
}
