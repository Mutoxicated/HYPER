using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EBob : MonoBehaviour
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
            if (bobs[currentIndex].toTarget && stats.entityForever != null){
                float yDiff = stats.entityForever.position.y-transform.position.y;
                rb.AddForce(bobs[currentIndex].direction.normalized*Mathf.Clamp(yDiff*5f,0f,200f),ForceMode.Impulse);
            }else
                rb.AddForce(bobs[currentIndex].direction.normalized*Random.Range(bobs[currentIndex].minmaxSpeed.x,bobs[currentIndex].minmaxSpeed.y),ForceMode.Impulse);
        }
    } 
}
