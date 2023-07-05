using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Animator animator;
    [SerializeField] private int punchDamage;
    [SerializeField] private float punchRange;
    [SerializeField] private float punchDelay;
    [SerializeField] private CameraShake shakeEffect;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private GameObject camHolder;

    private ButtonInput meleeInput = new ButtonInput("Melee");
    private Camera cam;
    private bool once = true;
    private bool punching = false;

    private RaycastHit hitInfo;
    private bool block = true;
    private float t;

    private bool Punch()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(cam.scaledPixelWidth / 2, cam.scaledPixelHeight / 2,0));
        bool hit = Physics.SphereCast(ray.origin,0.1f, ray.direction, out hitInfo, punchRange, layerMask);
        if (hit)
            hitInfo.collider.gameObject.GetComponent<IDamagebale>()?.TakeDamage(punchDamage,gameObject);
        return hit;
    }

    private void Start()
    {
        animator.Play("Nun");
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        shakeEffect._cam = camHolder;
    }

    private void Update()
    {
        meleeInput.Update();
        if (meleeInput.GetInputDown() && !punching && once)
        {
            //Debug.Log("non");
            once = false;
            animator.Play("Pwanch");
            punching = true;
            block = false;
        }
        else if (punching && animator.GetCurrentAnimatorStateInfo(0).IsName("Nun"))//nun is idle btw
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
            animator.SetFloat("speedMult", 0.85f);
            if (hit)
            {
                shakeEffect.Shake();
                Instantiate(particlePrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
            }
            block = true;
        }
    }
}
