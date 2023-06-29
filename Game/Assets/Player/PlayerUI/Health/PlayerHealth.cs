using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour, IDamagebale
{
    public enum HitReaction
    {
        BASE,
        WIREFRAME,
        BOTH
    }

    [Header("General")]
    [SerializeField] private GameObject[] playerObjects;
    [SerializeField] private Gradient healthBarGradient;
    [SerializeField] private Gradient playerHitGradient;
    [SerializeField] private Transform healthBar;
    [SerializeField] private Image healthBarImg;
    [SerializeField] private Image healthBarBackImg;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float HP;
    [SerializeField, Range(0.5f, 3f)] private float reactionSpeed = 0.05f;
    [SerializeField] private HitReaction hitReaction;

    private List<ObjectInfo> objs = new List<ObjectInfo>();
    private float currentHP;
    private float reactionT = 0f;
    private float healthT = 1f;
    private Vector3 initialScale;

    private void EvaluateObjColor(int i, int index,string ID)
    {
        objs[i].mats[index].SetColor(ID, playerHitGradient.Evaluate(reactionT));
    }

    private void ObjColorLogic()
    {
        if (hitReaction == HitReaction.BOTH)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                EvaluateObjColor(i, 0, "_Color");
                EvaluateObjColor(i, objs[i].mats.Length - 1, "_WireframeBackColour");
            }
            return;
        }
        if (hitReaction == HitReaction.WIREFRAME)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                EvaluateObjColor(i, objs[i].mats.Length - 1, "_WireframeBackColour");
            }
            return;
        }
        if (hitReaction == HitReaction.BASE)
        {
            for (int i = 0; i < objs.Count; i++)
            {
                EvaluateObjColor(i, 0, "_Color");
            }
        }
    }

    private Gradient ChangeGradientColor(Gradient gradient, Color color)
    {
        var copyGradient = gradient;
        gradient.SetKeys(new GradientColorKey[] { new GradientColorKey(color,0f), gradient.colorKeys[1] }, gradient.alphaKeys);
        return copyGradient;
    }

    private void Start()
    {
        initialScale = healthBar.localScale;
        currentHP = HP;
        healthT = currentHP / HP;
        healthBarImg = healthBar.GetComponent<Image>();
        if (playerObjects == null)
            return;
        foreach (var obj in playerObjects)
        {
            objs.Add(new ObjectInfo(obj, obj.GetComponent<Renderer>().materials));
        }
        playerHitGradient = ChangeGradientColor(playerHitGradient, healthBarImg.color);
        ObjColorLogic();
    }

    private void Update()
    {
        if (currentHP == 0f)
            healthT = 0f;
        else healthT = currentHP / HP;
        
        healthBar.localScale = Vector3.Lerp(
            healthBar.localScale, 
            new Vector3(Mathf.Clamp(initialScale.x * healthT,0.002f,initialScale.x),healthBar.localScale.y, healthBar.localScale.z), 
            Time.deltaTime * lerpSpeed);

        Color healthColor = healthBarGradient.Evaluate(healthT);
        healthBarImg.color = healthColor;
        healthBarBackImg.color = new Color(healthColor.r, healthColor.g, healthColor.b, healthBarBackImg.color.a);
        playerHitGradient = ChangeGradientColor(playerHitGradient, healthBarImg.color);

        if (playerObjects == null)
            return;
        reactionT = Mathf.Clamp01(reactionT - reactionSpeed*Time.deltaTime);
        //Debug.Log(reactionT);
        ObjColorLogic();
    }

    public void TakeDamage(int intake, GameObject sender)
    {
        reactionT = 1f;
        currentHP = Mathf.Clamp(currentHP -= intake, 0f, HP);
        if (currentHP <= 0)
        {
            //death
        }
    }

    public void TakeDamage(int intake)
    {

    }
}