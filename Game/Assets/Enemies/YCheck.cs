using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class YCheck : MonoBehaviour
{
    [SerializeField] private float yCheck;
    [SerializeField] private float distanceCheck;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    void Start()
    {
        float yDistance = Mathf.Abs(transform.position.y - yCheck);
        if (yDistance < distanceCheck)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Random.Range(minY, maxY),transform.position.z);
        }
    }
}
