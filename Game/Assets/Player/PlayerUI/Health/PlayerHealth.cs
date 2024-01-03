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
    [SerializeField] public Stats stats;
    [SerializeField] public Immunity immuneSystem;
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
    [SerializeField] private TMP_Text shieldsText;
    [SerializeField] private float HP;
    [SerializeField, Range(0.5f, 3f)] private float reactionSpeed = 0.05f;
    [SerializeField] private HitReaction hitReaction;

    private List<MaterialInfo> objs = new List<MaterialInfo>();
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
    private void OnEnable(){
        PlayerInfo.player = gameObject;
        PlayerInfo.playerHealth = this;
        stats.numericals["maxHealth"] = HP;
    }

    private void Start()
    {
        stats.numericals["health"] = stats.numericals["maxHealth"];
        initialScale = healthBar.localScale;
        healthT = stats.numericals["health"] / stats.numericals["maxHealth"];
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
        if (stats.numericals["health"] <= 0f){
            stats.numericals["health"] = 0f;
            healthT = 0f;
        } else healthT = stats.numericals["health"] / stats.numericals["maxHealth"];
        stats.numericals["health"] = Mathf.Clamp(stats.numericals["health"],0,stats.numericals["maxHealth"]);
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
        EvaluateShields();

        if (playerObjects == null)
            return;
        reactionT = Mathf.Clamp01(reactionT - reactionSpeed*Time.deltaTime);
        //Debug.Log(reactionT);
        ObjColorLogic();
    }
    
    private void EvaluateShields()
    {
        float allShields = stats.shields.Count+stats.numericals["permaShields"];
        if (allShields == 0)
        {
            shieldsText.color = Color.red;
        }
        else if (allShields >= 1 && allShields <= 3)
        {
            shieldsText.color = Color.yellow;
        }
        else
        {
            shieldsText.color = Color.green;
        }
        shieldsParent.color = shieldsText.color;
        shieldsText.text = allShields.ToString();
    }

    private bool EvaluateDamageIntake(Stats senderStats, float intake){
        if (stats == null){
            if (Random.Range(0f,100f) > stats.numericals["bacteriaBlockChance"])
                stats.numericals["health"] -= intake / (stats.shields.Count+stats.numericals["permaShields"] + 1);
            else return false;
            return true;
        }
        if (stats.type == EntityType.ORGANIC){
            intake*= senderStats.numericals["damageO"];
        }else{
            intake*= senderStats.numericals["damageNO"];
        }
        if (Random.Range(0f,100f) > stats.numericals["enemyBlockChance"]){
            if (Random.Range(0f,100f) > stats.numericals["enemyBlockChance"]){
                stats.numericals["health"] -= intake / (stats.shields.Count+stats.numericals["permaShields"] + 1);
                if (stats.shields.Count > 0){
                    if (stats.shields[stats.shields.Count-1].TakeDamage(intake) <= 0f)
                        stats.shields.RemoveAt(stats.shields.Count-1);
                }
            }
        }
        else return false;
        return true;
    }

    public void TakeHealth(float intake, float shield){
        stats.numericals["health"] += intake;
        stats.numericals["shields"] += shield;
    }

    public float TakeDamage(float intake, Stats senderStats, ref float shieldOut, float arb, int index)
    {
        if (!EvaluateDamageIntake(senderStats,intake)){
            return 0f;
        }
        reactionT = arb;
        hurtScreen.SetActive(true);
        fadeColor.ChangeGradientIndex(index);
        //Debug.Log("Shields: " + shields + " | Health outake: " + (intake / (shields + 1)));
        shieldOut = 0;
        playerColor = healthBarGradient.Evaluate(healthT);
        playerHitGradient = ChangeGradientColor(playerHitGradient, playerColor);
        if (stats.numericals["health"] <= 0)
        {
            //RecycleBacteria(); 
            //death(insane)
        }
        if (stats.numericals["health"] >= 0f)
            return 0f;
        else
            return stats.numericals["health"];
    }

    public float TakeDamage(float intake, Stats senderStats, float arb, int index)
    {
        if (!EvaluateDamageIntake(senderStats,intake)){
            return 0f;
        }
        reactionT = arb;
        hurtScreen.SetActive(true);
        fadeColor.ChangeGradientIndex(index);
        //Debug.Log("Shields: " + shields + " | Health outake: " + (intake / (shields + 1)));
        EvaluateShields();
        playerColor = healthBarGradient.Evaluate(healthT);
        playerHitGradient = ChangeGradientColor(playerHitGradient, playerColor);
        if (stats.numericals["health"] <= 0)
        {
            //RecycleBacteria(); 
            //death(insane)
        }
        if (stats.numericals["health"] >= 0f)
            return 0f;
        else
            return stats.numericals["health"];
    }

    public void TakeInjector(Injector injector, bool cacheInstances)
    {
        if (injector.type == injectorType.ENEMIES)
            return;
        if (stats.numericals["health"] <= 0f)
            return;
        foreach (var bac in injector.allyBacterias)
        {
            if (Random.Range(0f,100f) > injector.chance)
                continue;
            if (cacheInstances){
                Bacteria instancedBac;
                if (immuneSystem.bacterias.ContainsKey(bac.name.Replace("_ALLY",""))){
                    instancedBac = immuneSystem.bacterias[bac.name.Replace("_ALLY","")];
                    immuneSystem.bacterias[bac.name.Replace("_ALLY","")].BacteriaIn();
                }else{
                    instancedBac= PublicPools.pools[bac.name.Replace("_ALLY","")].SendObject(gameObject).GetComponent<Bacteria>();
                }
                instancedBac.injectorCachedFrom = injector;
                injector.cachedInstances.Add(instancedBac);
            }
            else{
                if (immuneSystem.bacterias.ContainsKey(bac.name.Replace("_ALLY",""))){
                    immuneSystem.bacterias[bac.name.Replace("_ALLY","")].BacteriaIn();
                }else{
                    PublicPools.pools[bac.name.Replace("_ALLY","")].SendObject(gameObject);
                }
            }
            Debug.Log(bac.gameObject.name);
        }
    }

    public void RevertInjector(Injector injector){
        foreach (var bac in injector.cachedInstances.ToArray())
        {
            if (immuneSystem.bacterias.ContainsValue(bac))
                immuneSystem.bacterias[bac.name].Instagib();
        }
    }
}