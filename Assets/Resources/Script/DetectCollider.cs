using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollider : MonoBehaviour
{
    [SerializeField] private Transform meleePos;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Collider2D detectCollider;
    public bool isDetect = false;

    void Update()
    {
        if (!isDetect)  // TODO : 찾은 콜라이더가 지워졌다가 생겼다하는 문제를 수정해야함
        {
            StartCoroutine(SearchCollider()); 

            if (detectCollider != null)
                isDetect = true;
        }
        else
            StartCoroutine(EmptyCollider());
    }

    /*private void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(meleePos.position, boxSize);
    }*/

    IEnumerator SearchCollider()
    {
        yield return null;
        detectCollider = Physics2D.OverlapBox(meleePos.position, boxSize, 0);
    }

    public DetectCollider EnemyInfo()
    {
        if (detectCollider != null)
            return detectCollider.GetComponent<DetectCollider>();
        else
            return null;
    }

    IEnumerator EmptyCollider()
    {
        yield return null;
        if (detectCollider)
        detectCollider = null;
        isDetect = false;
    }
}
