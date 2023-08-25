using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class TNT : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject explosionPrefab;

    public void Explode()
    {
        PublicPools.pools[explosionPrefab.name].UseObject(transform.position,Quaternion.identity);
    }

    void Start()
    {
        rb.velocity = transform.forward * speed;
    }
}
