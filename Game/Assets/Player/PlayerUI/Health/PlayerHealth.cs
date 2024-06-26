using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static Numerical;


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
    [Header("World Objects")]
    [SerializeField] private GameObject[] playerObjects;
    [Header("General")]
    [SerializeField] private UnityEvent OnDeath = new UnityEvent();
    [Header("UI")]
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
    [SerializeField] private TMP_Text info;
    [SerializeField] private bool infoIsScore;
    [SerializeField, Range(0.5f, 3f)] private float reactionSpeed = 0.05f;
    [SerializeField] private HitReaction hitReaction;
    [Header("Misc")]
    [SerializeField] private float regen = 0.1f;

    private List<MaterialInfo> objs = new List<MaterialInfo>();
    private float reactionT = 0f;
    private float healthT = 1f;
    private Vector3 initialScale;
    private Color playerColor;
    private Color healthBarBackColor;

    private float currentT;
    private TMP_Text shieldsParent;
    private float time;

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

    private void Awake(){
        PlayerInfo.SetPlayer(gameObject);
        PlayerInfo.SetPH(this);
    }

    private void Start()//Player colors dont update on scene switch
    {
        initialScale = healthBar.localScale;
        healthT = stats.numericals[HEALTH] / stats.maxHealth*stats.numericals[MAX_HEALTH_MODIFIER];
        currentT = healthT;
        sumText.text = Mathf.Round(healthT*100f).ToString() + '%';
        healthBar.localScale = new Vector3(Mathf.Clamp(initialScale.x * healthT,0.0002f,initialScale.x),healthBar.localScale.y, healthBar.localScale.z);
        healthBarImg = healthBar.GetComponent<Image>();
        playerColor = healthBarGradient.Evaluate(healthT);
        playerHitGradient = ChangeGradientColor(playerHitGradient, playerColor);
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
        time += Time.deltaTime;
        if (time >= 0.1f){
            time = 0f;
            stats.numericals[HEALTH] += regen;
        }
        if (infoIsScore){
            info.text = PlayerInfo.GetScore().ToString();
        }else{
            info.text = PlayerInfo.GetMoney().ToString()+"*";
        }
        stats.numericals[HEALTH] = Mathf.Clamp(stats.numericals[HEALTH],0,stats.maxHealth*stats.numericals[MAX_HEALTH_MODIFIER]);
        if (stats.numericals[HEALTH] <= 0f){
            stats.numericals[HEALTH] = 0f;
            healthT = 0f;
        } else healthT = stats.numericals[HEALTH] / stats.maxHealth*stats.numericals[MAX_HEALTH_MODIFIER];
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
        float allShields = stats.shields.Count+Mathf.RoundToInt(stats.numericals[PERMA_SHIELDS]);
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

    private bool ChanceFailed(Numerical name){
        return Random.Range(0f,101f) >= Mathf.Lerp(0f,100f,stats.numericals[name]);
    }

    private bool EvaluateDamageIntake(Stats senderStats, float intake){
        if (senderStats == null){//if stat is null that means its a bacteria
            if (ChanceFailed(BACTERIA_BLOCK_CHANCE)) {
                stats.numericals[HEALTH] -= intake / Mathf.Clamp(Mathf.FloorToInt((stats.shields.Count+Mathf.RoundToInt(stats.numericals[PERMA_SHIELDS]) + 1)*0.5f),1f,999999999f);
            }
            else return false;
            return true;
        }
        if (stats.type == EntityType.ORGANIC){
            intake*= senderStats.numericals[DAMAGE_O];
        }else{
            intake*= senderStats.numericals[DAMAGE_NO];
        }

        if (ChanceFailed(ENEMY_BLOCK_CHANCE)){
            stats.numericals[HEALTH] -= intake / (stats.shields.Count+Mathf.RoundToInt(stats.numericals[PERMA_SHIELDS]) + 1);
            if (stats.shields.Count > 0){
                if (stats.shields[stats.shields.Count-1].TakeDamage(intake) <= 0f)
                    stats.shields.RemoveAt(stats.shields.Count-1);
            }
        }
        else return false;
        return true;
    }

    public void TakeHealth(float intake, int shield){
        stats.numericals[HEALTH] += intake;
        stats.AddShield(shield);
    }

    public float TakeDamage(float intake, Stats senderStats, ref int shieldOut, float arb, int index)
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
        if (stats.numericals[HEALTH] <= 0)
        {
            OnDeath.Invoke();
        }
        if (stats.numericals[HEALTH] >= 0f)
            return 0f;
        else
            return stats.numericals[HEALTH];
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
        if (stats.numericals[HEALTH] <= 0)
        {
            OnDeath.Invoke();
        }
        if (stats.numericals[HEALTH] >= 0f)
            return 0f;
        else
            return stats.numericals[HEALTH];
    }

    public float TakeDamage(float intake, float arb, int index)
    {
        stats.numericals[HEALTH] -= intake / (stats.shields.Count+Mathf.RoundToInt(stats.numericals[PERMA_SHIELDS]) + 1);
        if (stats.shields.Count > 0){
            if (stats.shields[stats.shields.Count-1].TakeDamage(intake) <= 0f)
                stats.shields.RemoveAt(stats.shields.Count-1);
        }
        reactionT = arb;
        hurtScreen.SetActive(true);
        fadeColor.ChangeGradientIndex(index);
        //Debug.Log("Shields: " + shields + " | Health outake: " + (intake / (shields + 1)));
        EvaluateShields();
        playerColor = healthBarGradient.Evaluate(healthT);
        playerHitGradient = ChangeGradientColor(playerHitGradient, playerColor);
        if (stats.numericals[HEALTH] <= 0)
        {
            OnDeath.Invoke();
        }
        if (stats.numericals[HEALTH] >= 0f)
            return 0f;
        else
            return stats.numericals[HEALTH];
    }

    public void TakeInjector(Injector injector, bool cacheInstances)
    {
        if (injector.type == injectorType.ENEMIES)
            return;
        if (stats.numericals[HEALTH] <= 0f)
            return;
        foreach (var bac in injector.allyBacterias)
        {
            if (Random.Range(0f,100f) > injector.chance)
                continue;
            if (cacheInstances){
                Bacteria instancedBac;
                instancedBac = immuneSystem.AddBacteria(bac.name.Replace("_ALLY",""));
                instancedBac.injectorCachedFrom = injector;
                injector.cachedInstances.Add(instancedBac);
            }
            else{
                immuneSystem.AddBacteria(bac.name.Replace("_ALLY",""));
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

    public void ActivateScreen(int index){
        hurtScreen.SetActive(true);
        fadeColor.ChangeGradientIndex(index);
    }

    public void ActivateScreen(){
        hurtScreen.SetActive(true);
    }
}