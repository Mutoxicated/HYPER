using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInteractive : MonoBehaviour
{
    [SerializeField] private Movement movement;
    [SerializeField] private Vector3 slidePos = new Vector3(0f, -0.25f, 0f);
    private Vector3 initialPos;
    private bool once = true;

    private void Start()
    {
        initialPos = transform.localPosition;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;
        if (movement.movementState == MovementState.SLIDING && once)
        {
            once = false;
            //Debug.Log("la");
            transform.localPosition = slidePos;
        }
        else if (movement.movementState == MovementState.WALKING && !once)
        {
            once = true;
            transform.localPosition = initialPos;
        }
    }
}
