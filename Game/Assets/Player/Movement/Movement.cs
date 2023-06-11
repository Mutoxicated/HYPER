using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementState
{
    SLIDING,
    SLAMMING,
    DASHING,
    WALKING
}

public class Movement : MonoBehaviour
{
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
    private ButtonInput dashInput = new ButtonInput("Dash");
    private ButtonInput slideInput = new ButtonInput("Slide");

    private Vector3 slideDirection = Vector3.zero;
    private ContactPoint point;
    private bool airborne = true;
    private bool crouchReleased = true;
    private bool uponSlide;

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
            if (uponSlide && moveDirection != Vector3.zero)
            {
                slideDirection = moveDirection.normalized;
                uponSlide = false;
            }
            else if (uponSlide && moveDirection == Vector3.zero)
            {
                slideDirection = transform.forward;
                uponSlide = false;
            }
            MoveHelper.Slide(rb, slideDirection, slideSpeed);
        }
        else if (movementState == MovementState.SLAMMING)
        {
            MoveHelper.GroundSlam(rb, slamSpeed);
        }else if (movementState == MovementState.DASHING)
        {
            if (moveDirection != Vector3.zero)
            {
                MoveHelper.Dash(rb, moveDirection.normalized, dashSpeed);
            }
            else
            {
                MoveHelper.Dash(rb, transform.forward, dashSpeed);
            }
            movementState = MovementState.WALKING;
        }
        rb.AddForce(Vector3.up * -gravityForce, ForceMode.Acceleration);
    }

    //input system has problems? put everything in the same update method
    private void FixedUpdate() 
    {
        jumpInput.Update();
        dashInput.Update();
        slideInput.Update();
        Move();
        if (jumpInput.GetInputDown() && !airborne)
        {
            MoveHelper.Jump(rb, point.normal, jumpForce);
            movementState = MovementState.WALKING;
            crouchReleased = false;
            return;
        }
        //Debug.Log(movementState);
        if (dashInput.GetInputDown())
        {
            movementState = MovementState.DASHING;
            crouchReleased = false;
            return;
        }

        if (slideInput.GetInputDown())
        {
            uponSlide = true;
        }
        if (slideInput.GetInput())
        {
            if (!airborne && crouchReleased)
            {
                movementState = MovementState.SLIDING;
            }
            if (airborne && crouchReleased)
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

    private void OnCollisionStay(Collision collision)
    {
        point = collision.contacts[0];
        airborne = false;
        rb.drag = 5f;
        if (movementState == MovementState.SLAMMING)
        {
            movementState = MovementState.WALKING;
        }
    }

    private void OnCollisionExit()
    {
        rb.drag = 2f;
        airborne = true;
    }
}
