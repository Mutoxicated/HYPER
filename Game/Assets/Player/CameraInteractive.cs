using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteractive : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private float lerpSpeed;
    private Vector3 defaultPos = Vector3.zero;
    private Vector3 Pos = Vector3.zero;
    private Vector3 dashPos = new Vector3(0f, 0f, 1f);
    private Vector3 slidePos = new Vector3(0f, -0.25f, 0f);

    void Update()
    {
        if (movement.movementState == MovementState.SLIDING && transform.localPosition.y == 0f)
        {
            transform.localPosition = slidePos;
            return;
        }
        else if (movement.movementState == MovementState.WALKING && transform.localPosition.y != 0f)
        {
            transform.localPosition = Vector3.zero;
            Pos = defaultPos;
        }
        else if (movement.movementState == MovementState.DASHING)
        {
            Pos = dashPos;
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, Pos, Time.deltaTime* lerpSpeed);
    }
}
