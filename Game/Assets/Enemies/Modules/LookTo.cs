using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DeathFor {
    PLAYER,
    ENEMIES
}

public class LookTo : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private Vector3 axisMultiplier = new Vector3(1f, 1f, 1f);
    [SerializeField] private float lerpSpeed;
    private Quaternion lookRotation;
    private Vector3 toEntity = Vector3.zero;
    private Transform entity;

    private void Start(){
        if (Difficulty.enemies.Count == 1){
            stats.objective = DeathFor.PLAYER;
            entity = Difficulty.player;
            return;
        }
        if (stats.objective == DeathFor.PLAYER){
            entity = Difficulty.player;
        }else{
            entity = Difficulty.FindClosestEnemy(transform).transform;
        }
    }

    private void Update()
    {
        if (entity == null){
            if (Difficulty.enemies.Count == 1){
                stats.objective = DeathFor.PLAYER;
                entity = Difficulty.player;
                return;
            }
            if (stats.objective == DeathFor.PLAYER){
                stats.objective = DeathFor.ENEMIES;
                entity = Difficulty.FindClosestEnemy(transform).transform;
            }else{
                entity = Difficulty.FindClosestEnemy(transform).transform;
            }
        }
        toEntity = entity.position - transform.position;
        toEntity.x *= axisMultiplier.x;
        toEntity.y *= axisMultiplier.y;
        toEntity.z *= axisMultiplier.z;
        lookRotation = Quaternion.LookRotation(toEntity,Vector3.up);
        //Debug.Log(toEntity);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * lerpSpeed);
    }
}
