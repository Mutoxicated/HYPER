using UnityEngine;
using static Numerical;
using static GunScrewState;

enum GunScrewState {
    PRESSED,
    NOT_PRESSED,
    COOLDOWN,
}

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
    private float rotationMod = 1f;

    private float requiredLerp = 1f;

    private void Start()
    {
        shooter.OnShootEvent.AddListener(UpdateRotationMod);
        scroll.AlterIndex(1);
        gunScrew = transform.GetChild(0);
        points[0] = new Vector3(0f,yLow,-zMax);
        points[1] = new Vector3(0f,0f,0f);
        points[2] = new Vector3(0f,yLow,zMax);
    }

    private void UpdateRotationMod(float mod) {
        rotationMod = mod;
    }

    private GunScrewState RotateGunScrew() {
        if (shooter.isOnCooldown()) {
            //Debug.Log("is on cooldown");
            angle -= 5f;
            once2 = false;
            return COOLDOWN;
        }
        if (!once2 && !shooter.isOnCooldown()) {
            //Debug.Log("no longer on cooldown");
            angle = gunScrew.eulerAngles.z;
            angle -= angle % 73f;
            once2 = true;
            once = true;
            return NOT_PRESSED;
        }
        if (pic.IsPressed("Shoot") || (pic.IsPressed("ExtraShoot") && shooter.GetCurrentWeapon().extraEnabled)) {
            //Debug.Log("is pressed");
            once = false;
            angle += 5f*rotationMod;
            return PRESSED;
        }else {
            if (!once) {
                //Debug.Log("no longer pressed");
                angle = gunScrew.eulerAngles.z;
                //Debug.Log(angle);
                //Debug.Log(angle % 73f);
                angle += 73f-angle % 73f;
                once = true;
            }
            return NOT_PRESSED;
        }
    }

    private float InverseLerp(float a, float b, float value) {
        if (a == b) {
            return 0f;
        }
        return (value - a) / (b - a);
    }

    private void PullRotation() {
        float normalAngle;
        if (angle == 360f) {
            normalAngle = 360f;
        }else {
            normalAngle = angle % 360f;
        }
        //Debug.Log("Angle: "+normalAngle+", AAngle: "+gunScrew.eulerAngles.z);
        requiredLerp = Mathf.Abs(gunScrew.eulerAngles.z-normalAngle)/360f;
        if (requiredLerp < 1f) {
            requiredLerp = 1f;
        }
        requiredLerp = Mathf.Sqrt(requiredLerp)*3f;
    }

    private void Update()
    {
        if (shooter.GetIsLocked())
            return;
        move_x = Mathf.Clamp(pic.GetVertical() * xMultiplier, -xThreshold, xThreshold);
        move_z = Mathf.Clamp(pic.GetHorizontal() * zMultiplier, -zThreshold, zThreshold);
        //point = new Vector3(-move_x, Mathf.Clamp(-playerRB.velocity.normalized.y, -yThreshold, yThreshold), -move_z);
        point.x = -move_x;
        point.y = Mathf.Clamp(-playerRB.velocity.normalized.y* yMultiplier, -yThreshold, yThreshold);
        if (!movement.airborne)
            point.y = 0f;
        point.z = move_z;
        anchor.localPosition = Vector3.Lerp(anchor.localPosition, point, Time.deltaTime * snapiness);

        gunHolder.transform.localPosition = Vector3.Lerp(gunHolder.transform.localPosition,points[scroll.index],Time.deltaTime*movement.stats.numericals[MOVE_SPEED]*6f);

        walkInterval.ChangeInterval(interval*movement.stats.numericals[MOVE_SPEED]);

        if (pic.GetWASDIsPressed() && !walkInterval.enabled){
            walkInterval.enabled = true;
        }
        
        //relation ship: gunScrew.localEulerAngles and 
        if (RotateGunScrew() == NOT_PRESSED) {
            requiredLerp = 1f;
        }else {
            PullRotation();
        }

        gunScrew.localRotation = Quaternion.Lerp(
            gunScrew.localRotation,
            Quaternion.AngleAxis(angle, new Vector3(0f, 0f, 1f)),
            Time.deltaTime * 7 * requiredLerp);
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
