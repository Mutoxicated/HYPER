using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GunShooter : MonoBehaviour
{
    [SerializeField] private Transform weaponHolder;
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

    private bool readyToShoot = true;
    private Stopwatch stopwatch = new Stopwatch();
    private ButtonInput fireInput = new ButtonInput("Fire1");

    private void Recoil()
    {
        weaponHolder.localPosition += recoilPosition;
        weaponHolder.localRotation *= recoilRotation;
    }

    private void Shoot()
    {
        Recoil();
        readyToShoot = false;
        stopwatch.Start();
        UnityEngine.Debug.Log("Shoot");
    }

    private void Start()
    {
        defaultPos = weaponHolder.localPosition;
        defaultRot = weaponHolder.localRotation;
    }

    private void Update()
    {
        if (stopwatch.IsRunning)
        {
            if (stopwatch.ElapsedMilliseconds > fireRate)
            {
                readyToShoot = true;
                stopwatch.Reset();
            }
        }
        fireInput.Update();
        if (fireInput.GetInputDown() && readyToShoot)
        {
            Shoot();
        }
        weaponHolder.localPosition = Vector3.Lerp(weaponHolder.localPosition, defaultPos, Time.deltaTime*lerpSpeed);
        weaponHolder.localRotation = Quaternion.Lerp(weaponHolder.localRotation, defaultRot, Time.deltaTime * lerpSpeed);
    }
}
