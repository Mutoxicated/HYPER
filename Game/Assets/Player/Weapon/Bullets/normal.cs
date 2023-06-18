using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normal : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private GameObject particlePrefab;

    private void OnEnable()
    {
        rb.velocity = transform.forward * speed;
    }

    private void OnCollisionStay()
    {
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(particlePrefab, transform.position, transform.rotation);
    }
}
