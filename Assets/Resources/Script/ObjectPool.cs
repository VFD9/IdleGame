using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPool : Singleton<ObjectPool>
{
    [Header("STAGE")]
    public uint Stagenum;
    public Text Stage;
    [Space(15.0f)]
    public GameObject MonsterParentObject;
    public int monsterCount;
    [Header("Monster's name")]
    public List<GameObject> monsterName;
    [Header("MonsterList")]
    public List<GameObject> makeMonsters;

    void Start()
    {
        MakeMonster(7.0f);
    }

    void Update()
    {
        getStage();
    }

    public void MakeMonster(float _x)
    {
        for (int i = 0; i < monsterCount; ++i)
        {
            GameObject obj = Instantiate(monsterName[Random.Range(0, 4)],
                new Vector3(_x + (4.0f * i), -1.15f, 0.0f),
                Quaternion.identity, MonsterParentObject.transform);
            makeMonsters.Add(obj);

            int index = makeMonsters[i].name.IndexOf("(Clone)");
            if (index > 0)
                makeMonsters[i].name = makeMonsters[i].name.Substring(0, index) + (i + 1).ToString();
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

    void getStage()
    {
        Stage.text = "STAGE " + Stagenum.ToString();
    }

    public void StageUp()
    {
        MakeMonster(7.0f);
        Stagenum += 1;
    }

    public void StageDown()
    {
        MakeMonster(7.0f);

        if (Stagenum != 1)
            Stagenum -= 1;
        else
            Stagenum = 1;
    }
}
