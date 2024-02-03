using UnityEngine;

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
    [SerializeField, Range(0f,2f)] private float airdrag;

    [Header("Timers")]
    [SerializeField] public OnInterval launchInterval;
    [SerializeField] private OnInterval lockInterval;
    [SerializeField] private OnInterval slamJumpInterval;
    [SerializeField] private OnInterval momentumWindow;

    [Header("Particles")]
    [SerializeField] private ParticleSystem groundSlam;
    [SerializeField] private ParticleSystem dash;
    [SerializeField] private ParticleSystem slide;
    [SerializeField] private ParticleSystem lockEffect;

    [Header("Misc")]
    [SerializeField] private float slamJumpForceRate;
    [SerializeField] private Stats stats;
    [SerializeField] private Transform camHolder;
    public StaminaControl stamina;
    [SerializeField] private Rigidbody rb;

    [HideInInspector] public MovementState movementState = MovementState.WALKING;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 slideDirection = Vector3.zero;
    public ContactPoint point;

    private ButtonInput jumpInput = new ButtonInput("Jump");
    private ButtonInput dashInput = new ButtonInput("Dash");
    private ButtonInput slideInput = new ButtonInput("Slide");
    private ButtonInput launchInInput = new ButtonInput("LaunchIn");

    [HideInInspector] public Vector3 launchPoint;
    [HideInInspector] public bool airborne = true;
    [HideInInspector] public GameObject sender;
    private bool crouchReleased = true;
    private bool uponSlide;
    private int currentJumps = 0;
    private int bounces = 0;
    private bool readyToJump;

    private MoveAbilities ability;
    private float moveX, moveZ;
    private CameraShake camShake;
    private int shields;

    public void ChangeState(int ms)
    {
        movementState = (MovementState)ms;
    }

    public void ChangeDrag(float n)
    {
        rb.drag = n;
    }

    public Rigidbody GetRB(){
        return rb;
    }

    public void TriggerBounceState(Vector3 point, GameObject sender){
        if (stamina.GetCurrentStamina() < 100f)
            return;
        ability.LaunchIn(point, launchForce * stats.numericals["moveSpeed"]);
        movementState = MovementState.BOUNCING;
        launchInterval.ResetEarly();
        stamina.ReduceStamina(100f);
        this.sender = sender;
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
            moveX = Input.GetAxisRaw("Horizontal") * walkSpeed;
            moveZ = Input.GetAxisRaw("Vertical") * walkSpeed;
            Vector3 flatForward = camHolder.forward;
            flatForward.y = 0f;

            moveDirection = flatForward.normalized * moveZ + camHolder.right * moveX;
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
            ability.Slide(slideDirection, ability.speed+2f);
            return;
        }
        //slamming
        if (movementState == MovementState.SLAMMING)
        {
            extraJumpForce += slamJumpForceRate;
            ability.GroundSlam(slamSpeed * stats.numericals["moveSpeed"]);
            ability.speed = -rb.velocity.y;
        }
    }

    private void LaunchLogic()
    {
        if (stamina.GetCurrentStamina() < 75f)
            return;
        if (launchInInput.GetInputDown())
        {
            ability.LaunchIn(launchPoint, launchForce * stats.numericals["moveSpeed"]);
            movementState = MovementState.BOUNCING;
            launchInterval.ResetEarly();
            stamina.ReduceStamina(75f);
        }
    }

    private void Awake(){
        PlayerInfo.SetMovement(this);
    }

    private void Start()
    {
        camShake = GetComponent<CameraShake>();
        rb.drag = airdrag;
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
        if (!point.Equals(null))//this check covers specifically the enemies when they die while you are in contact with them
        {
            if (point.otherCollider == null)
                airborne = true;
        }
        if (jumpInput.GetInputDown() && currentJumps <= maxJumps)
        {
            if (movementState == MovementState.LOCKED)
            {
                lockInterval.ResetEarly();
                movementState = MovementState.WALKING;
                return;
            }
            if (readyToJump)
            {
                currentJumps++;
                if (!momentumWindow.enabled)
                    ability.Jump(point, jumpForce);
                else if (momentumWindow.enabled && stamina.GetCurrentStamina() > 40f){
                    ability.Jump(point, jumpForce+extraJumpForce);
                    stamina.ReduceStamina(40f);
                }
                extraJumpForce = 2f;
                movementState = MovementState.WALKING;
                crouchReleased = true;
            }
            else
            {
                if (stamina.GetCurrentStamina() < 50f)
                    return;
                ability.speed = rb.velocity.magnitude;
                stamina.ReduceStamina(50f);
                lockEffect.Play();
                if (slide.isPlaying)
                    slide.Stop();
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
                if (momentumWindow.enabled | lockInterval.enabled)
                    ability.Dash(moveDirection.normalized, ability.speed+2f);
                else
                    ability.Dash(moveDirection.normalized, Mathf.Clamp(rb.velocity.magnitude+2f,dashSpeed,99999999f));
            }
            else
            {
                dash.transform.rotation = camHolder.rotation;
                dash.Play();
                if (momentumWindow.enabled | lockInterval.enabled)
                    ability.Dash(camHolder.forward, ability.speed+2f);
                else
                    ability.Dash(camHolder.forward, Mathf.Clamp(rb.velocity.magnitude+2f,dashSpeed,99999999f));
            }
            stamina.ReduceStamina(50f);
            lockInterval.ResetEarly();
            movementState = MovementState.WALKING;
            rb.drag = airdrag;
            crouchReleased = false;
            return;
        }
        if (slideInput.GetInputDown() && movementState != MovementState.LOCKED)
        {
            uponSlide = true;
            if (!airborne && crouchReleased && point.normal.y >= 0.9f) 
            {
                if (momentumWindow.enabled)
                    ability.speed = Mathf.Clamp(extraJumpForce,slideSpeed*stats.numericals["moveSpeed"],9999f);
                else
                    ability.speed = Mathf.Clamp(rb.velocity.magnitude,slideSpeed*stats.numericals["moveSpeed"],9999f);
                movementState = MovementState.SLIDING;
            }
            if (airborne && crouchReleased && stamina.GetCurrentStamina() >= 25f)
            {
                lockInterval.ResetEarly();
                movementState = MovementState.SLAMMING;
                stamina.ReduceStamina(25f);
                momentumWindow.ResetEventless();
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

    private void OnTriggerEnter(Collider coll){
        if (coll.gameObject.tag == "Enemy" | coll.gameObject.layer == 16)
            return;
        readyToJump = true;
    }

    private void OnTriggerStay(Collider coll){
        if (coll.gameObject.tag == "Enemy" | coll.gameObject.layer == 16)
            return;
        readyToJump = true;
    }

    private void OnTriggerExit(){
        readyToJump = false;
    }

    private void CollisionDamage(Collision collision){
        collision.collider.gameObject.GetComponent<IDamageable>().TakeDamage(250f*stats.numericals["damage"],stats,ref shields,1f,0);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        point = collision.GetContact(0);
        if (point.normal.y >= 0.9f)
        {
            currentJumps = 0;
        }
        if (collision.gameObject.tag != "Enemy")
            airborne = false;
        launchInterval.ResetEarly();
        if (movementState == MovementState.SLAMMING)
        {
            momentumWindow.ResetEventless();
            momentumWindow.enabled = true;
            groundSlam.transform.position = point.point;
            groundSlam.Play();
            movementState = MovementState.WALKING;
            camShake.Shake(0.1f);
            if (collision.collider.gameObject.tag == "Enemy")
            {
                CollisionDamage(collision);
            }
        }
        if (movementState != MovementState.BOUNCING)
            return;
        if (sender != null){
            Destroy(sender);
        }
        bounces++;
        ability.Bounce(point);
        if (collision.collider.gameObject.tag == "Enemy")
        {
            CollisionDamage(collision);
            bounces = maxBounces;
        }
        if (bounces >= maxBounces)
        {
            rb.velocity = ability.GetBounceDir() * launchForce * stats.numericals["moveSpeed"];
            rb.drag = airdrag;
            bounces = 0;
            movementState = MovementState.WALKING;
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        point = collision.GetContact(0);
        if (collision.gameObject.tag != "Enemy")
            airborne = false;
        if (movementState == MovementState.LOCKED)
        {
            lockInterval.ResetEarly();
            movementState = MovementState.WALKING;
        }
        if (movementState == MovementState.BOUNCING)
            return;
        rb.drag = 4f;
    }

    private void OnCollisionExit()
    {
        airborne = true;
        launchInterval.enabled = !disableLaunch;
        launchPoint = transform.position;
        rb.drag = airdrag;
    }
}
