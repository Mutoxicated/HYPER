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
        transform.position = VerifyPos(transform.position);
    }

    public Vector3 VerifyPos(Vector3 pos){
        Physics.RaycastNonAlloc(pos,Vector3.down,hits,Mathf.Infinity,layermask);

        float yDistance = Mathf.Abs(pos.y - hits[0].point.y);

        //Debug.Log("Hit point y: "+hits[0].point.y);
        if (yDistance < distanceCheck)
        {
            return new Vector3(pos.x, pos.y + Random.Range(minY, maxY),pos.z);
        }
        return pos;
    }
}
