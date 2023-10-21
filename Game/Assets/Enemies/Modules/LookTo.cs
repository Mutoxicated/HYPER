using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum DeathFor {
    PLAYER,
    ENEMIES
}

public class LookTo : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private float lerpSpeed;
    private Quaternion lookRotation;
    private Vector3 toEntity = Vector3.zero;

    private void OnEnable(){
        stats.FindEntity();
        //Debug.Log("enabled on: "+gameObject.name);
        GetRotation();
        transform.rotation = lookRotation;
    }

    private void GetRotation(){
        if (stats.entity == null){
            stats.DecideObjective();
            stats.FindEntity();
        }
        toEntity = stats.entity.position - transform.position;
        lookRotation = Quaternion.LookRotation(toEntity,Vector3.up);
    }

    public void ResetLocalRotation(){
        transform.localRotation = Quaternion.identity;
        lookRotation = Quaternion.identity;
    }

    public void ChangeLerpSpeed(float speed){
        lerpSpeed = speed;
    }

    private void Update()
    {
        GetRotation();
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * lerpSpeed);
    }
}
