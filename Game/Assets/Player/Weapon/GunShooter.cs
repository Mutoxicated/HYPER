using System;
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
        SINGLE,
        DOUBLE,
        TRIPLE,
        QUADRUPLE,
        NA
    }

    public Stats stats;
    [SerializeField] private Transform anchor;
    [SerializeField] private Transform firepoint;
    [SerializeField] private Camera cam;
    private Vector3 defaultPos;
    private Quaternion defaultRot;

    [Header("Prefabs")]
    [SerializeField] private GameObject gunScrew;
    [SerializeField] private ParticleSystem particle;
    [Space]
    [Header("Gun Settings")]
    [SerializeField] private Gradient exhaustGradient;
    [SerializeField] private float coolingRate;
    [SerializeField] private AudioSource shootSFX;
    [SerializeField, Range(0.01f, 0.5f)] private float fireRate;
    [SerializeField] private Vector3 recoilPosition;
    [SerializeField] private Quaternion recoilRotation;
    [SerializeField] private float lerpSpeed;
    [SerializeField,Range(1f,30f)] private float angle = 30f;
    [SerializeField] private Weapon[] echelonWeapons = new Weapon[3];
    private Echelon weaponType;

    [SerializeField] private List<Weapon> weapons = new List<Weapon>();

    private bool readyToShoot = true;
    private float t = 0f;
    private float exhaustT = 0f;
    private float exhaustStep = 0.05f;
    private bool onCooldown;
    private Renderer gunScrewRenderer;
    private bool running;
    private ButtonInput fireInput = new ButtonInput("Fire1");

    private int weaponIndex;
    private int altWeaponIndex {
        get {
                if (weaponIndex == 1) {
                    return 1;
                }
                return weaponIndex-1;
            }
        set {altWeaponIndex = value;}
    }
    private int currentWeaponIndex;

    [HideInInspector] public UnityEvent<float> OnShootEvent = new UnityEvent<float>();
    private ButtonInput fire2Input = new ButtonInput("Fire2");
    public Scroll scroll = new Scroll(0, 2);

    private bool locked = false;

    public int GetWeaponTypeInt()
    {
        return weaponIndex;
    }

    public Weapon GetCurrentWeapon() {
        return weapons[scroll.index];
    }

    public bool isOnCooldown() {
        return onCooldown;
    }

    private void Recoil(float recoilMod)
    {
        anchor.localPosition += recoilPosition*recoilMod*stats.numericals["focus"];
        Quaternion modifiedRotation = recoilRotation;
        if (modifiedRotation.x != 0) {
            modifiedRotation.x = modifiedRotation.x * recoilMod*stats.numericals["focus"];
        }
        if (modifiedRotation.y != 0) {
            modifiedRotation.y = modifiedRotation.y * recoilMod*stats.numericals["focus"];
        }
        if (modifiedRotation.z != 0) {
            modifiedRotation.z = modifiedRotation.z * recoilMod*stats.numericals["focus"];
        }
        anchor.localRotation *= modifiedRotation;
    }

    private void ShootState(float rcMod)
    {
        particle.Play();
        Recoil(rcMod);
        shootSFX.Play();
        readyToShoot = false;
        running = true;
    }

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

    private void ShootBullet(string bulletPool, Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -angle*stats.numericals["focus"]/2f,0f));
        for (int i = 0; i < currentWeaponIndex; i++)
        {
            rotation *= Quaternion.Euler(new Vector3(0f, angle*stats.numericals["focus"]/(currentWeaponIndex+1), 0f));
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(bulletPool, pos, rotation);
        }

        if (currentWeaponIndex == altWeaponIndex) {
            currentWeaponIndex = weaponIndex;
        }else {
            currentWeaponIndex = altWeaponIndex;
        }
    }

    private void WeaponLogic(){
        if (Input.GetKeyDown(KeyCode.Alpha1)){
            scroll.AlterIndex(0);
            return;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2)){
            scroll.AlterIndex(1);
            return;
        } 
        if (Input.GetKeyDown(KeyCode.Alpha3)){
            scroll.AlterIndex(2);
            return;
        } 
        if (Input.GetKeyDown(KeyCode.Alpha4)){
            scroll.AlterIndex(3);
            return;
        } 
        if (Input.GetKeyDown(KeyCode.Alpha5)){
            scroll.AlterIndex(4);
            return;
        } 
        if (Input.GetKeyDown(KeyCode.Alpha6)){
            scroll.AlterIndex(5);
            return;
        } 
        if (Input.GetKeyDown(KeyCode.Alpha7)){
            scroll.AlterIndex(6);
            return;
        } 
        if (Input.GetKeyDown(KeyCode.Alpha7)){
            scroll.AlterIndex(7);
            return;
        } 
        if (Input.GetKeyDown(KeyCode.Alpha8)){
            scroll.AlterIndex(8);
            return;
        } 
        if (Input.GetKeyDown(KeyCode.Alpha9)){
            scroll.AlterIndex(9);
            return;
        } 
        if (Input.GetKeyDown(KeyCode.Alpha0)){
            scroll.AlterIndex(10);
            return;
        } 
    }

    private void Awake(){
        if (RunDataSave.rData.echelonType == Echelon.NA){
            RunDataSave.rData.echelonType = PlayerInfo.GetEchelon();
            weaponType = PlayerInfo.GetEchelon();
        }else{
            weaponType = RunDataSave.rData.echelonType;   
        }

        weaponIndex = (int)weaponType+1;
        currentWeaponIndex = weaponIndex;
        PlayerInfo.SetGun(this);

        gunScrewRenderer = gunScrew.GetComponent<Renderer>();
    }

    private void Start()
    {
        scroll.AlterMaxIndex(weapons.Count-1);
        defaultPos = anchor.localPosition;
        defaultRot = anchor.localRotation;
    }

    private void Shoot(Weapon weapon) {
        exhaustT += exhaustStep*weapon.recoilModifier;
        if (exhaustT >= 1f) {
            exhaustT = 1f;
            onCooldown = true;
        }
        ShootState(weapon.recoilModifier*stats.numericals["rate"]);
        OnShootEvent.Invoke(fireRate*weapon.fireRateModifier);
        ShootBullet(weapon.bulletPool, firepoint.position, GetAccurateRotation(cam,firepoint));
        t = fireRate*weapon.fireRateModifier*stats.numericals["rate"];
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
            return;
        if (!locked){
            WeaponLogic();
            fireInput.Update();
            fire2Input.Update();
            if (fireInput.GetInput() && readyToShoot && !onCooldown)
            {
                Shoot(weapons[scroll.index]);
            }
            else if (weapons[scroll.index].extraEnabled && fire2Input.GetInput() && readyToShoot && !onCooldown)
            {
                Shoot(weapons[scroll.index].extra);
            }
            
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                scroll.Increase();
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                scroll.Decrease();
            }
        }
        
        anchor.localPosition = Vector3.Lerp(anchor.localPosition, defaultPos, Time.deltaTime*lerpSpeed);
        anchor.localRotation = Quaternion.Lerp(anchor.localRotation, defaultRot, Time.deltaTime * lerpSpeed);

        if (exhaustT < 0f) {
            exhaustT = 0f;
        }
        exhaustT -= coolingRate*(1f*Mathf.Lerp(1f,0.5f,exhaustT))*Time.deltaTime*stats.numericals["rate"];
        if (exhaustT < 0.4f && onCooldown) {
            onCooldown = false;
        } 

        gunScrewRenderer.materials[1].color = exhaustGradient.Evaluate(exhaustT);

        if (!running)
            return;
        t -= Time.deltaTime * stats.numericals["shootSpeed"];
        if (t <= 0f)
        {
            readyToShoot = true;
            running = false;
        }
    }

    public List<Weapon> GetWeapons(){
        return weapons;
    }

    public void AddWeapon(Weapon weapon){
        weapons.Add(weapon);
        scroll.AlterMaxIndex(weapons.Count-1);
    }

    public void RemoveWeapon(Weapon weapon){
        weapons.Remove(weapon);
        scroll.AlterMaxIndex(weapons.Count-1);
    }

    public bool ContainsWeapon(Weapon weapon){
        if (weapons.Contains(weapon))
            return true;
        else 
            return false;
    }

    public void IsLocked(bool state){
        locked = state;
    }

    public bool GetIsLocked() {
        return locked;
    }
}
