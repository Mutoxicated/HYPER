using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.ParticleSystem;

public class GunShooter : MonoBehaviour
{
    public enum Echelon
    {
        SINGULARITY,
        DOUBLE_STANDARD,
        CERBERUS,
        TETRIPLEX,
        CINCOS
    }

    [SerializeField] public Stats stats;
    [SerializeField] private Transform firepoint;
    //[SerializeField] private Transform transform;
    [SerializeField] private Camera cam;
    private Vector3 defaultPos;
    private Quaternion defaultRot;

    [Header("Prefabs")]
    [SerializeField] private GameObject gunScrew;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private ParticleSystem particle;
    [Space]
    [Header("Gun Settings")]
    [SerializeField] private AudioSource shootSFX;
    [SerializeField] public float fireRate;
    [SerializeField] private Vector3 recoilPosition;
    [SerializeField] private Quaternion recoilRotation;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private Echelon weaponType;

    private bool readyToShoot = true;
    private float t = 0f;
    private bool running = false;
    private ButtonInput fireInput = new ButtonInput("Fire1");

    public delegate void Methods(Vector3 pos, Quaternion rotation);
    private Methods[] shootMethods;
    private int index;

    [HideInInspector] public Queue<GameObject> bulletQueue = new Queue<GameObject>();
    [HideInInspector] public UnityEvent OnShootEvent = new UnityEvent();

    public delegate void extraShootMethods(Vector3 pos, Quaternion rotation);
    public List<extraShootMethods> shootAbilities = new List<extraShootMethods>();
    private ButtonInput fire2Input = new ButtonInput("Fire2");

    private Scroll scrollWheel = new Scroll(0, 5);

    public int GetWeaponTypeInt()
    {
        return index;
    }

    private void Recoil()
    {
        transform.localPosition += recoilPosition;
        transform.localRotation *= recoilRotation;
    }

    private void ShootState()
    {
        particle.Play();
        Recoil();
        shootSFX.Play();
        readyToShoot = false;
        running = true;
    }

    //rotation point from firepoint to the crosshair
    public static Quaternion GetAccurateRotation(Camera cam, Transform firepoint)
    {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(new Vector3(cam.scaledPixelWidth/2, cam.scaledPixelHeight/2, 0));
        var gotHit = Physics.Raycast(ray, out hit, Mathf.Infinity);
        Vector3 point = hit.point;
        if (!gotHit)
        {
            point = ray.GetPoint(50f);
        }
        return Quaternion.LookRotation(point - firepoint.position);
    }

    private void SpawnBullet(Vector3 pos, Quaternion rotation)
    {
        if (bulletQueue.Count == 0 || bulletQueue.Peek().activeSelf)
        {
            var instance = Instantiate(bulletPrefab, pos, rotation);
            bulletQueue.Enqueue(instance);
        }
        else// reuse a bullet
        {
            var instance = bulletQueue.Dequeue();
            instance.transform.position = pos;
            instance.transform.rotation = rotation;
            instance.SetActive(true);
            bulletQueue.Enqueue(instance);
        }
    }
    private void Single(Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0, 0, 0));
        SpawnBullet(pos, rotation);
    }

    private void Double(Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -2.5f, 0f));
        for (int i = 0; i < 2; i++)
        {
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0f, 5, 0f));
        }
    }

    private void Triple(Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -5, 0f));
        for (int i = 0; i < 3; i++)
        {
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0f, 5, 0f));
        }
    }

    private void Quadruple(Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -7.5f, 0f));
        for (int i = 0; i < 4; i++)
        {
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0f, 4.6875f, 0f));//3.75f-7.5f
        }
    }

    private void Quintuple(Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -10f, 0f));
        for (int i = 0; i < 5; i++)
        {
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0f, 5f, 0f));
        }
    }

    private void Start()
    {
        shootMethods = new Methods[] { Single, Double, Triple, Quadruple, Quintuple };
        defaultPos = transform.localPosition;
        defaultRot = transform.localRotation;
        index = (int)weaponType;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;
        fireInput.Update();
        fire2Input.Update();
        if (fireInput.GetInput() && readyToShoot)
        {
            ShootState();
            OnShootEvent.Invoke();
            shootMethods[index](firepoint.position, GetAccurateRotation(cam,firepoint));
        }
        if (fire2Input.GetInputDown() && shootAbilities.Count > 0)
        {
            shootAbilities[scrollWheel.index](firepoint.position,firepoint.parent.transform.rotation);
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            scrollWheel.Increase();
        }else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            scrollWheel.Decrease();
        }
        
        transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPos, Time.deltaTime*lerpSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, defaultRot, Time.deltaTime * lerpSpeed);

        if (!running)
            return;
        t += Time.deltaTime * stats.numericals["shootSpeed"];
        if (t > fireRate)
        {
            readyToShoot = true;
            t = 0f;
            running = false;
        }
    }
}
