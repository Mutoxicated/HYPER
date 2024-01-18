using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowDown : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float slowDownSpeed;
    [SerializeField] private OnInterval onInterval;
    [SerializeField] private float maxSpeed;

    private float currentSpeed;

    private void Update()
    {
        currentSpeed = Mathf.Lerp(maxSpeed * stats.GetNum("moveSpeed"), slowDownSpeed * stats.GetNum("moveSpeed"), onInterval.t);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, currentSpeed * stats.GetNum("moveSpeed"));
    }
}
