using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class StrStrObj : SerializableDictionary<string, StrObj> { }
[System.Serializable]
public class StrObj : SerializableDictionary<string, GameObject> { }

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private StrStrObj items;
    private int currentpercent;

    void Update()
    {
        currentpercent = Random.Range(1, 101);
    }

    public void SpawnItem(Vector3 pos)
    {
        // ElementAt() : 지정된 시퀀스의 인덱스 요소를 반환
        // 여기에서는 SerializableDictionary<int, GameObject>의 key값인 int를 반환
         if (currentpercent <= 100 && currentpercent > 80)
             MakeItem(pos, items["Item"].Keys.ElementAt(0));
         else if (currentpercent <= 60 && currentpercent > 45)
             MakeItem(pos, items["Item"].Keys.ElementAt(1));
         else if (currentpercent <= 30 && currentpercent > 20)
             MakeItem(pos, items["Item"].Keys.ElementAt(2));
         else if (currentpercent <= 1 && currentpercent > 0)
             MakeItem(pos, items["Item"].Keys.ElementAt(3));
    }

    public void SpawnItem(float _x, float _y, float _z)
    {
        if (currentpercent <= 100 && currentpercent > 80)
            MakeItem(_x, _y, _z, items["Item"].Keys.ElementAt(0));
        else if (currentpercent <= 60 && currentpercent > 45)
            MakeItem(_x, _y, _z, items["Item"].Keys.ElementAt(1));
        else if (currentpercent <= 30 && currentpercent > 20)
            MakeItem(_x, _y, _z, items["Item"].Keys.ElementAt(2));
        else if (currentpercent <= 1 && currentpercent > 0)
            MakeItem(_x, _y, _z, items["Item"].Keys.ElementAt(3));
    }

    void MakeItem(Vector3 pos, string str)
    {
        GameObject obj = Instantiate(items["Item"].GetValueOrDefault(str),
            new Vector3(pos.x + 1.4f, pos.y - 1.25f, pos.z), Quaternion.identity, null);
        obj.name = items["Item"].GetValueOrDefault(str).name;
    }

    void MakeItem(float _x, float _y, float _z, string str)
    {
        GameObject obj = Instantiate(items["Item"].GetValueOrDefault(str),
            new Vector3(_x + 1.4f, _y - 1.25f, _z), Quaternion.identity, null);
        obj.name = items["Item"].GetValueOrDefault(str).name;
    }
}
