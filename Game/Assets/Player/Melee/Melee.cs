using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Melee : MonoBehaviour
{
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

    private RaycastHit hitInfo;
    private bool block = false;
    private float t;

    private bool Punch()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(cam.scaledPixelWidth / 2, cam.scaledPixelHeight / 2,0));
        bool hit = Physics.Raycast(ray,out hitInfo,punchRange);
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
        if (meleeInput.GetInputDown() && animator.GetCurrentAnimatorStateInfo(0).IsName("Nun") && once)//nun is idle btw
        {
            once = false;
            animator.SetBool("punch", true);
            block = false;
            return;
        }else if (animator.GetNextAnimatorStateInfo(0).IsName("Nun"))
        {
            once = true;
            animator.SetBool("punch", false);
        }
        if (block)
            return;
        if (animator.GetBool("punch"))
        {
            t += Time.deltaTime;
            if (t >= punchDelay)
            {
                t = 0;
                bool hit = Punch();
                if (hit)
                {
                    shakeEffect.Shake();
                    Instantiate(particlePrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                }
                block = true;
            }
        }
    }
}
