using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWalk : MonoBehaviour
{
    /*
    step 1: find valid direction,
    step 2: rest
    step 3: walk
    */

    [SerializeField] private Rigidbody rb;
    [SerializeField] private FloorDetector fd;

    [SerializeField] private OnInterval walkInterval;
    [SerializeField] private OnInterval restInterval;

    [SerializeField] private float walkLerp;
    [SerializeField] private Vector3 walkLerpminmaxcale;

    [SerializeField] private float restIntervalTime;
    [SerializeField] private Vector2 restIntervalminmaxScale;

    private Vector3 moveDirection;
    private Quaternion rotation;
    private float lerp;

    private void ChangeDirection(){
        float ry = Random.Range(0f,360f);
        rotation = Quaternion.Euler(0f,ry,0f);
    }

    private void ValidateDirection(){
        if (!fd.isGrounded){
            rotation *= Quaternion.Euler(0f,180f,0f);
        }
    }

    private void Walk(){
        walkInterval.enabled = true;
        ValidateDirection();
    }

    private void Rest(){
        restInterval.enabled = true;
        ChangeDirection();
        ChangeLerp();
    }

    private void ChangeLerp(){
        lerp = walkLerp+Random.Range(walkLerpminmaxcale.x,walkLerpminmaxcale.y);
    }

    private void Start(){
        restInterval.ChangeInterval(restIntervalTime+Random.Range(restIntervalminmaxScale.x,restIntervalminmaxScale.y));
        Rest();
    }

    private void Update(){
        if (walkInterval.enabled != true)
            return;
        if (!fd.isGrounded){
            Rest();
        }
    }

    private void FixedUpdate(){
        if (walkInterval.enabled != true)
            return;
        rb.rotation = Quaternion.Lerp(rb.rotation,rotation,Time.fixedDeltaTime*7f);
        moveDirection = transform.forward;
        rb.position = Vector3.Lerp(rb.position,rb.position+moveDirection*2f,Time.fixedDeltaTime*lerp);
    }

}