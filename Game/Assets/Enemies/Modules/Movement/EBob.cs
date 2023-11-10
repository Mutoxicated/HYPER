using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBob : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BobInformation[] bobs;

    public void Bob(int index){
        rb.AddForce(bobs[index].direction.eulerAngles.normalized*bobs[index].speed,ForceMode.Impulse);
    } 
}
