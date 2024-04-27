using UnityEngine;
using UnityEngine.UIElements;

public class GunController : MonoBehaviour
{
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private GunShooter shooter;
    [SerializeField] private Movement movement;
    [SerializeField] private PlayerInputContext pic;
    [SerializeField] private Transform gunHolder;
    [SerializeField] private Transform anchor;

    [Header("Walk Variables")]
    [SerializeField] private OnInterval walkInterval;
    [SerializeField] private float interval;
    [SerializeField] private float zMax;
    [SerializeField] private float yLow;

    [Header("Thresholds")]
    [SerializeField] private float xThreshold;
    [SerializeField] private float zThreshold;
    [SerializeField] private float yThreshold;

    [Space]
    [Header("Multipliers")]
    [SerializeField] private float xMultiplier;
    [SerializeField] private float zMultiplier;
    [SerializeField] private float yMultiplier;
    [SerializeField, Range(1f, 6f)] private float snapiness;

    private float move_x, move_z;
    private Vector3 point = Vector3.zero;
    private Transform gunScrew;
    private float angle = 0f;

    private Vector3 gunHolderPoint;
    private Scroll scroll = new Scroll(0,2);
    private bool reversing = false;
    private Vector3[] points = new Vector3[3];

    private bool once;
    private bool once2 = true;

    private void Start()
    {
        scroll.AlterIndex(1);
        gunScrew = transform.GetChild(0);
        points[0] = new Vector3(0f,yLow,-zMax);
        points[1] = new Vector3(0f,0f,0f);
        points[2] = new Vector3(0f,yLow,zMax);
    }

    private void RotateGunScrew() {
        if (shooter.isOnCooldown()) {
            angle -= 10f;
            once2 = false;
            return;
        }
        if (!once2) {
            angle -= angle % 73f;
            once2 = true;
            once = true;
        }
        if (pic.IsPressed("Shoot") || (pic.IsPressed("ExtraShoot") && shooter.GetCurrentWeapon().extraEnabled)) {
            once = false;
            angle += 10f;
        }else {
            if (!once) {
                angle = gunScrew.localEulerAngles.z;
                Debug.Log(angle);
                Debug.Log(angle % 73f);
                angle += 73f-angle % 73f;
                once = true;
            }
        }
    }

    private void Update()
    {
        move_x = Mathf.Clamp(pic.GetVertical() * xMultiplier, -xThreshold, xThreshold);
        move_z = Mathf.Clamp(pic.GetHorizontal() * zMultiplier, -zThreshold, zThreshold);
        //point = new Vector3(-move_x, Mathf.Clamp(-playerRB.velocity.normalized.y, -yThreshold, yThreshold), -move_z);
        point.x = -move_x;
        point.y = Mathf.Clamp(-playerRB.velocity.normalized.y* yMultiplier, -yThreshold, yThreshold);
        if (!movement.airborne)
            point.y = 0f;
        point.z = move_z;
        anchor.localPosition = Vector3.Lerp(anchor.localPosition, point, Time.deltaTime * snapiness);

        gunHolder.transform.localPosition = Vector3.Lerp(gunHolder.transform.localPosition,points[scroll.index],Time.deltaTime*movement.stats.numericals["moveSpeed"]*6f);

        walkInterval.ChangeInterval(interval*movement.stats.numericals["moveSpeed"]);

        if (pic.GetWASDIsPressed() && !walkInterval.enabled){
            walkInterval.enabled = true;
        }
        RotateGunScrew();
        gunScrew.localRotation = Quaternion.Lerp(
            gunScrew.localRotation,
            Quaternion.AngleAxis(angle, new Vector3(0f, 0f, 1f)),
            Time.deltaTime * 11);
    }

    public void NextWalkIteration(){
        if (!pic.GetWASDIsPressed() | movement.airborne){
            if (scroll.index == 1){
                walkInterval.enabled = false;
            }
            scroll.AlterIndex(1);
        }else{
            if (scroll.index == scroll.GetMaxIndex()){
                reversing = true;
            }else if (scroll.index == 0){
                reversing = false;
            }
            if (reversing){
                scroll.Decrease();
            }else{
                scroll.Increase();
            }
        }
    }
}
