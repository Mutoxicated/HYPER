using UnityEngine;

public class MoveAbilities
{
    private float t;
    private Rigidbody rb;
    private float bounceSpeed;
    private Vector3 bounceDirection;

    public MoveAbilities(Rigidbody rb)
    {
        this.rb = rb;
    }

    public void Jump(ContactPoint point, float jumpForce)
    {   
        if (point.otherCollider.gameObject.layer != 10)
        {
            rb.velocity = Vector3.zero;
        }
        Vector3 finalNormal = point.normal;
        finalNormal.y = 1f;
        rb.AddForce(finalNormal.normalized * jumpForce, ForceMode.Impulse);
    }

    public void Slide(Vector3 direction, float slideSpeed)
    {
        direction.y = 0f;
        rb.velocity = direction * slideSpeed;
    }

    public void GroundSlam(float slamSpeed)
    {
        rb.velocity = Vector3.down * slamSpeed;
    }

    public void Dash(Vector3 direction, float dashSpeed)
    {
        rb.AddForce(direction * dashSpeed, ForceMode.Impulse);
    }

    public bool Lock(float duration)
    {
        t += Time.fixedDeltaTime;
        rb.drag = 3f;
        if (t > duration)
        {
            rb.drag = 2f;
            t = 0;
            return true;
        }
        return false;
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