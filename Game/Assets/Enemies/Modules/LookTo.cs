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

    private void Start(){
        stats.FindEntity();
    }

    public void ResetLocalRotation(){
        transform.localRotation = Quaternion.identity;
    }

    public void ChangeLerpSpeed(float speed){
        lerpSpeed = speed;
    }

    private void Update()
    {
        if (stats.entity == null){
            stats.DecideObjective();
            stats.FindEntity();
        }
        toEntity = stats.entity.position - transform.position;
        toEntity.x *= axisMultiplier.x;
        toEntity.y *= axisMultiplier.y;
        toEntity.z *= axisMultiplier.z;
        lookRotation = Quaternion.LookRotation(toEntity,Vector3.up);
        //Debug.Log(toEntity);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * lerpSpeed);
    }
}
