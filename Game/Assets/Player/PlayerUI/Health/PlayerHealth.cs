using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour, IDamageable
{
    public enum HitReaction
    {
        BASE,
        WIREFRAME,
        BOTH
    }
    [SerializeField] private Stats stats;
    [SerializeField] private Immunity immuneSystem;
    [Header("General")]
    [SerializeField] private GameObject[] playerObjects;
    [SerializeField] private Gradient healthBarGradient;
    [SerializeField] private Gradient playerHitGradient;
    [SerializeField] private TMP_Text sumText;
    [SerializeField] private Transform healthBar;
    [SerializeField] private Image healthBarImg;
    [SerializeField] private Image healthBarBackImg;
    [SerializeField] private GameObject hurtScreen;
    [SerializeField] private FadeMatColor fadeColor;
    [SerializeField] private float lerpSpeed;
    [SerializeField] private int shields = 0;
    [SerializeField] private TMP_Text shieldsText;
    [SerializeField] private float HP;
    [SerializeField, Range(0.5f, 3f)] private float reactionSpeed = 0.05f;
    [SerializeField] private HitReaction hitReaction;

    private List<MaterialInfo> objs = new List<MaterialInfo>();
    private float currentHP;
    private float reactionT = 0f;
    private float healthT = 1f;
    private Vector3 initialScale;
    private Color playerColor;
    private Color healthBarBackColor;

    private float currentT;
    private TMP_Text shieldsParent;

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

    private void RecycleBacteria()
    {
        if (immuneSystem.foreignBacteria.Count == 0)
            return;
        foreach (var bac in immuneSystem.foreignBacteria)
        {
            PublicPools.pools[bac.gameObject.name].Reattach(bac.gameObject);
        }
    }

    private void Start()
    {
        stats.ModifyIncrementalStat("shields", shields - 1, false);
        initialScale = healthBar.localScale;
        currentHP = HP;
        healthT = currentHP / HP;
        currentT = healthT;
        sumText.text = Mathf.Round(healthT*100f).ToString() + '%';
        healthBarImg = healthBar.GetComponent<Image>();
        if (playerObjects == null)
            return;
        foreach (var obj in playerObjects)
        {
            objs.Add(new MaterialInfo(obj, obj.GetComponent<Renderer>().materials));
        }
        playerHitGradient = ChangeGradientColor(playerHitGradient, healthBarImg.color);
        ObjColorLogic();
        shieldsParent = shieldsText.transform.parent.GetComponent<TMP_Text>();
        EvaluateShields();
    }

    private void Update()
    {
        if (currentHP == 0f)
            healthT = 0f;
        else healthT = currentHP / HP;

        if (currentT != healthT)
        {
            currentT = Mathf.Lerp(currentT, healthT, Time.deltaTime*3f);
            sumText.text = Mathf.Round(currentT * 100f).ToString() + '%';
        }

        healthBar.localScale = Vector3.Lerp(
            healthBar.localScale, 
            new Vector3(Mathf.Clamp(initialScale.x * healthT,0.0002f,initialScale.x),healthBar.localScale.y, healthBar.localScale.z), 
            Time.deltaTime * lerpSpeed);

        playerColor = healthBarGradient.Evaluate(healthT);
        healthBarImg.color = playerColor;
        sumText.color = playerColor;
        healthBarBackColor = playerColor;
        healthBarBackColor.a = healthBarBackImg.color.a;
        healthBarBackImg.color = healthBarBackColor;

        if (playerObjects == null)
            return;
        reactionT = Mathf.Clamp01(reactionT - reactionSpeed*Time.deltaTime);
        //Debug.Log(reactionT);
        ObjColorLogic();
    }
    
    private void EvaluateShields()
    {
        if (shields == 0)
        {
            shieldsText.color = Color.red;
        }
        else if (shields >= 1 && shields <= 3)
        {
            shieldsText.color = Color.yellow;
        }
        else
        {
            shieldsText.color = Color.white;
        }
        shieldsParent.color = shieldsText.color;
        shieldsText.text = shields.ToString();
    }

    public void TakeDamage(float intake, GameObject sender, float arb, int index)
    {
        reactionT = 1f;
        hurtScreen.SetActive(true);
        fadeColor.ChangeGradientIndex(index);
        //Debug.Log("Shields: " + shields + " | Health outake: " + (intake / (shields + 1)));
        currentHP = Mathf.Clamp(currentHP-intake/(shields+1), 0f, HP);
        shields = Mathf.Clamp(shields - 1, 0, 999999);
        stats.ModifyIncrementalStat("shields", -1, false);
        EvaluateShields();
        playerColor = healthBarGradient.Evaluate(healthT);
        playerHitGradient = ChangeGradientColor(playerHitGradient, playerColor);
        if (currentHP <= 0)
        {
            //RecycleBacteria(); 
            //death(insane)
        }
    }

    public void TakeInjector(Injector injector)
    {
        if (injector.type == injectorType.ENEMIES)
            return;
        foreach (string bacteriaPool in injector.bacteriaPools)
        {
            if (Random.Range(0f,100f) <= injector.chance)
                PublicPools.pools[bacteriaPool].SendObject(gameObject);
        }
    }
}