using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoTo : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private Rigidbody rb;
    [SerializeField,Range(3f,40f)] private float speed;
    [SerializeField,Range(0.2f,6f)] private float lerpSpeed;
    [SerializeField] private OnInterval interval;
    [SerializeField] private bool useInterval;
    [SerializeField] private Vector3 axisMultiplier = new Vector3(1f,1f,1f);
    Vector3 toEntity = Vector3.zero;
    private Transform entity;

    private void Start()
    {
        if (Difficulty.enemies.Count == 1){
            stats.objective = DeathFor.PLAYER;
            entity = Difficulty.player;
        }
        if (stats.objective == DeathFor.PLAYER){
            entity = Difficulty.player;
        }else{
            entity = Difficulty.FindClosestEnemy(transform).transform;
        }
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
        if (entity == null){
            if (Difficulty.enemies.Count == 0){
                stats.objective = DeathFor.PLAYER;
                entity = Difficulty.player;
            }
            if (stats.objective == DeathFor.PLAYER){
                stats.objective = DeathFor.ENEMIES;
                entity = Difficulty.FindClosestEnemy(transform).transform;
            }else{
                entity = Difficulty.FindClosestEnemy(transform).transform;
            }
        }
        toEntity = entity.position - transform.position;
        toEntity.Normalize();
        toEntity.x *= axisMultiplier.x;
        toEntity.y *= axisMultiplier.y;
        toEntity.z *= axisMultiplier.z;
        if (!useInterval)
            rb.velocity = Vector3.Lerp(rb.velocity, toEntity * speed * stats.numericals["moveSpeed"], Time.deltaTime * lerpSpeed);
        else
            rb.velocity = Vector3.Lerp(rb.velocity, toEntity * speed * stats.numericals["moveSpeed"], interval.t* Time.deltaTime);
    }
}
