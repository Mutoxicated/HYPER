using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EWalk : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform transformToRotate;
    [SerializeField] private FloorDetector fd;

    [SerializeField] private OnInterval walkInterval;
    [SerializeField] private OnInterval restInterval;

    [SerializeField] private float walkLerp;
    [SerializeField] private Vector2 walkLerpminmaxcale;

    [SerializeField] private float restIntervalTime;
    [SerializeField] private Vector2 restIntervalminmaxScale;

    private Vector3 moveDirection;
    private Quaternion rotation;
    private float lerp;
    private bool lookToWalk = true;

    private void ChangeDirection(){
        float ry = Random.Range(0f,360f);
        rotation = Quaternion.Euler(0f,ry,0f);
    }

    private void ValidateDirection(){
        if (!fd.isGrounded){
            rotation *= Quaternion.Euler(0f,180f,0f);
        }
    }

    public void Walk(){
        walkInterval.enabled = true;
        ValidateDirection();
    }

    public void Rest(){
        restInterval.enabled = true;
        ChangeDirection();
        ChangeLerp();
    }

    public void LookToWalk(bool state){
        lookToWalk = !state;
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
        if (lookToWalk)
            transformToRotate.rotation = Quaternion.Lerp(transformToRotate.rotation,rotation,Time.fixedDeltaTime*15f);
        moveDirection = transformToRotate.forward;
        rb.position = Vector3.Lerp(rb.position,rb.position+moveDirection*2f,Time.fixedDeltaTime*lerp);
    }

}