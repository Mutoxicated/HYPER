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

    private ButtonInput meleeInput = new ButtonInput("Melee");
    private Camera cam;
    private bool once = true;
    private bool block = false;
    private int t;

    private void Punch()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(cam.scaledPixelWidth / 2, cam.scaledPixelHeight / 2,0));
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(ray,out hitInfo,punchRange);
        if (hit)
            hitInfo.collider.gameObject.GetComponent<IDamagebale>()?.TakeDamage(punchDamage,gameObject);
    }

    private void Start()
    {
        animator.Play("Nun");
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    private void FixedUpdate()
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
            t++;
            if (t >= punchDelay)
            {
                t = 0;
                Punch();
                block = true;
            }
        }
    }
}
