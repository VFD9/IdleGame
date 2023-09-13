using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StrIntObj : SerializableDictionary<string, IntObj> { }
[System.Serializable]
public class IntObj : SerializableDictionary<int, GameObject> { }

public class DropItem : MonoBehaviour
{
    [SerializeField] private StrIntObj dropItem;
    [SerializeField] private int currentpercent;
    private int sumPercent;

    void Start()
    {
        Probability();
    }

    void Update()
    {
        currentpercent = Random.Range(1, 101);
    }

    // TODO : Ȯ���� ���� �������� �������� �ڵ带 �����ؾ���
    public void DropTable(Vector3 pos)
    {
        for (int i = 0; i < dropItem["Item"].Keys.ToList().Count; ++i)
        {
            // ElementAt() : ������ �������� �ε��� ��Ҹ� ��ȯ
            // ���⿡���� SerializableDictionary<int, GameObject>�� key���� int�� ��ȯ
            if (dropItem["Item"].Keys.ElementAt(i) == currentpercent) 
            {
                MakeItem(pos, dropItem["Item"].Keys.ElementAt(i));
            }
        }
    }

    public void DropTable(float _x, float _y, float _z)
    {
        for (int i = 0; i < dropItem["Item"].Keys.ToList().Count; ++i)
        {
            // ElementAt() : ������ �������� �ε��� ��Ҹ� ��ȯ
            // ���⿡���� SerializableDictionary<int, GameObject>�� key���� int�� ��ȯ
            if (dropItem["Item"].Keys.ElementAt(i) == currentpercent)
            {
                MakeItem(_x, _y, _z, dropItem["Item"].Keys.ElementAt(i));
            }
        }
    }

    void MakeItem(Vector3 pos,int num)
    {
        GameObject obj = Instantiate(dropItem["Item"].GetValueOrDefault(num), 
            new Vector3(pos.x + 1.2f, pos.y - 1.25f, pos.z), Quaternion.identity, null);
        obj.name = dropItem["Item"].GetValueOrDefault(num).name;
    }

    void MakeItem(float _x, float _y, float _z, int num)
    {
        GameObject obj = Instantiate(dropItem["Item"].GetValueOrDefault(num), 
            new Vector3(_x + 1.2f, _y - 1.25f, _z), Quaternion.identity, null);
        obj.name = dropItem["Item"].GetValueOrDefault(num).name;
    }

    void Probability()
    {
        for (int i = 0; i < dropItem["Item"].Keys.ToList().Count; ++i)
            sumPercent += dropItem["Item"].Keys.ElementAt(i);
    }
}
