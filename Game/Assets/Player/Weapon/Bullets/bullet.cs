using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class bullet : MonoBehaviour
{
    public static float bulletEffectiveness = 1f;

    [SerializeField] private Stats bStats;
    [SerializeField] private Immunity immuneSystem;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private float lifetime;
    [SerializeField] private float damage;
    [SerializeField] private float pierceModifier = 1f;
    [SerializeField] private OnInterval interval;
    private float time;
    private float pierces;

    private Vector3 initialScale = Vector3.zero;

    private Injector injector;

    public static void ResetBulletEffectiveness(){
        bulletEffectiveness = 1f;
    }

    private void Awake()
    {
        injector = GetComponent<Injector>();
        damage = damage / (PlayerInfo.GetGun().GetWeaponTypeInt() + 1);//damage is equally shared with all of the bullets
        bStats.numericals["moveSpeed"] = bStats.numericals["moveSpeed"] / ((PlayerInfo.GetGun().GetWeaponTypeInt() + 1)*0.5f);
    }

    private float ClampedT(){
        return Mathf.Clamp(interval.t,0.6f,1f);
    }

    private float CombinedStat(string name){
        return PlayerInfo.GetGun().stats.numericals[name]+bStats.numericals[name]-1f;
    }

    private void OnEnable()
    {
        injector.InheritInjector(PlayerInfo.GetPH().immuneSystem.injector);
        injector.injectorToInheritFrom = PlayerInfo.GetPH().immuneSystem.injector;
        interval.enabled = true;
        if (initialScale == Vector3.zero){
            initialScale = transform.localScale;
        }
        transform.localScale = initialScale*CombinedStat("size")*ClampedT();
        pierces = CombinedStat("pierces");
        pierces = Mathf.RoundToInt(pierces*pierceModifier);
        rb.velocity = transform.forward * speed * CombinedStat("moveSpeed");
    }

    private void Update()
    {
        transform.localScale = initialScale*CombinedStat("size")*ClampedT();
        time += Time.deltaTime;
        if (time >= lifetime)
        {
            immuneSystem.RecycleBacteria();
            gameObject.SetActive(false);
            PublicPools.pools[gameObject.name].Reattach(gameObject);
            time = 0;
        }
    }

    private void Recycle()
    {
        immuneSystem?.RecycleBacteria();
        gameObject.SetActive(false);
        PublicPools.pools[particlePrefab.name].UseObject(transform.position, transform.rotation);
        PublicPools.pools[gameObject.name].Reattach(gameObject);
        time = 0;
    }

    private bool LogicStop(Collider other){
        if (other.gameObject.layer == LayerMask.NameToLayer("IgnoreAllBullets")){
            return true;
        }
        if (other.gameObject.tag == "Player")
            return true;
        if (other.gameObject.layer == LayerMask.NameToLayer("explosions"))
            return true;
        return false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (LogicStop(other))
            return;
        if (other.gameObject.layer != 8)
        {
            if (other.gameObject.layer != 9){
                Recycle();
                return;
            }
        }
        //Debug.Log(other.gameObject.name);
        var damageable = other.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(damage * CombinedStat("damage"), bStats,1f,0);
        if (injector != null)
        {
            damageable?.TakeInjector(injector,false);
        }
        if (pierces == 0)
            Recycle();
        PublicPools.pools[particlePrefab.name].UseObject(transform.position, transform.rotation);
        pierces -= 1;
        //Debug.Log("ENTER");
    }

    private void OnTriggerStay(Collider other)
    {
        if (LogicStop(other))
            return;
        if (other.gameObject.layer != 8)
        {
            if (other.gameObject.layer != 9){
                Recycle();
                return;
            }
        }
        //Debug.Log("STAY");
    }

    private void OnTriggerExit(Collider other)
    {
        if (LogicStop(other))
            return;
        if (other.gameObject.layer != 8)
        {
            if (other.gameObject.layer != 9){
                Recycle();
                return;
            }
        }
        //Debug.Log("EXIT");
    }
}
