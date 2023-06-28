using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamagebale
{
    public enum HitReaction
    {
        BASE,
        WIREFRAME,
        BOTH
    }
    [Header("Post-Death")]
    [SerializeField] private UnityEvent<Vector3> OnDeath = new UnityEvent<Vector3>();
    [SerializeField] private GameObject[] ObjectsToDestroy;
    [SerializeField] private GameObject particleUponDeath;
    [SerializeField] private bool detachChildren;
    [Space]
    [Header("General")]
    [SerializeField] private Gradient healthBarGradient;
    [SerializeField] private Gradient enemyHitGradient;
    [SerializeField] private int HP;
    [SerializeField,Range(0.5f,2f)] private float rate = 0.05f;

    [SerializeField] private HitReaction hitReaction;
    [SerializeField] private bool enableHealthBar;

    private Material[] mats;
    private int currentHP;
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
        t = Mathf.Clamp01(t - rate*Time.deltaTime);
    
        if (hitReaction == HitReaction.BOTH)
        {
            mats[index].SetColor(ID, enemyHitGradient.Evaluate(t));
            mats[mats.Length-1].SetColor("_WireframeBackColour", enemyHitGradient.Evaluate(t));
            return;
        }
        mats[index].SetColor(ID, enemyHitGradient.Evaluate(t));
    }

    private void DestroyStuff()
    {
        if (ObjectsToDestroy == null)
            return;
        foreach (var obj in ObjectsToDestroy)
        {
            Destroy(obj);
        }
    }

    private void Detach()
    {
        if (!detachChildren)
            return;
        transform.DetachChildren();
    }

    public void TakeDamage(int intake, GameObject sender)
    {
        t = 1f;
        currentHP -= intake;
        Debug.Log(intake);
        if (currentHP <= 0)
        {
            if (particleUponDeath != null)
                Instantiate(particleUponDeath, transform.position, Quaternion.identity);
            OnDeath.Invoke(sender.transform.position);
            Detach();
            DestroyStuff();
        }
    }

    public void TakeDamage(int intake)
    {

    }
}
