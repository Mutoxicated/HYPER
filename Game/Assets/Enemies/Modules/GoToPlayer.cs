using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField,Range(3f,20f)] private float speed;
    [SerializeField,Range(0.2f,2f)] private float lerpSpeed;
    [SerializeField] private Vector3 axisMultiplier = new Vector3(1f,1f,1f);
    Vector3 toPlayer = Vector3.zero;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        toPlayer = player.position - transform.position;
        toPlayer.Normalize();
        toPlayer.x *= axisMultiplier.x;
        toPlayer.y *= axisMultiplier.y;
        toPlayer.z *= axisMultiplier.z;
        rb.velocity = Vector3.Lerp(rb.velocity, toPlayer * speed, Time.deltaTime * lerpSpeed);
    }
}
