using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPlayer : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private Rigidbody rb;
    [SerializeField,Range(3f,40f)] private float speed;
    [SerializeField,Range(0.2f,6f)] private float lerpSpeed;
    [SerializeField] private OnInterval interval;
    [SerializeField] private bool useInterval;
    [SerializeField] private Vector3 axisMultiplier = new Vector3(1f,1f,1f);
    Vector3 toPlayer = Vector3.zero;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb.velocity = transform.forward * speed;
    }

    public void ChangeSpeed(float speed)
    {
        this.speed = speed;
    }

    public void ChangeLerpSpeed(float lerpSpeed)
    {
        this.lerpSpeed = lerpSpeed;
    }

    public void ChangeAxisMultiplier(Vector3 axis)
    {
        axisMultiplier = axis;
    }

    private void Update()
    {
        toPlayer = player.position - transform.position;
        toPlayer.Normalize();
        toPlayer.x *= axisMultiplier.x;
        toPlayer.y *= axisMultiplier.y;
        toPlayer.z *= axisMultiplier.z;
        if (!useInterval)
            rb.velocity = Vector3.Lerp(rb.velocity, toPlayer * speed * stats.incrementalStat["moveSpeed"], Time.deltaTime * lerpSpeed);
        else
            rb.velocity = Vector3.Lerp(rb.velocity, toPlayer * speed * stats.incrementalStat["moveSpeed"], interval.t* Time.deltaTime);
    }
}
