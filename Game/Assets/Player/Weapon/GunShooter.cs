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
        SINGULARITY,
        DOUBLE_STANDARD,
        CERBERUS,
        TETRIPLEX,
        CINCOS,
        IMPOSSIBLE
    }

    [SerializeField] public Stats stats;
    [SerializeField] private Transform firepoint;
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
        transform.localPosition += recoilPosition*recoilMod*stats.numericals["focus"];
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
        rotation *= Quaternion.Euler(new Vector3(0f, -2.5f*stats.numericals["focus"], 0f));
        for (int i = 0; i < 2; i++)
        {
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(bulletPool, pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0f, 5*stats.numericals["focus"], 0f));
        }
    }

    private void Triple(string bulletPool, Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -5*stats.numericals["focus"], 0f));
        for (int i = 0; i < 3; i++)
        {
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(bulletPool, pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0f, 5*stats.numericals["focus"], 0f));
        }
    }

    private void Quadruple(string bulletPool, Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -7.5f*stats.numericals["focus"], 0f));
        for (int i = 0; i < 4; i++)
        {
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(bulletPool, pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0f, 4.6875f*stats.numericals["focus"], 0f));//3.75f-7.5f
        }
    }

    private void Quintuple(string bulletPool, Vector3 pos, Quaternion rotation)
    {
        rotation *= Quaternion.Euler(new Vector3(0f, -10f*stats.numericals["focus"], 0f));
        for (int i = 0; i < 5; i++)
        {
            rotation.x = Mathf.Clamp(rotation.x, -89f, 89f);
            SpawnBullet(bulletPool, pos, rotation);
            rotation *= Quaternion.Euler(new Vector3(0f, 5f*stats.numericals["focus"], 0f));
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
        if (RunDataSave.rData.echelonType == Echelon.IMPOSSIBLE){
            RunDataSave.rData.echelonType = PlayerInfo.GetEchelon();
            weaponType = PlayerInfo.GetEchelon();
        }else{
            weaponType = RunDataSave.rData.echelonType;
        }

        PlayerInfo.SetGun(this);
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
        WeaponLogic();
        fireInput.Update();
        fire2Input.Update();
        if (fireInput.GetInput() && readyToShoot)
        {
            ShootState(weapons[scroll.index].recoilModifier*stats.numericals["rate"]);
            OnShootEvent.Invoke(weapons[scroll.index].fireRate*weapons[scroll.index].modifier);
            shootMethods[index](weapons[scroll.index].bulletPool, firepoint.position, GetAccurateRotation(cam,firepoint));
            t = weapons[scroll.index].fireRate*stats.numericals["rate"];
        }
        if (fire2Input.GetInput() && readyToShoot && weapons[scroll.index].extra)
        {
            ShootState(weapons[scroll.index].extraRecoilModifier*stats.numericals["rate"]);
            OnShootEvent.Invoke(weapons[scroll.index].extraFireRate*weapons[scroll.index].modifier);
            shootMethods[index](weapons[scroll.index].extraBulletPool, firepoint.position, GetAccurateRotation(cam,firepoint));
            t = weapons[scroll.index].extraFireRate*stats.numericals["rate"];
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
        this.enabled = !state;
    }
}
