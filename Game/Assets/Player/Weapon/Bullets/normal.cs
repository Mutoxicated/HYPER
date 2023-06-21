using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class normal : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private int lifetime;
    private int time;

    private void OnEnable()
    {
        rb.velocity = transform.forward * speed;
    }

    private void Update()
    {
        time++;
        if (time >= lifetime)
        {
            gameObject.SetActive(false);
            time = 0;
        }
    }

    private void Recycle()
    {
        gameObject.SetActive(false);
        Instantiate(particlePrefab, transform.position, transform.rotation);
        time = 0;
    }

    private void OnCollisionEnter()
    {
        Recycle();
    }

    private void OnCollisionStay(Collision collision)
    {
        Recycle();
    }
}
