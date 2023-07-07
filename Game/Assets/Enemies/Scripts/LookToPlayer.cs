using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookToPlayer : MonoBehaviour
{
    [SerializeField] private Vector3 axisMultiplier = new Vector3(1f, 1f, 1f);
    [SerializeField] private float lerpSpeed;
    private Transform player;
    private Quaternion lookRotation;
    private Vector3 toPlayer = Vector3.zero;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {
        toPlayer = player.position - transform.position;
        toPlayer.x *= axisMultiplier.x;
        toPlayer.y *= axisMultiplier.y;
        toPlayer.z *= axisMultiplier.z;
        lookRotation = Quaternion.LookRotation(toPlayer,Vector3.up);
        //Debug.Log(toPlayer);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * lerpSpeed);
    }
}
