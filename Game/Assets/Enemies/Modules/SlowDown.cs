using UnityEngine;
using static Numerical;

public class SlowDown : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float slowDownSpeed;
    [SerializeField] private OnInterval onInterval;
    [SerializeField] private float maxSpeed;

    private float currentSpeed;

    private void Update()
    {
        currentSpeed = Mathf.Lerp(maxSpeed * stats.GetNum(MOVE_SPEED), slowDownSpeed * stats.GetNum(MOVE_SPEED), onInterval.t);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, currentSpeed * stats.GetNum(MOVE_SPEED));
    }
}
