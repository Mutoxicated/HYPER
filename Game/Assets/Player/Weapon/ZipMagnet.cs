using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZipMagnet : MonoBehaviour
{
    [SerializeField] private OnInterval interval;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    private void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.velocity = Vector3.zero;
        transform.rotation = Quaternion.LookRotation(GameObject.FindWithTag("Player").transform.position-transform.position);
        interval.enabled = true;
    }
}
