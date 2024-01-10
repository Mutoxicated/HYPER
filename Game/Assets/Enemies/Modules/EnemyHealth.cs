using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    public Stats stats;
    public Immunity immuneSystem;
    [Header("Post-Death")]
    public UnityEvent<Transform> OnDeath = new UnityEvent<Transform>();
    [SerializeField] private GameObject[] ObjectsToDestroy;
    [SerializeField] private GameObject[] ChildrenToDetach;
    [Space]
    [Header("General")]
    [SerializeField] private float immunityDuration;
    [SerializeField,Range(0.5f,2f)] private float rate = 0.05f;
    [SerializeField] private float hitScore;
    [SerializeField] private float deathScore;

    [SerializeField] private HealthBar healthBar;

    [HideInInspector] public float t = 0f;
    [HideInInspector] public bool immune = false;
    private float immunityTime;

    private void OnEnable(){
        Difficulty.enemies.Add(gameObject);
        immunityTime = 0f;
        t = 0f;
        immune = true;
    }

    private void Update()
    {
        if (immunityTime < immunityDuration)
        {
            immunityTime += Time.deltaTime;
            immune = true;
        }
        else
        {
            immune = false;
        }
        stats.numericals["health"] = Mathf.Clamp(stats.numericals["health"],0,stats.maxHealth* stats.numericals["maxHealthModifier"]);
        if (stats.numericals["health"] < 0)
            stats.numericals["health"] = 0f;
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
        if (ChildrenToDetach == null)
            return;
        foreach (var obj in ChildrenToDetach)
        {
            obj.transform.parent = null;
        }
    }

    private void Parry(GameObject sender){
        if (Random.Range(0f,100f) <= stats.numericals["parryChance"]){
            sender.GetComponent<IParriable>()?.Parry(gameObject,Vector3.zero);
        }
    }

    private bool EvaluateDamageIntake(Stats senderStats, float intake){
        if (senderStats == null){
            if (Random.Range(0f,100f) > stats.numericals["bacteriaBlockChance"])
                stats.numericals["health"] -= intake / (stats.shields.Count+stats.numericals["permaShields"] + 1);
            else {
                Debug.Log("blocked");
                return false;
            }
            return true;
        }else if (senderStats == stats){
            stats.numericals["health"] -= intake / (stats.shields.Count+stats.numericals["permaShields"] + 1);
            return true;
        }
        if (stats.type == EntityType.ORGANIC){
            intake*= senderStats.numericals["damageO"];
        }else{
            intake*= senderStats.numericals["damageNO"];
        }
        if (Random.Range(0f,100f) > stats.numericals["enemyBlockChance"]){
            stats.numericals["health"] -= intake / (stats.shields.Count+stats.numericals["permaShields"] + 1);
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

    private void OnDestroy(){
        OnDeath.RemoveAllListeners();
    }

    private void ScoreOnHit(Stats senderStats){
        if (senderStats.gameObject.tag == "Player" | senderStats.inheritFromPlayer)
            PlayerInfo.AddScore(hitScore);
    }

    public void Die(Stats senderStats){
        if (stats.conditionals["explosive"] && Random.Range(0f,100f) <= stats.numericals["explosionChance"]){
                PublicPools.pools[stats.explosionPrefab.name].UseObject(transform.position,Quaternion.identity);
            }
        Detach();
        if (senderStats == null){
            OnDeath.Invoke(transform);
        }else{
            if (senderStats.gameObject.tag == "Player" | senderStats.inheritFromPlayer)
                PlayerInfo.AddScore(deathScore);
            OnDeath.Invoke(senderStats.transform);
        }
        DestroyStuff();
    }

    public void TakeHealth(float intake, int shield){
        stats.numericals["health"] += intake;
        stats.AddShield(shield);
    }

    public float TakeDamage(float intake, Stats senderStats, ref int shieldOut, float strength, int _)
    {
        if (!EvaluateDamageIntake(senderStats,intake)){
            return 0f;
        }
        if (immune)
            return 0f;
        
        shieldOut = 0;
        if (senderStats != null){
            Parry(senderStats.gameObject);
            ScoreOnHit(senderStats);
        }
        t += strength;
        healthBar?.Activate();
        if (stats.numericals["health"] <= 0)
        {
            Die(senderStats);
        }
        if (stats.numericals["health"] >= 0f)
            return 0f;
        else
            return stats.numericals["health"];
    }
    
    public float TakeDamage(float intake, Stats senderStats, float strength, int _)
    {
        if (!EvaluateDamageIntake(senderStats,intake)){
            return 0f;
        }
        if (immune)
            return 0f;
        if (senderStats != null){
            Parry(senderStats.gameObject);
            ScoreOnHit(senderStats);
        }
        t += strength;
        healthBar?.Activate();
        if (stats.numericals["health"] <= 0)
        {
            Die(senderStats);
        }
        if (stats.numericals["health"] >= 0f)
            return 0f;
        else
            return stats.numericals["health"];
    }

    public void TakeInjector(Injector injector, bool cacheInstances)
    {
        //Debug.Log("acknowledged.");
        if (injector.type == injectorType.PLAYER)
            return;
        //Debug.Log("injector was fine.");
        if (stats.numericals["health"] <= 0)
            return;
        //Debug.Log("health was fine.");
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
