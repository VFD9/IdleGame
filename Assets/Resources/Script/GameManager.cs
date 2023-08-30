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
        // 1000��带 ���� ��� A~Z���� ǥ���ϰ� �ƴ϶�� ǥ������ �ʱ⶧���� �빮�� 26���� ǥ������ �ʴ� �� �ͱ����ؼ� 27���̴�.
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

                // i��° �迭���� ������ ������ �迭������ 1�� ���� i��° �迭�� 1000�� �����ش�.
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
        // ���� ��带 �������� ������ �� �� �ְ� �Ѵ�.
        float a = curGold[index];

        // index�� 0���� ũ�� ���� 1000����̻��̴ϱ� 100�� �ڸ��������� ���ڸ� 1000���� ������ ������ a�� �����ش�.
        if (index > 0)
        {
            float b = curGold[index - 1];
            a += b / 1000;
        }

        // index�� 0�̶�� 1000��带 ���� �ʾұ⶧���� ���� ��忡 ������ �Ҽ����� �����Ƿ� 0�� ���Ѵ�.
        if (index == 0)
            a += 0;

        // char�� ������ �����ϰ� 1000����̻��� �� index�� 1�̻��̹Ƿ� 64 + index�� ���ϸ� �빮�� ���ĺ�(A~Z)�� ���� ���ڿ� �ش��Ѵ�.
        char unit = (char)(64 + index);
        string p;
        // Math.Truncate : �Ҽ����� ����, Math.Floor : �Ҽ����� �ø�
        // ���׿����ڸ� �̿��� unit�� 65�̻��� ��� �빮�ڸ� ǥ���� ��带 �����ְ� �ƴ϶�� ���� ���� ��带 �����ش�.
        p = unit >= (char)65 ? (float)(Math.Truncate(a * 100) / 100) + unit.ToString() : curGold[0].ToString();
        Goldtext.text = p;

        return p;
    }

    public string myMoneyToString(int[] _gold, int _index, Text _text)
    {
        // ���� ��带 �������� ������ �� �� �ְ� �Ѵ�.
        float a = _gold[_index];

        // index�� 0���� ũ�� ���� 1000����̻��̴ϱ� 100�� �ڸ��������� ���ڸ� 1000���� ������ ������ a�� �����ش�.
        if (_index > 0)
        {
            float b = _gold[_index - 1];
            a += b / 1000;
        }

        // index�� 0�̶�� 1000��带 ���� �ʾұ⶧���� ���� ��忡 ������ �Ҽ����� �����Ƿ� 0�� ���Ѵ�.
        if (_index == 0)
            a += 0;

        // char�� ������ �����ϰ� 1000����̻��� �� index�� 1�̻��̹Ƿ� 64 + index�� ���ϸ� �빮�� ���ĺ�(A~Z)�� ���� ���ڿ� �ش��Ѵ�.
        char unit = (char)(64 + _index);
        string p;
        // Math.Truncate : �Ҽ����� ����, Math.Floor : �Ҽ����� �ø�
        // ���׿����ڸ� �̿��� unit�� 65�̻��� ��� �빮�ڸ� ǥ���� ��带 �����ְ� �ƴ϶�� ���� ���� ��带 �����ش�.
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
