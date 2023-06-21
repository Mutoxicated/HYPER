using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public enum HitReaction
    {
        BASE,
        WIREFRAME,
        BOTH
    }

    [SerializeField] private Gradient healthBarGradient;
    [SerializeField] private Gradient enemyHitGradient;
    private Material[] mats;

    [SerializeField] private int HP;
    private int currentHP;

    [SerializeField] private HitReaction hitReaction;
    [SerializeField] private bool enableHealthBar;

    private float t = 0f;
    private int index;
    private string ID;

    private void Start()
    {
        currentHP = HP;
        mats = GetComponent<Renderer>().materials;
        switch (hitReaction)
        {
            case HitReaction.BASE:
                index = 0;
                ID = "_Color";
                break;
            case HitReaction.WIREFRAME:
                index = mats.Length-1;
                ID = "_WireframeBackColour";
                break;
            case HitReaction.BOTH:
                index = 0;
                ID = "_Color";
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        t = Mathf.Clamp01(t - 0.05f);
    
        if (hitReaction == HitReaction.BOTH)
        {
            mats[index].SetColor(ID, enemyHitGradient.Evaluate(t));
            mats[mats.Length-1].SetColor("_WireframeBackColour", enemyHitGradient.Evaluate(t));
            return;
        }
        mats[index].SetColor(ID, enemyHitGradient.Evaluate(t));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "bullets")
        {
            return;
        }
        t = 1f;
        currentHP--;
        if (currentHP <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }
}
