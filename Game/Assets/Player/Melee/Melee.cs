using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Animator animator;
    [SerializeField] private int punchDamage;
    [SerializeField] private float punchRange;
    [SerializeField] private float punchDelay;
    [SerializeField] private CameraShake shakeEffect;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private GameObject TNTPrefab;
    [SerializeField] private AudioSource punchSFX;

    private ButtonInput throwInput = new ButtonInput("Throw");
    private ButtonInput punchInput = new ButtonInput("Melee");
    private Camera cam;
    private bool once = true;
    private bool punching = false;
    private bool isIdle;

    private RaycastHit hitInfo;
    private Vector3 spawnCoords;
    private bool block = true;
    private float t;
    private Collider[] colls;

    private bool Punch()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(cam.scaledPixelWidth / 2, cam.scaledPixelHeight / 2,0));
        bool hit = Physics.SphereCast(ray.origin,0.1f, ray.direction, out hitInfo, punchRange, layerMask);
        if (hit){
            bool? success = hitInfo.collider.gameObject.GetComponent<IParriable>()?.Parry(cam.transform.parent.parent.gameObject,cam.transform.position+cam.transform.forward*4f);
            Debug.Log(success);
            if (success == null || success == false)
                hitInfo.collider.gameObject.GetComponent<IDamageable>()?.TakeDamage(punchDamage,gameObject,1f,0);
        }else{
            colls = Physics.OverlapSphere(ray.origin,0.1f);
            foreach (Collider coll in colls){
                if (coll.gameObject.tag != "Player"){
                    bool? success = coll.gameObject.GetComponent<IParriable>()?.Parry(cam.transform.parent.parent.gameObject,cam.transform.position+cam.transform.forward*4f);
                    if (success == null || success == false)
                        coll.gameObject.GetComponent<IDamageable>()?.TakeDamage(punchDamage,gameObject,1f,0);
                }
            }
        }
        spawnCoords = hitInfo.point;
        return hit;
    }

    private void Start()
    {
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    private void Update()
    {
        isIdle = animator.GetCurrentAnimatorStateInfo(0).IsName("Nun");//nun is idle btw
        throwInput.Update();
        punchInput.Update();
        if (throwInput.GetInputDown() && isIdle && stats.numericals["capacitor1"] > 0)
        {
            animator.Play("Throw");
            Instantiate(TNTPrefab, throwPoint.position,GunShooter.GetAccurateRotation(cam,throwPoint)*TNTPrefab.transform.rotation);
            //stats.ModifyIncrementalStat("capacitor1", -1);
        }
        if (punchInput.GetInputDown() && isIdle && !punching && once)
        {
            punchSFX.Play();
            once = false;
            animator.Play("Pwanch");
            punching = true;
            block = false;
        }
        else if (punching && isIdle)
        {
            once = true;
            animator.SetFloat("speedMult", 1.3f);
            punching = false;
        }
        if (block)
            return;
        t += Time.deltaTime;
        if (t >= punchDelay)
        {
            t = 0;

            bool hit = Punch();
            animator.SetFloat("speedMult", 0.75f);
            if (hit)
            {
                shakeEffect.Shake();
                Instantiate(particlePrefab, spawnCoords, Quaternion.LookRotation(hitInfo.normal));
            }
            block = true;
        }
    }
}
