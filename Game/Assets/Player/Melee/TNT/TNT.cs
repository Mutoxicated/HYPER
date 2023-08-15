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
    [SerializeField] private float damageMultiplier;
    [SerializeField] private GameObject explosionPrefab;

    public void Explode()
    {
        var instance = Instantiate(explosionPrefab,transform.position,Quaternion.identity);
        instance.GetComponent<Stats>().ModifyIncrementalStat("damage", damageMultiplier, 0.8f);
    }

    void Start()
    {
        rb.velocity = transform.forward * speed;
    }
}
