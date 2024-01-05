using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTo : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private Rigidbody rb;
    [SerializeField,Range(3f,40f)] private float speed;
    [SerializeField] private float randomnessScale;
    [SerializeField,Range(0.2f,6f)] private float lerpSpeed;
    [SerializeField] private OnInterval interval;
    [SerializeField] private bool useInterval;
    [SerializeField] private Vector3 axisMultiplier = new Vector3(1f,1f,1f);
    Vector3 toEntity = Vector3.zero;
    
    private void OnEnable()
    {
        stats.FindEntity();
        speed += Random.Range(-randomnessScale,randomnessScale);
        GetDirection();
        Go();
    }

    public void ChangeSpeed(float speed)
    {
        this.speed = speed;
        speed += Random.Range(-randomnessScale,randomnessScale);
    }

    public void ChangeLerpSpeed(float lerpSpeed)
    {
        this.lerpSpeed = lerpSpeed;
    }

    public void ChangeAxisMultiplier(Vector3 axis)
    {
        axisMultiplier = axis;
    }

    public void ResetPosition(){
        transform.position = Vector3.zero;
    }

    public void ResetLocalPosition(){
        transform.localPosition = Vector3.zero;
    }

    public void ResetVelocity(){
        rb.velocity = Vector3.zero;
    }

    private void GetDirection(){
        if (stats.entity == null){
            stats.DecideObjective();
            stats.FindEntity();
        }
        if (stats.entity == null)
            return;
        toEntity = stats.entity.position - transform.position;
        toEntity.Normalize();
        toEntity.x *= axisMultiplier.x;
        toEntity.y *= axisMultiplier.y;
        toEntity.z *= axisMultiplier.z;
    }

    private void Go(){
        if (stats.entity == null)
            return;
        if (!useInterval)
            rb.velocity = Vector3.Lerp(rb.velocity, toEntity * speed * stats.numericals["moveSpeed"], Time.deltaTime * lerpSpeed);
        else
            rb.velocity = Vector3.Lerp(rb.velocity, toEntity * speed * stats.numericals["moveSpeed"], interval.t* Time.deltaTime);
    }

    private void Update()
    {
        GetDirection();
        Go();
    }
}
