using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Internal;

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
    [SerializeField, Range(32f, 54f)] private float slamSpeed = 16f;
    [SerializeField, Range(18f, 30f)] private float dashSpeed = 25f;

    [Header("Forces")]
    [SerializeField,Range(15f,30f)] private float gravityForce;
    [SerializeField, Range(15f, 40f)] private float jumpForce;
    [SerializeField, Range(0f, 1f)] private float airMultiplier;
    [SerializeField] private float launchForce;
    private float extraJumpForce = 2f;

    [Header("Limits")]
    [SerializeField] private int maxJumps;
    [SerializeField] private int maxBounces;
    [SerializeField] private bool disableLaunch;

    [Header("Timers")]
    [SerializeField] public OnInterval launchInterval;
    [SerializeField] private OnInterval lockInterval;
    [SerializeField] private OnInterval slamJumpInterval;

    [Header("Particles")]
    [SerializeField] private ParticleSystem groundSlam;
    [SerializeField] private ParticleSystem dash;
    [SerializeField] private ParticleSystem slide;
    [SerializeField] private ParticleSystem lockEffect;

    [Header("Misc")]
    [SerializeField] private float slamJumpForceRate;
    [SerializeField] private Stats stats;
    [SerializeField] private Transform camHolder;
    [SerializeField] private StaminaControl stamina;
    [SerializeField] private Rigidbody rb;

    [HideInInspector] public MovementState movementState = MovementState.WALKING;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 slideDirection = Vector3.zero;
    private ContactPoint point;

    private ButtonInput jumpInput = new ButtonInput("Jump");
    private ButtonInput dashInput = new ButtonInput("Dash");
    private ButtonInput slideInput = new ButtonInput("Slide");
    private ButtonInput launchInInput = new ButtonInput("LaunchIn");

    [HideInInspector] public Vector3 launchPoint;
    [HideInInspector] public bool airborne = true;
    private bool crouchReleased = true;
    private bool uponSlide;
    private int currentJumps = 0;
    private int bounces = 0;

    private MoveAbilities ability;
    private float moveX, moveZ;
    private CameraShake camShake;

    public void ChangeState(int ms)
    {
        movementState = (MovementState)ms;
    }

    public void ChangeDrag(float n)
    {
        rb.drag = n;
    }

    private void Move()
    {
        //Debug.Log(movementState);
        if (movementState == MovementState.BOUNCING)
        {
            rb.velocity = ability.GetBounceDir() * launchForce * stats.numericals["moveSpeed"];
            return;
        }
        if (movementState != MovementState.LOCKED && movementState != MovementState.BOUNCING)
        {
            rb.AddForce(Vector3.up * -gravityForce, ForceMode.Acceleration);
        }
        else 
        {
            ability.Lock(lockInterval.t);
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
                rb.AddForce(moveDirection * stats.numericals["moveSpeed"], ForceMode.Force);
            else
                rb.AddForce(moveDirection * airMultiplier * stats.numericals["moveSpeed"], ForceMode.Force);

            return;
        }
        //sliding
        if (!crouchReleased && movementState == MovementState.SLIDING)
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
            
            slide.transform.rotation = Quaternion.LookRotation(slideDirection);
            if (!slide.isPlaying)
                slide.Play();
            ability.Slide(slideDirection, slideSpeed * stats.numericals["moveSpeed"]);
            return;
        }
        //slamming
        if (movementState == MovementState.SLAMMING)
        {
            extraJumpForce += slamJumpForceRate;
            ability.GroundSlam(slamSpeed * stats.numericals["moveSpeed"]);
        }
    }

    private void LaunchLogic()
    {
        if (stamina.GetCurrentStamina() < 100f)
            return;
        if (launchInInput.GetInputDown())
        {
            ability.LaunchIn(launchPoint, launchForce * stats.numericals["moveSpeed"]);
            movementState = MovementState.BOUNCING;
            launchInterval.ResetEarly();
            stamina.ReduceStamina(100f);
        }
    }

    private void Start()
    {
        camShake = GetComponent<CameraShake>();
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
        launchInInput.Update();
        if (!point.IsUnityNull())//this check covers specifically the enemies when they die while you are in contact with them
        {
            if (point.otherCollider == null)
                airborne = true;
        }
        if (jumpInput.GetInputDown() && currentJumps < maxJumps)
        {
            currentJumps++;
            if (movementState == MovementState.LOCKED)
            {
                lockInterval.ResetEarly();
                movementState = MovementState.WALKING;
                return;
            }
            if (!airborne)
            {
                if (!slamJumpInterval.enabled)
                    ability.Jump(point, jumpForce);
                else
                    ability.Jump(point, jumpForce+extraJumpForce);
                extraJumpForce = 2f;
                movementState = MovementState.WALKING;
                crouchReleased = true;
            }
            else
            {
                if (stamina.GetCurrentStamina() < 50f)
                    return;
                stamina.ReduceStamina(50f);
                lockEffect.Play();
                movementState = MovementState.LOCKED;
                lockInterval.enabled = true;
            }
            return;
        }
        if (dashInput.GetInputDown() && stamina.GetCurrentStamina() > 50f)
        {
            dash.transform.position = transform.position;
            if (moveDirection != Vector3.zero)
            {
                dash.transform.rotation = Quaternion.LookRotation(moveDirection.normalized);
                dash.Play();
                ability.Dash(moveDirection.normalized, dashSpeed * stats.numericals["moveSpeed"]);
            }
            else
            {
                dash.transform.rotation = camHolder.rotation;
                dash.Play();
                ability.Dash(camHolder.forward, dashSpeed * stats.numericals["moveSpeed"]);
            }
            stamina.ReduceStamina(50f);
            movementState = MovementState.WALKING;
            rb.drag = 2f;
            crouchReleased = false;
            return;
        }
        if (slideInput.GetInputDown())
        {
            uponSlide = true;
            if (!airborne && crouchReleased)
            {
                movementState = MovementState.SLIDING;
            }
            if (airborne && crouchReleased && stamina.GetCurrentStamina() >= 25f)
            {
                movementState = MovementState.SLAMMING;
                stamina.ReduceStamina(25f);
                slamJumpInterval.ResetEarly();
            }
        }
        if (slideInput.GetInput())
        {
            crouchReleased = false;
        }
        else
        {
            crouchReleased = true;
        }
        if (movementState == MovementState.LOCKED)
        {
            return;
        }
        if (launchInterval.enabled)
        {
            LaunchLogic();
        }
        //Debug.Log(movementState.ToString());
        if (movementState == MovementState.SLIDING && !crouchReleased)
            return;
        if (movementState == MovementState.SLAMMING)
            return;
        if (movementState == MovementState.BOUNCING)
            return;
        if (slide.isPlaying)
            slide.Stop();
        movementState = MovementState.WALKING;
    }

    // used to be this comment said something really stupid
    private void FixedUpdate() => Move();
    
    private void OnCollisionEnter(Collision collision)
    {
        point = collision.GetContact(0);
        airborne = false;
        launchInterval.ResetEarly();
        if (movementState == MovementState.SLAMMING)
        {
            slamJumpInterval.enabled = true;
            groundSlam.transform.position = point.point;
            groundSlam.Play();
            movementState = MovementState.WALKING;
        }
        if (movementState != MovementState.BOUNCING)
            return;
        bounces++;
        ability.Bounce(point);
        if (collision.collider.gameObject.tag == "Enemy")
        {
            camShake.Shake();
            collision.collider.gameObject.GetComponent<IDamageable>().TakeDamage(100f,gameObject,1f,0);
            bounces = maxBounces;
        }
        if (bounces >= maxBounces)
        {
            rb.velocity = ability.GetBounceDir() * launchForce * stats.numericals["moveSpeed"];
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
        if (movementState == MovementState.LOCKED)
        {
            lockInterval.ResetEarly();
            movementState = MovementState.WALKING;
        }
        if (movementState == MovementState.BOUNCING)
            return;
        rb.drag = 5f;
    }

    private void OnCollisionExit()
    {
        airborne = true;
        launchInterval.enabled = !disableLaunch;
        launchPoint = transform.position;
        rb.drag = 2f;
    }
}
