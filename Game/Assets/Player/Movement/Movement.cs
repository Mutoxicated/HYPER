using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum MovementState
{
    SLIDING,
    SLAMMING,
    WALKING,
    LOCKED
}

public class Movement : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField, Range(40f,85f)] private float walkSpeed;
    [SerializeField, Range(12f,24f)] private float slideSpeed = 19f;
    [SerializeField, Range(12f, 24f)] private float slamSpeed = 16f;
    [SerializeField, Range(18f, 30f)] private float dashSpeed = 25f;

    [Header("Forces")]
    [SerializeField,Range(15f,30f)] private float gravityForce;
    [SerializeField, Range(15f, 40f)] private float jumpForce;
    [SerializeField, Range(0f, 1f)] private float airMultiplier;

    [Header("Misc")]
    [SerializeField] private StaminaControl stamina;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int maxJumps;

    [HideInInspector] public MovementState movementState = MovementState.WALKING;
    [HideInInspector] public UnityEvent OnDash = new UnityEvent();
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 slideDirection = Vector3.zero;
    private ContactPoint point;

    private ButtonInput jumpInput = new ButtonInput("Jump");
    private ButtonInput dashInput = new ButtonInput("Dash");
    private ButtonInput slideInput = new ButtonInput("Slide");

    private bool airborne = true;
    private bool crouchReleased = true;
    private bool uponSlide;
    private int currentJumps = 0;

    private void Move()
    {
        if (movementState != MovementState.LOCKED)
        {
            rb.AddForce(Vector3.up * -gravityForce, ForceMode.Acceleration);
        }
        else 
        {
            bool durationExceeded = MoveAbilities.Lock(rb, 40);
            if (durationExceeded)
            {
                //Debug.Log("UNLOCKED");
                movementState = MovementState.WALKING;
            }
            return;
        }
        //walking
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

            return;
        }
        //sliding
        if (movementState == MovementState.SLIDING)
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
            MoveAbilities.Slide(rb, slideDirection, slideSpeed);

            return;
        }
        //slamming
        if (movementState == MovementState.SLAMMING)
        {
            MoveAbilities.GroundSlam(rb, slamSpeed);
        }
    }

    private void Start()
    {
        movementState = MovementState.WALKING;
    }

    //input system has problems? put everything in the same update method
    private void FixedUpdate() 
    {
        jumpInput.Update();
        dashInput.Update();
        slideInput.Update();
        Move();
        if (jumpInput.GetInputDown() && currentJumps <= maxJumps)
        {
            currentJumps++;
            if (!airborne)
            {
                MoveAbilities.Jump(rb, point.normal, jumpForce);
                movementState = MovementState.WALKING;
                crouchReleased = false;
            }
            else
            {
                movementState = MovementState.LOCKED;
            }
            return;
        }

        if (dashInput.GetInputDown() && stamina.GetCurrentStamina() > 100f)
        {
            if (moveDirection != Vector3.zero)
            {
                MoveAbilities.Dash(rb, moveDirection.normalized, dashSpeed);
            }
            else
            {
                MoveAbilities.Dash(rb, transform.forward, dashSpeed);
            }
            stamina.ReduceStamina(100f);
            OnDash.Invoke();
            movementState = MovementState.WALKING;
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
        if (movementState != MovementState.SLAMMING && movementState != MovementState.LOCKED)
        {
            movementState = MovementState.WALKING;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        point = collision.contacts[0];
        airborne = false;
        rb.drag = 5f;
        if (movementState == MovementState.SLAMMING || movementState == MovementState.LOCKED)
        {
            movementState = MovementState.WALKING;
        }
        if (point.normal == Vector3.up)
        {
            currentJumps = 0;
        }
    }

    private void OnCollisionExit()
    {
        rb.drag = 2f;
        airborne = true;
    }
}
