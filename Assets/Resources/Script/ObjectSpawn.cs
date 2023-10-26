using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectSpawn : Singleton<ObjectSpawn>
{
    [Header("STAGE")]
    public int stageNum;
    public Text stage;
    [Space(15.0f)]
    public int monsterCount;
    public Transform monsterParent;
    [Header("Monster's name")]
    public List<GameObject> monsterName;
    [Header("MonsterList")]
    //public List<GameObject> makeMonsters;

    int prevStageNum = 1;

    void Start()
    {
        stage.text = "STAGE " + stageNum.ToString();
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
                Quaternion.identity, monsterParent);
            //makeMonsters.Add(obj);

            //int index = makeMonsters[i].name.IndexOf("(Clone)");
            //if (index > 0)
            //    makeMonsters[i].name = makeMonsters[i].name.Substring(0, index) + (i + 1).ToString();
        }
    }

    public void PullObject(GameObject obj)
    {
        obj.GetComponent<BoxCollider2D>().enabled = true;
        Destroy(obj);
        obj = null;
    }

    public void DestroyMonster()
    {
        for (int i = 0; i < monsterParent.childCount; ++i)
        {
            Object[] childObj = monsterParent.GetComponentsInChildren<Object>();
            Destroy(childObj[i]);
            childObj = null;
        }
    }

    void getStage()
    {
        if (stageNum != prevStageNum)
        {
            stage.text = "STAGE " + stageNum.ToString();
            prevStageNum = stageNum;
        }
    }

    public void StageUp()
    {
        DestroyMonster();
        MakeMonster(7.0f);
        stageNum += 1;
    }

    public void StageDown()
    {
        DestroyMonster();
        MakeMonster(7.0f);

        if (stageNum > 1)
            stageNum -= 1;
        else
            stageNum = 1;
    }
}
