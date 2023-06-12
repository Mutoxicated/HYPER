using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRB;

    [Header("Thresholds")]
    [SerializeField] private float xThreshold;
    [SerializeField] private float zThreshold;
    [SerializeField] private float yThreshold;

    [Space]
    [Header("Multipliers")]
    [SerializeField] private float xMultiplier;
    [SerializeField] private float zMultiplier;
    [SerializeField, Range(1f, 6f)] private float snapiness;

    private float move_x, move_z;
    private Vector3 point = Vector3.zero;

    private void Update()
    {
        move_x = Mathf.Clamp(Input.GetAxisRaw("Vertical")* xMultiplier, -xThreshold, xThreshold);
        move_z = Mathf.Clamp(Input.GetAxisRaw("Horizontal")* zMultiplier, -zThreshold, zThreshold);
    }

    private void FixedUpdate()
    {
        //point = new Vector3(-move_x, Mathf.Clamp(-playerRB.velocity.normalized.y, -yThreshold, yThreshold), -move_z);
        point.x = -move_x;
        point.y = Mathf.Clamp(-playerRB.velocity.normalized.y, -yThreshold, yThreshold);
        point.z = move_z;
        transform.localPosition = Vector3.Lerp(transform.localPosition, point, Time.fixedDeltaTime * snapiness);
    }
}
