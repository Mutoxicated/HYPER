using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteractive : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private float lerpSpeed;
    private Vector3 slidePos = new Vector3(0f, -0.25f, 0f);

    private void Update()
    {
        if (movement.movementState == MovementState.SLIDING)
        {
            transform.localPosition = slidePos;
            return;
        }
        else if (movement.movementState == MovementState.WALKING)
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
