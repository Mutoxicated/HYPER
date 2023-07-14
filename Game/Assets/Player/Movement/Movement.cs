using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public enum MovementState
{
    SLIDING,
    SLAMMING,
    WALKING,
    LOCKED,
    BOUNCING
}

public class Movement : MonoBehaviour
{
    [Header("Speeds")]
    [SerializeField, Range(40f,85f)] private float walkSpeed;
    [SerializeField, Range(12f,24f)] private float slideSpeed = 19f;
    [SerializeField, Range(22f, 44f)] private float slamSpeed = 16f;
    [SerializeField, Range(18f, 30f)] private float dashSpeed = 25f;

    [Header("Forces")]
    [SerializeField,Range(15f,30f)] private float gravityForce;
    [SerializeField, Range(15f, 40f)] private float jumpForce;
    [SerializeField, Range(0f, 1f)] private float airMultiplier;
    [SerializeField] private float launchForce;

    [Header("Limits")]
    [SerializeField] private int maxJumps;
    [SerializeField] private int maxBounces;

    [Header("Misc")]
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject zipMagnetPrefab;
    [SerializeField] private GameObject throwPoint;
    [SerializeField] private Transform camHolder;
    [SerializeField] private StaminaControl stamina;
    [SerializeField] private Rigidbody rb;

    [HideInInspector] public MovementState movementState = MovementState.WALKING;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 slideDirection = Vector3.zero;
    private ContactPoint point;
    private OnInterval launchInterval;

    private ButtonInput jumpInput = new ButtonInput("Jump");
    private ButtonInput dashInput = new ButtonInput("Dash");
    private ButtonInput slideInput = new ButtonInput("Slide");
    private ButtonInput launchInput = new ButtonInput("Launch");

    private RaycastHit hit;
    private Vector3 launchPoint;
    private bool airborne = true;
    private bool crouchReleased = true;
    private bool uponSlide;
    private int currentJumps = 0;
    private int bounces = 0;

    private MoveAbilities ability;
    private float moveX, moveZ;

    private Vector3 GetCameraRayPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Camera.main.scaledPixelWidth / 2, Camera.main.scaledPixelHeight / 2, 0));
        Physics.Raycast(ray, out hit, Mathf.Infinity);
        return hit.point;
    }

    private void Move()
    {
        if (movementState == MovementState.BOUNCING)
        {
            if (bounces == 0)
            {
                rb.AddForce(ability.GetBounceDir() * launchForce * 1.1f, ForceMode.Acceleration); ;
            }
            else
            {
                rb.velocity = ability.GetBounceDir() * launchForce * 0.38f;
            }
            return;
        }
        if (movementState != MovementState.LOCKED && movementState != MovementState.BOUNCING)
        {
            rb.AddForce(Vector3.up * -gravityForce, ForceMode.Acceleration);
        }
        else 
        {
            bool durationExceeded = ability.Lock(1f);
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
            moveX = Input.GetAxisRaw("Horizontal") * walkSpeed;
            moveZ = Input.GetAxisRaw("Vertical") * walkSpeed;

            Vector3 flatForward = camHolder.forward;
            flatForward.y = 0f;

            moveDirection = flatForward.normalized * moveZ + camHolder.right * moveX;
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
                slideDirection = camHolder.forward;
                uponSlide = false;
            }
            ability.Slide(slideDirection, slideSpeed);
            return;
        }
        //slamming
        if (movementState == MovementState.SLAMMING)
        {
            ability.GroundSlam(slamSpeed);
        }
    }

    private void LaunchLogic()
    {
        if (!launchInterval.enabled)
            return;
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            ability.LaunchIn(launchPoint, launchForce);
            Destroy(launchInterval.gameObject);
            movementState = MovementState.BOUNCING;
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            ability.LaunchOut(launchPoint, launchForce);
            Destroy(launchInterval.gameObject);
            movementState = MovementState.BOUNCING;
        }
    }

    private void Start()
    {
        rb.drag = 2f;
        ability = new MoveAbilities(rb);
        movementState = MovementState.WALKING;
    }

    private void Update()
    {
        //Debug.Log(airborne);
        jumpInput.Update();
        dashInput.Update();
        slideInput.Update();
        launchInput.Update();
        if (!point.IsUnityNull())//this check covers specifically the enemies when they die while you are in contact with them
        {
            if (point.otherCollider == null)
                airborne = true;
        }
        if (launchInput.GetInputDown() && animator.GetCurrentAnimatorStateInfo(0).IsName("Nun") && launchInterval == null && movementState != MovementState.BOUNCING)
        {
            animator.Play("Throw");
            launchPoint = GetCameraRayPoint();
            var instance = Instantiate(zipMagnetPrefab, throwPoint.transform.position, Quaternion.LookRotation(launchPoint-throwPoint.transform.position));
            launchInterval = instance.GetComponent<OnInterval>();
        }
        if (launchInterval != null)
        {
            LaunchLogic();
        }
        if (jumpInput.GetInputDown() && currentJumps < maxJumps)
        {
            currentJumps++;
            if (!airborne)
            {
                ability.Jump(point, jumpForce);
                movementState = MovementState.WALKING;
                crouchReleased = false;
            }
        else
        {
            stamina.ReduceStamina(50f);
            movementState = MovementState.LOCKED;
        }
        return;
        }
        if (dashInput.GetInputDown() && stamina.GetCurrentStamina() > 100f)
        {
            if (moveDirection != Vector3.zero)
            {
                ability.Dash(moveDirection.normalized, dashSpeed);
            }
            else
            {
                ability.Dash(camHolder.forward, dashSpeed);
            }
            stamina.ReduceStamina(100f);
            movementState = MovementState.WALKING;
            rb.drag = 2f;
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
        //Debug.Log(movementState.ToString());
        if (movementState == MovementState.SLAMMING)
            return;
        if (movementState == MovementState.LOCKED)
            return;
        if (movementState == MovementState.BOUNCING)
            return;
        movementState = MovementState.WALKING;
    }

    // used to be this comment said something really stupid
    private void FixedUpdate() => Move();
    
    private void OnCollisionEnter(Collision collision)
    {
        point = collision.GetContact(0);
        airborne = false;
        if (movementState != MovementState.BOUNCING)
            return;
        bounces++;
        ability.Bounce(point);
        if (bounces >= maxBounces)
        {
            rb.velocity = ability.GetBounceDir() * launchForce * 0.38f;
            rb.drag = 2f;
            bounces = 0;
            movementState = MovementState.WALKING;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        point = collision.GetContact(0);
        airborne = false;
        if (Mathf.Approximately(point.normal.y, Vector3.up.y))
        {
            currentJumps = 0;
        }
        if (movementState == MovementState.SLAMMING || movementState == MovementState.LOCKED)
        {
            movementState = MovementState.WALKING;
        }
        if (movementState == MovementState.BOUNCING)
            return;
        rb.drag = 5f;
    }

    private void OnCollisionExit()
    {
        airborne = true;
        if (movementState == MovementState.BOUNCING)
            return;
        rb.drag = 2f;
    }
}
