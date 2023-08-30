using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    public GameObject MonsterParentObject;
    public int monsterCount;
    [Header("Monster's name")]
    public List<GameObject> monsterName;
    [Header("MonsterList")]
    public List<GameObject> makeMonsters;

    void Start()
    {
        MakeMonster(10.0f);
    }

    public void MakeMonster(float _x)
    {
        for (int i = 0; i < monsterCount; ++i)
        {
            makeMonsters.Add(Instantiate(monsterName[Random.Range(0, 4)],
                new Vector3(_x + (3.0f * i), -1.15f, 0.0f),
                Quaternion.identity, MonsterParentObject.transform));

            int index = makeMonsters[i].name.IndexOf("(Clone)");
            if (index > 0)
                makeMonsters[i].name = makeMonsters[i].name.Substring(0, index) + i.ToString();
        }
    }

    public void MakeMonster(GameObject _Obj, float _x)
    {
        for (int i = 0; i < monsterCount; ++i)
        {
            makeMonsters.Add(Instantiate(_Obj,
                new Vector3(_x + (5.0f * i), -1.15f, 0.0f),
                Quaternion.identity, MonsterParentObject.transform));

            int index = makeMonsters[i].name.IndexOf("(Clone)");
            if (index > 0)
                makeMonsters[i].name = makeMonsters[i].name.Substring(0, index) + i.ToString();
        }
    }

    public void PullObject(GameObject _Obj)
    {
        _Obj.SetActive(false);
        _Obj.GetComponent<BoxCollider2D>().enabled = true;
        makeMonsters.RemoveAt(0);
    }

    public void DestroyChild()
    {
        makeMonsters.Clear();

        for (int i = 0; i < MonsterParentObject.transform.childCount; ++i)
            Destroy(MonsterParentObject.transform.GetChild(i).gameObject);
    }
}
