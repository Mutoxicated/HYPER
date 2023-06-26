using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Melee : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private int punchDamage;
    [SerializeField] private float punchRange;

    private ButtonInput meleeInput = new ButtonInput("Melee");
    private Vector3 center;
    private Camera cam;
    private bool once = true;

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

    private void Update()
    {
        meleeInput.Update();
        if (meleeInput.GetInputDown() && animator.GetCurrentAnimatorStateInfo(0).IsName("Nun") && once)//nun is idle btw
        {
            Punch();
            once = false;
            animator.SetBool("punch", true);
            return;
        }else if (animator.GetNextAnimatorStateInfo(0).IsName("Nun"))
        {
            once = true;
            animator.SetBool("punch", false);
        }

    }
}
