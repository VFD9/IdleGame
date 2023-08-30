using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Object Object;
    [SerializeField] private float smoothig = 0.2f;

    void Start()
    {
        Object = GameObject.FindGameObjectWithTag("Player").GetComponent<Object>();
    }

    void FixedUpdate()
    {
        Vector3 targetPos = new Vector3(Object.transform.position.x, transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smoothig);
    }
}
