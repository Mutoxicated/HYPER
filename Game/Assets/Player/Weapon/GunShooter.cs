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
    [SerializeField] private ParticleSystem particle;
    [Space]
    [Header("Gun Settings")]
    [SerializeField] private AudioSource shootSFX;
    [SerializeField] private Vector3 recoilPosition;
    [SerializeField] private Quaternion recoilRotation;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private Echelon weaponType;

    [SerializeField] private List<Weapon> weapons = new List<Weapon>();

    private bool readyToShoot = true;
    private float t = 0f;
    private bool running = false;
    private ButtonInput fireInput = new ButtonInput("Fire1");

    public delegate void Methods(string bulletPool, Vector3 pos, Quaternion rotation);
    private Methods[] shootMethods;
    private int index;

    [HideInInspector] public UnityEvent<float> OnShootEvent = new UnityEvent<float>();

    private ButtonInput fire2Input = new ButtonInput("Fire2");

    public Scroll scroll = new Scroll(0, 5);

    public int GetWeaponTypeInt()
    {
        return index;
    }

    private void Recoil(float recoilMod)
    {
        transform.localPosition += recoilPosition*recoilMod;
        Quaternion modifiedRotation = recoilRotation;
        if (modifiedRotation.x != 0) {
            modifiedRotation.x = modifiedRotation.x * recoilMod;
        }
        if (modifiedRotation.y != 0) {
            modifiedRotation.y = modifiedRotation.y * recoilMod;
        }
        if (modifiedRotation.z != 0) {
            modifiedRotation.z = modifiedRotation.z * recoilMod;
        }
        transform.localRotation *= modifiedRotation;
    }

    private void ShootState(float rcMod)
    {
        particle.Play();
        Recoil(rcMod);
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

    private void SpawnBullet(string bulletPool, Vector3 pos, Quaternion rotation)
    {
        PublicPools.pools[bulletPool].UseObject(pos,rotation);
    }
    private void Single(string bulletPool, Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0, 0, 0));
        SpawnBullet(bulletPool, pos, rotation);
    }

    private void Double(string bulletPool, Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -2.5f, 0f));
        for (int i = 0; i < 2; i++)
        {
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(bulletPool, pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0f, 5, 0f));
        }
    }

    private void Triple(string bulletPool, Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -5, 0f));
        for (int i = 0; i < 3; i++)
        {
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(bulletPool, pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0f, 5, 0f));
        }
    }

    private void Quadruple(string bulletPool, Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -7.5f, 0f));
        for (int i = 0; i < 4; i++)
        {
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(bulletPool, pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0f, 4.6875f, 0f));//3.75f-7.5f
        }
    }

    private void Quintuple(string bulletPool, Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -10f, 0f));
        for (int i = 0; i < 5; i++)
        {
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(bulletPool, pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0f, 5f, 0f));
        }
    }

    private void Awake(){
        PlayerInfo.playerGun = this;
    }

    private void Start()
    {
        scroll.AlterMaxIndex(weapons.Count-1);
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
            ShootState(weapons[scroll.index].recoilModifier);
            OnShootEvent.Invoke(weapons[scroll.index].fireRate);
            shootMethods[index](weapons[scroll.index].bulletPool, firepoint.position, GetAccurateRotation(cam,firepoint));
            t = weapons[scroll.index].fireRate;
        }
        if (fire2Input.GetInput() && readyToShoot && weapons[scroll.index].extra)
        {
            ShootState(weapons[scroll.index].extraRecoilModifier);
            OnShootEvent.Invoke(weapons[scroll.index].extraFireRate);
            shootMethods[index](weapons[scroll.index].extraBulletPool, firepoint.position, GetAccurateRotation(cam,firepoint));
            t = weapons[scroll.index].extraFireRate;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            scroll.Increase();
        }else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            scroll.Decrease();
        }
        
        transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPos, Time.deltaTime*lerpSpeed);
        transform.localRotation = Quaternion.Lerp(transform.localRotation, defaultRot, Time.deltaTime * lerpSpeed);

        if (!running)
            return;
        t -= Time.deltaTime * stats.numericals["shootSpeed"];
        if (t <= 0f)
        {
            readyToShoot = true;
            running = false;
        }
    }

    public void AddWeapon(Weapon weapon){
        weapons.Add(weapon);
    }

    public void RemoveWeapon(Weapon weapon){
        weapons.Remove(weapon);
    }

    public bool ContainsWeapon(Weapon weapon){
        if (weapons.Contains(weapon))
            return true;
        else 
            return false;
    }
}
