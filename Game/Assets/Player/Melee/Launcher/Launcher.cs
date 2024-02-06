using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{
    public static Movement playerMovement;
    [SerializeField] private OnInterval interval;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Collider coll;
    [SerializeField] private float speed;

    private Vector3 toEntity;
    private Transform entity;

    private void Awake(){
        FindMovement();
    }

    public static void FindMovement(){
        if (playerMovement == null){
            playerMovement = GameObject.FindWithTag("Player").GetComponent<Movement>();
        }
    }
    private void Start()
    {
        rb.velocity = transform.forward * speed;
    }

    public void SetEntity(Transform entity){
        this.entity = entity;
    }

    private void GetDirection(){
        if (entity == null){
            return;
        }

        toEntity = entity.position - transform.position;      
        toEntity.Normalize();
    }

    private void Go(){
        if (entity == null) return;
        rb.velocity = Vector3.Lerp(rb.velocity, toEntity * speed, Time.deltaTime*10f);
    }

    private void Update()
    {
        GetDirection();
        Go();
    }

    private void SettleMagnet(Collision collision){
        rb.velocity = Vector3.zero;
        transform.position = collision.contacts[0].point;
        transform.rotation = Quaternion.LookRotation(collision.contacts[0].normal,Vector3.up);
        playerMovement.TriggerBounceState(collision.contacts[0].point,gameObject);
        rb.constraints = RigidbodyConstraints.FreezeAll;
        coll.enabled = false;
        interval.enabled = true;
        transform.SetParent(collision.gameObject.transform);
    }

    private void OnCollisionEnter(Collision collision)
    {
        SettleMagnet(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        SettleMagnet(collision);
    }
}
