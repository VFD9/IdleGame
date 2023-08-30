using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public Transform UI;
    [Header("STAGE")]
    public uint Stagenum;
    public Text Stage;
    [Header("USER")]
    public Player player;
    public Text currentHp;
    public Text fullHp;
    public Image Hpbar;
    public GameObject monsterHpPrefab;
    public Skill skill;
    public float userSpeed;
    [Header("GOLD")]
    public int[] curGold;
    public int getGold;
    public Text Goldtext;
    public int index;
    [Space(20.0f)]
    public Transform thunderParent;
    public Transform thunderPos;
    [Header("Sound")]
    public AudioSource clickSound;

    void Start()
    {
        StartCoroutine(earnGold());
    }

    void Update()
    {
        goldTheorem();
        myMoneyToString();
        getStage();
    }

    void getStage()
    {
        Stage.text = "STAGE " + Stagenum.ToString();
    }

    IEnumerator earnGold()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);

        while (true)
        {
            yield return waitForSeconds;

            if (curGold[26] >= 999)
                curGold[26] = 999;
            else
                curGold[0] += getGold;
        }
    }

    void goldTheorem()
    {
        // 1000골드를 넘을 경우 A~Z까지 표기하고 아니라면 표기하지 않기때문에 대문자 26개와 표기하지 않는 빈 것까지해서 27개이다.
        for (int i = 0; i < 27; ++i)
        {
            if (curGold[i] > 0)
                index = i;
        }

        for (int i = 0; i <= index; ++i)
        {
            if (index < 26)
            {
                if (curGold[i] > 1000)
                {
                    curGold[i] -= 1000;
                    curGold[i + 1] += 1;
                }

                // i번째 배열값이 음수일 경우다음 배열값에서 1을 빼고 i번째 배열에 1000을 더해준다.
                if (curGold[i] < 0)
                {
                    if (index > i)
                    {
                        index = i;
                        curGold[i + 1] -= 1;
                        curGold[i] += 1000;
                    }
                }
            }
        }
    }

    public void goldTheorem(int[] _gold, int _index)
    {
        for (int i = 0; i < 27; ++i)
        {
            if (_gold[i] > 0)
                _index = i;
        }

        for (int i = 0; i <= _index; ++i)
        {
            if (_index < 26)
            {
                if (_gold[i] > 1000)
                {
                    _gold[i] -= 1000;
                    _gold[i + 1] += 1;
                }
            }
        }
    }

    string myMoneyToString()
    {
        // 현재 골드를 수소점을 포함해 볼 수 있게 한다.
        float a = curGold[index];

        // index가 0보다 크면 현재 1000골드이상이니까 100의 자리수까지의 숫자를 1000으로 나눠서 선언한 a에 더해준다.
        if (index > 0)
        {
            float b = curGold[index - 1];
            a += b / 1000;
        }

        // index가 0이라면 1000골드를 넘지 않았기때문에 현재 골드에 보여줄 소수점이 없으므로 0을 더한다.
        if (index == 0)
            a += 0;

        // char형 변수를 선언하고 1000골드이상일 때 index가 1이상이므로 64 + index를 더하면 대문자 알파벳(A~Z)가 가진 숫자에 해당한다.
        char unit = (char)(64 + index);
        string p;
        // Math.Truncate : 소수점을 버림, Math.Floor : 소수점을 올림
        // 삼항연산자를 이용해 unit이 65이상일 경우 대문자를 표기해 골드를 보여주고 아니라면 현재 가진 골드를 보여준다.
        p = unit >= (char)65 ? (float)(Math.Truncate(a * 100) / 100) + unit.ToString() : curGold[0].ToString();
        Goldtext.text = p;

        return p;
    }

    public string myMoneyToString(int[] _gold, int _index, Text _text)
    {
        // 현재 골드를 수소점을 포함해 볼 수 있게 한다.
        float a = _gold[_index];

        // index가 0보다 크면 현재 1000골드이상이니까 100의 자리수까지의 숫자를 1000으로 나눠서 선언한 a에 더해준다.
        if (_index > 0)
        {
            float b = _gold[_index - 1];
            a += b / 1000;
        }

        // index가 0이라면 1000골드를 넘지 않았기때문에 현재 골드에 보여줄 소수점이 없으므로 0을 더한다.
        if (_index == 0)
            a += 0;

        // char형 변수를 선언하고 1000골드이상일 때 index가 1이상이므로 64 + index를 더하면 대문자 알파벳(A~Z)가 가진 숫자에 해당한다.
        char unit = (char)(64 + _index);
        string p;
        // Math.Truncate : 소수점을 버림, Math.Floor : 소수점을 올림
        // 삼항연산자를 이용해 unit이 65이상일 경우 대문자를 표기해 골드를 보여주고 아니라면 현재 가진 골드를 보여준다.
        p = unit >= (char)65 ? (float)(Math.Truncate(a * 100) / 100) + unit.ToString() : _gold[0].ToString();
        _text.text = p;

        return p;
    }

    public void StageUp()
    {
        ObjectPool.Instance.MakeMonster(10.0f);
        Stagenum += 1;
    }

    public void StageDown()
    {
        ObjectPool.Instance.MakeMonster(10.0f);

        if (Stagenum != 1)
            Stagenum -= 1;
        else
            Stagenum = 1;
    }
}
