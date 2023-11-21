using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorDetector : MonoBehaviour
{
    public bool isGrounded;

    //it is assumed that whatever gameobject contains this script also have a trigger collider on it
    private void OnTriggerEnter(Collider coll){
        if (coll.gameObject.layer == 10)
            isGrounded = true;
    }

    private void OnTriggerStay(Collider coll){
        if (coll.gameObject.layer == 10)
            isGrounded = true;
    }

    private void OnTriggerExit(Collider coll){
        if (coll.gameObject.layer == 10)
            isGrounded = false;
    }
}
