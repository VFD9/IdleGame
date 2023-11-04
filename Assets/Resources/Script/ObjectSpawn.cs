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
    public GameObject boss;

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
        if (stageNum % 4 == 0)
        {
            GameObject obj = Instantiate(boss, new Vector3(12.0f, boss.transform.position.y, 0.0f),
                Quaternion.identity, monsterParent);
            obj.name = "Boss";
        }
        else
        {
            for (int i = 0; i < monsterCount; ++i)
            {
                GameObject obj = Instantiate(monsterName[Random.Range(0, 4)],
                    new Vector3(_x + (4.0f * i), -1.15f, 0.0f),
                    Quaternion.identity, monsterParent);
            }
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
            GameObject obj = monsterParent.GetChild(i).gameObject;
            Destroy(obj);
            obj = null;
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
        stageNum += 1;
        MakeMonster(7.0f);
    }

    public void StageDown()
    {
        DestroyMonster();

        if (stageNum > 1)
            stageNum -= 1;
        else
            stageNum = 1;

        MakeMonster(7.0f);
    }
}
