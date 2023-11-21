using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBob : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BobInformation[] bobs;

    private int currentIndex;

    public void ChangeIndex(int index){
        currentIndex = index;
    }

    public void Bob(bool state){
        if (state)
            rb.AddForce(bobs[currentIndex].direction.normalized*bobs[currentIndex].speed,ForceMode.Impulse);
    } 
}
