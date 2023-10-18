using UnityEngine;

public class MoveAbilities
{
    private float t;
    private Rigidbody rb;
    private float bounceSpeed;
    private Vector3 bounceDirection;
    public float speed;

    public MoveAbilities(Rigidbody rb)
    {
        this.rb = rb;
    }

    public void Jump(ContactPoint point, float jumpForce)
    {   
        if (point.normal.y < 0.9f)
        {
            rb.velocity = Vector3.zero;
        }
        Vector3 finalNormal = point.normal;
        finalNormal.y = 1f;
        rb.AddForce(finalNormal.normalized * jumpForce, ForceMode.Impulse);
    }

    public void Slide(Vector3 direction, float slideSpeed)
    {
        direction *= slideSpeed;
        direction.y = rb.velocity.y;
        rb.velocity = direction;
    }

    public void GroundSlam(float slamSpeed)
    {
        rb.velocity = Vector3.down * slamSpeed;
    }

    public void Dash(Vector3 direction, float curSpeed)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(direction * curSpeed, ForceMode.Impulse);
    }

    public void Lock(float t)
    {
        rb.velocity = Vector3.Lerp(rb.velocity, rb.velocity.normalized*0.5f, t);
        //rb.drag = Mathf.Lerp(2f, 7f, t);
    }

    public void LaunchIn(Vector3 point, float launchSpeed)
    {
        bounceSpeed = launchSpeed;
        bounceDirection = (point - rb.position).normalized * bounceSpeed;
        rb.velocity = bounceDirection;
        rb.drag = 0f;
    }

    public void LaunchOut(Vector3 point, float launchSpeed)
    {
        bounceSpeed = launchSpeed;
        bounceDirection = (rb.position - point).normalized * bounceSpeed;
        rb.velocity = bounceDirection;
        rb.drag = 0f;
    }

    public void Bounce(ContactPoint point)
    {
        bounceDirection = Vector3.Reflect(bounceDirection, point.normal).normalized * bounceSpeed;
        rb.velocity = Vector3.zero;
    }

    public Vector3 GetBounceDir()
    {
        return bounceDirection;
    }
}