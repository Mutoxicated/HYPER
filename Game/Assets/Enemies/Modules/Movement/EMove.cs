using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMove : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private bool clampVelocity;
    [SerializeField, Range(0.1f, 10f)] private float clampSpeed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    private Vector3 direction = Vector3.one;

    public void ResetVelocity()
    {
        rb.velocity = rb.rotation.eulerAngles.normalized * speed * stats.GetNum("moveSpeed");
    }

    void Start()
    {
        rb.velocity = (Random.rotation * direction).normalized*speed * stats.GetNum("moveSpeed");
    }

    void Update()
    {
        if (!clampVelocity)
            return;
        if (rb.velocity.magnitude > speed * stats.GetNum("moveSpeed"))
        {
            var clampedVel = Vector3.ClampMagnitude(rb.velocity, speed * stats.GetNum("moveSpeed"));
            rb.velocity = Vector3.Lerp(rb.velocity, clampedVel, Time.deltaTime);
        }
    }

    public void LookToTarget(){
        stats.FindEntity();
        if (stats.entity != null)
        {
            direction = stats.entity.position - transform.position;
            direction.Normalize();

        }else{
            direction = -direction;
        }
        rb.velocity = direction.normalized*speed * stats.GetNum("moveSpeed");
    }
}
