using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normal : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    private void OnEnable()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionStay()
    {
        gameObject.SetActive(false);
    }
}
