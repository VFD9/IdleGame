using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollider : MonoBehaviour
{
    [SerializeField] private Transform meleePos;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Collider2D detectCollider;

    void Update()
    {
        SearchCollider();
    }

    /*private void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(meleePos.position, boxSize);
    }*/

    void SearchCollider()
    {
        // OverlapBox 메서드를 이용해 boxSize안에 있는 콜라이더를 확인한다.
        detectCollider = Physics2D.OverlapBox(meleePos.position, boxSize, 0);
    }

    public DetectCollider EnemyInfo()
    {
        if (detectCollider != null)
            return detectCollider.GetComponent<DetectCollider>();
        return null;
    }

    public void EmptyCollider2D()
    {
        if (detectCollider != null)
            detectCollider = null;
    }
}
