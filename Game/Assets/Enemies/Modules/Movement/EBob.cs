using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBob : ExtraBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private FloorDetector fd;
    [SerializeField] private BobInformation[] bobs;

    private int currentIndex;
    private bool bobFInished = true;

    public void ChangeIndex(int index){
        currentIndex = index;
    }

    public void Bob(bool state){
        if (!fd.isGrounded)
            return;
        if (state){
            rb.AddForce(bobs[currentIndex].direction.normalized*Random.Range(bobs[currentIndex].minmaxSpeed.x,bobs[currentIndex].minmaxSpeed.y),ForceMode.Impulse);
        }
    } 
}
