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
