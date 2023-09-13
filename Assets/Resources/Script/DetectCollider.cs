using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectCollider : MonoBehaviour
{
    [SerializeField] private Transform meleePos;
    [SerializeField] private Vector2 boxSize;
    [SerializeField] private Collider2D detectCollider;

    private void Update()
    {
        SearchCollider();
        EmptyCollider();
    }

    /*private void OnDrawGizmos() 
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(meleePos.position, boxSize);
    }*/

    public void SearchCollider()
    {
        detectCollider = Physics2D.OverlapBox(meleePos.position, boxSize, 0);
    }

    public DetectCollider ColliderInfo()
    {
        if (detectCollider != null)
            return detectCollider.gameObject.GetComponent<DetectCollider>();
        else
            return null;
    }

    public void EmptyCollider()
    {
        if (gameObject.activeInHierarchy == false)
            detectCollider = null;
    }
}
