using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [HideInInspector]
    public enum MovementState
    {
        SLIDING,
        SLAMMING,
        DASHING,
        WALKING
    }

    [SerializeField] private Rigidbody rb;
    [Header("Speeds")]
    [SerializeField, Range(40f,85f)] private float walkSpeed;
    [SerializeField] private float slideSpeed;
    [SerializeField] private float slamSpeed;
    [SerializeField] private float dashSpeed;

    [Header("Forces")]
    [SerializeField,Range(15f,30f)] private float gravityForce;
    [SerializeField, Range(15f, 40f)] private float jumpForce;
    [SerializeField, Range(0f, 1f)] private float airMultiplier;

    [HideInInspector] public MovementState movementState = MovementState.WALKING;
    private Vector3 moveDirection = Vector3.zero;
    private ButtonInput jumpInput = new ButtonInput("Jump");

    private ContactPoint point;
    private bool airborne = true;
    private bool crouchReleased = true;

    private void Move()
    {
        if (movementState == MovementState.WALKING)
        {
            float moveX = Input.GetAxis("Horizontal") * walkSpeed;
            float moveZ = Input.GetAxis("Vertical") * walkSpeed;

            Vector3 flatForward = transform.forward;
            flatForward.y = 0f;

            moveDirection = flatForward.normalized * moveZ + transform.right * moveX;
            if (!airborne)
                rb.AddForce(moveDirection, ForceMode.Force);
            else
                rb.AddForce(moveDirection * airMultiplier, ForceMode.Force);
        }
        else if (movementState == MovementState.SLIDING)
        {
            MoveHelper.Slide(rb, moveDirection.normalized, slideSpeed);
        }
        else if (movementState == MovementState.SLAMMING)
        {
            MoveHelper.GroundSlam(rb, slamSpeed);
        }
        rb.AddForce(Vector3.up * -gravityForce, ForceMode.Acceleration);
    }

    private void Update()
    {
        jumpInput.Update();
        if (jumpInput.GetInputDown() && !airborne)
        {
            MoveHelper.Jump(rb, point.normal, jumpForce);
        }
        //Debug.Log(movementState);
        if (Input.GetButton("Slide"))
        {
            if (!airborne && crouchReleased)
            {
                movementState = MovementState.SLIDING;
            }
            if (airborne)
            {
                movementState = MovementState.SLAMMING;
                crouchReleased = false;
            }
            return;
        }
        else
        {
            crouchReleased = true;
        }
        if (movementState != MovementState.SLAMMING)
        {
            movementState = MovementState.WALKING;
        }
    }

    private void FixedUpdate() => Move();

    private void OnCollisionStay(Collision collision)
    {
        point = collision.GetContact(0);
        airborne = false;
        rb.drag = 5f;
        if (movementState == MovementState.SLAMMING)
        {
            movementState = MovementState.WALKING;
        }
    }

    private void OnCollisionEnter()
    {
       
    }

    private void OnCollisionExit()
    {
        rb.drag = 2f;
        airborne = true;
    }
}
