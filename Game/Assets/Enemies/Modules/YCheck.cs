using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class YCheck : MonoBehaviour
{
    [SerializeField] private LayerMask layermask;
    [SerializeField] private float distanceCheck;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    private RaycastHit[] hits = new RaycastHit[1];

    void Start()
    {
        Physics.RaycastNonAlloc(transform.position,Vector3.down,hits,Mathf.Infinity,layermask,QueryTriggerInteraction.UseGlobal);

        float yDistance = Mathf.Abs(transform.position.y - hits[0].point.y);
        if (yDistance < distanceCheck)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + Random.Range(minY, maxY),transform.position.z);
        }
    }
}
