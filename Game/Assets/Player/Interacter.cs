using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interacter : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask layers;

    private GameObject goHit;

    private RaycastHit hit;

    public GameObject GetGoHit(){
        return goHit;
    }

    private void Update(){
        bool didHit = Physics.Raycast(transform.position,transform.TransformDirection(Vector3.forward),out hit,rayDistance,layers,QueryTriggerInteraction.Collide);
        if (didHit){
            goHit = hit.transform.gameObject;
        }else{
            goHit = null;
        }
        //Debug.Log(goHit);
    }
}
