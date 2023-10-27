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
        // OverlapBox �޼��带 �̿��� boxSize�ȿ� �ִ� �ݶ��̴��� Ȯ���Ѵ�.
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
