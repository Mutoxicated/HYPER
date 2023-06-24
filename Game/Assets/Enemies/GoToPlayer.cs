using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToPlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField,Range(3f,20f)] private float speed;
    [SerializeField,Range(0.2f,2f)] private float lerpSpeed;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        Vector3 toPlayer = player.position-transform.position;
        rb.velocity = Vector3.Lerp(rb.velocity, toPlayer.normalized * speed, Time.deltaTime * lerpSpeed);
    }
}
