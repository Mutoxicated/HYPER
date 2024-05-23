using UnityEngine;
using UnityEngine.Events;
using static Numerical;
using static Conditional;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public Stats stats;
    public Immunity immuneSystem;
    [SerializeField] private ScorePopupCanvas spc;
    [Header("Post-Death")]
    public UnityEvent<Transform> OnDeath = new UnityEvent<Transform>();
    [SerializeField] private GameObject[] ObjectsToDestroy;
    [SerializeField] private GameObject[] ChildrenToDetach;
    [Space]
    [Header("General")]
    [SerializeField] private float regen;
    [SerializeField] private float immunityDuration;
    [SerializeField,Range(0.5f,2f)] private float rate = 0.05f;
    [SerializeField] private int hitScore;
    [SerializeField] private int deathScore;

    [SerializeField] private HealthBar healthBar;

    [HideInInspector] public float t = 0f;
    [HideInInspector] public bool immune = false;
    private float immunityTime;
    private float time;

    private void OnEnable(){
        Difficulty.enemies.Add(gameObject);
        immunityTime = 0f;
        t = 0f;
        immune = true;
    }

    private void Update()
    {
        if (Difficulty.roundFinished){
            Die(null);
            return;
        }
        if (immunityTime < immunityDuration)
        {
            immunityTime += Time.deltaTime;
            immune = true;
        }
        else
        {
            immune = false;
        }
        time += Time.deltaTime;
        if (time >= 0.1f){
            time = 0f;
            stats.numericals[HEALTH] += regen;
        }
        stats.numericals[HEALTH] = Mathf.Clamp(stats.numericals[HEALTH],0,stats.maxHealth* stats.numericals[MAX_HEALTH_MODIFIER]);
        if (stats.numericals[HEALTH] < 0)
            stats.numericals[HEALTH] = 0f;
        t = Mathf.Clamp01(t - rate*Time.deltaTime);
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
        if (spc != null){
            spc.transform.SetParent(null,false);
            spc.transform.position = transform.position;
            spc.transform.rotation = transform.rotation;
            spc.Die();
        }
        if (ChildrenToDetach == null)
            return;
        foreach (var obj in ChildrenToDetach)
        {
            obj.transform.parent = null;
        }
    }

    private void Parry(GameObject sender){
        if (Random.Range(0f,100f) <= stats.numericals[PARRY_CHANCE]){
            sender.GetComponent<IParriable>()?.Parry(gameObject,Vector3.zero);
        }
    }

    private bool ChanceFailed(Numerical name){
        return Random.Range(0f,101f) >= Mathf.Lerp(0f,100f,stats.numericals[name]);
    }

    private bool EvaluateDamageIntake(Stats senderStats, float intake){
        if (senderStats == null){
            if (ChanceFailed(BACTERIA_BLOCK_CHANCE))
                stats.numericals[HEALTH] -= intake / (stats.shields.Count+Mathf.RoundToInt(stats.numericals[PERMA_SHIELDS]) + 1);
            else {
                Debug.Log("blocked");
                return false;
            }
            return true;
        }else if (senderStats == stats){
            stats.numericals[HEALTH] -= intake / (stats.shields.Count+Mathf.RoundToInt(stats.numericals[PERMA_SHIELDS]) + 1);
            return true;
        }
        if (stats.type == EntityType.ORGANIC){
            intake *= senderStats.numericals[DAMAGE_O];
        }else{
            intake *= senderStats.numericals[DAMAGE_NO];
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

    private void OnDisable(){
        Difficulty.enemies.Remove(gameObject);
    }

    private void PopScore(int score, float duration){
        if (spc != null)
            spc.PopScore(score, duration);
    }

    private void ScoreOnHit(Stats senderStats){
        if (senderStats.gameObject.tag == "Player" | senderStats.gameObject.tag == "bullets"){
            PlayerInfo.AddScore(hitScore);
            PopScore(hitScore,0.5f);
        }
    }

    public void Die(Stats senderStats){
        if (stats.conditionals[EXPLOSIVE] && Random.Range(0f,100f) <= stats.numericals[EXPLOSION_CHANCE]){
            PublicPools.pools[stats.explosionPrefab.name].UseObject(transform.position,Quaternion.identity);
        }
        Detach();
        if (senderStats == null){
            OnDeath.Invoke(transform);
        }else{
            if (senderStats.gameObject.tag == "bullets") {
                PlayerEvents.EnemyBulletKill.Invoke(transform.position, transform.rotation);
            }
            if (senderStats.gameObject.tag == "Player" | senderStats.gameObject.tag == "bullets"){
                PlayerInfo.AddScore(deathScore);
                PopScore(deathScore,1.2f);
            }
            OnDeath.Invoke(senderStats.transform);
        }
        OnDeath.RemoveAllListeners();
        DestroyStuff();
    }

    public void TakeHealth(float intake, int shield){
        stats.numericals[HEALTH] += intake;
        stats.AddShield(shield);
    }

    public float TakeDamage(float intake, Stats senderStats, ref int shieldOut, float strength, int _)
    {
        if (immune)
            return 0f;
        if (!EvaluateDamageIntake(senderStats,intake)){
            return 0f;
        }
        shieldOut = 0;
        if (senderStats != null){
            Parry(senderStats.gameObject);
            ScoreOnHit(senderStats);
        }
        t += strength;
        healthBar?.Activate();
        if (stats.numericals[HEALTH] <= 0)
        {
            Die(senderStats);
        }
        if (stats.numericals[HEALTH] >= 0f)
            return 0f;
        else
            return stats.numericals[HEALTH];
    }
    
    public float TakeDamage(float intake, Stats senderStats, float strength, int _)
    {
        if (immune)
            return 0f;
        if (!EvaluateDamageIntake(senderStats,intake)){
            return 0f;
        }
        if (senderStats != null){
            Parry(senderStats.gameObject);
            ScoreOnHit(senderStats);
        }
        t += strength;
        healthBar?.Activate();
        if (stats.numericals[HEALTH] <= 0)
        {
            Die(senderStats);
        }
        if (stats.numericals[HEALTH] >= 0f)
            return 0f;
        else
            return stats.numericals[HEALTH];
    }

    public float TakeDamage(float intake, float strength, int _)
    {
        if (immune)
            return 0f;
        if (stats.shields.Count > 0){
            if (stats.shields[stats.shields.Count-1].TakeDamage(intake) <= 0f)
                stats.shields.RemoveAt(stats.shields.Count-1);
        }
        stats.numericals[HEALTH] -= intake / (stats.shields.Count+Mathf.RoundToInt(stats.numericals[PERMA_SHIELDS]) + 1);
        t += strength;
        healthBar?.Activate();
        if (stats.numericals[HEALTH] <= 0)
        {
            Die(null);
        }
        if (stats.numericals[HEALTH] >= 0f)
            return 0f;
        else
            return stats.numericals[HEALTH];
    }

    public void TakeInjector(Injector injector, bool cacheInstances)
    {
        //Debug.Log("acknowledged.");
        if (injector.type == injectorType.PLAYER)
            return;
        //Debug.Log("injector was fine.");
        if (stats.numericals[HEALTH] <= 0)
            return;
        //Debug.Log("health was fine.");
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
}
