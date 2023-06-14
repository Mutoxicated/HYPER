using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GunShooter : MonoBehaviour
{
    public enum WeaponType
    {
        SINGLE,
        DOUBLE,
        TRIPLE
    }

    [SerializeField] private Transform firepoint;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private Camera cam;
    private Vector3 defaultPos;
    private Quaternion defaultRot;

    [Header("Prefabs")]
    [SerializeField] private GameObject gunScrew;
    [SerializeField] private GameObject bulletPrefab;
    [Space]
    [Header("Gun Settings")]
    [SerializeField, Range(500f,4000f)] private float fireRate;//in milliseconds
    [SerializeField] private Vector3 recoilPosition;
    [SerializeField] private Quaternion recoilRotation;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private WeaponType weaponType;

    private bool readyToShoot = true;
    private Stopwatch stopwatch = new Stopwatch();
    private ButtonInput fireInput = new ButtonInput("Fire1");

    public delegate void Methods(Vector3 pos, Quaternion rotation);

    private Methods[] shootMethods;
    private int index;

    private void Recoil()
    {
        weaponHolder.localPosition += recoilPosition;
        weaponHolder.localRotation *= recoilRotation;
    }

    private void ShootState()
    {
        Recoil();
        readyToShoot = false;
        stopwatch.Start();
        UnityEngine.Debug.Log("Shoot");
    }

    //rotation point from firepoint to the crosshair
    private float GetAccurateXrotation()
    {
        RaycastHit hit;
        Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, Mathf.Infinity, 0);
        return Quaternion.LookRotation(hit.point - firepoint.position).x;
    }

    private void SpawnBullet(Vector3 pos, Quaternion rotation)
    {
        WeaponInfo.OnShootEvent.Invoke();
        ShootState();
        if (WeaponInfo.bulletQueue.Count == 0 || WeaponInfo.bulletQueue.Peek().activeSelf)
        {
            var instance = Instantiate(WeaponInfo.bulletPrefab, pos, rotation);
            WeaponInfo.bulletQueue.Enqueue(instance);
        }
        else// reuse a bullet
        {
            var instance = WeaponInfo.bulletQueue.Dequeue();
            instance.transform.position = pos;
            instance.transform.rotation = rotation;
            instance.SetActive(true);
            WeaponInfo.bulletQueue.Enqueue(instance);
        }
    }
    private void Single(Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(GetAccurateXrotation() - rotation.x, 0, 0));
        SpawnBullet(pos, rotation);
    }

    private void Double(Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(GetAccurateXrotation() - rotation.x, -4.5f, 0));
        for (int i = 0; i < 2; i++)
        {
            SpawnBullet(pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0, 9, 0));
        }
    }

    private void Triple(Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(GetAccurateXrotation() - rotation.x, -10, 0));
        for (int i = 0; i < 3; i++)
        {
            SpawnBullet(pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0, 10, 0));
        }
    }

    private void Awake()
    {
        WeaponInfo.fireRate = fireRate;
        WeaponInfo.bulletPrefab = bulletPrefab;
    }

    private void Start()
    {
        shootMethods = new Methods[] { Single, Double, Triple };
        defaultPos = weaponHolder.localPosition;
        defaultRot = weaponHolder.localRotation;
        index = (int)weaponType;
    }

    private void Update()
    {
        fireInput.Update();
        if (fireInput.GetInputDown() && readyToShoot)
        {
            shootMethods[index](firepoint.position,firepoint.rotation);
        }

        weaponHolder.localPosition = Vector3.Lerp(weaponHolder.localPosition, defaultPos, Time.deltaTime*lerpSpeed);
        weaponHolder.localRotation = Quaternion.Lerp(weaponHolder.localRotation, defaultRot, Time.deltaTime * lerpSpeed);

        if (!stopwatch.IsRunning)
            return;
        if (stopwatch.ElapsedMilliseconds > fireRate*WeaponInfo.fireRateMod)
        {
            readyToShoot = true;
            stopwatch.Reset();
        }
    }
}
