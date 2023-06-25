using UnityEngine;
using UnityEngine.UIElements;

public class GunController : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private GunShooter shooter;

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
    private Transform gunScrew;
    private float angle = 0f;

    private void Start()
    {
        gunScrew = transform.GetChild(0);
        shooter.OnShootEvent.AddListener(RotateScrew);
    }

    private void Update()
    {
        move_x = Mathf.Clamp(Input.GetAxisRaw("Vertical") * xMultiplier, -xThreshold, xThreshold);
        move_z = Mathf.Clamp(Input.GetAxisRaw("Horizontal") * zMultiplier, -zThreshold, zThreshold);
        //point = new Vector3(-move_x, Mathf.Clamp(-playerRB.velocity.normalized.y, -yThreshold, yThreshold), -move_z);
        point.x = -move_x;
        point.y = Mathf.Clamp(-playerRB.velocity.normalized.y, -yThreshold, yThreshold);
        point.z = move_z;
        transform.localPosition = Vector3.Lerp(transform.localPosition, point, Time.deltaTime * snapiness);

        gunScrew.localRotation = Quaternion.Lerp(
            gunScrew.localRotation, 
            Quaternion.AngleAxis(angle, new Vector3(0f, 0f, 1f)), 
            Time.fixedDeltaTime * (shooter.fireRate * shooter.fireRateMod / 90f));
    }

    private void RotateScrew()
    {
        angle += 90f;
    }
}
