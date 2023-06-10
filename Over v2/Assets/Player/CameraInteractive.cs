using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteractive : MonoBehaviour
{
    [SerializeField] private Movement movement;

    void Update()
    {
        if (movement.movementState == Movement.MovementState.SLIDING && transform.localPosition.y == 0f)
        {
            transform.localPosition = new Vector3(0f, -0.25f, 0f);
        }
        else if (movement.movementState == Movement.MovementState.WALKING && transform.localPosition.y != 0f)
        {
            transform.localPosition = Vector3.zero;
        }
    }
}
