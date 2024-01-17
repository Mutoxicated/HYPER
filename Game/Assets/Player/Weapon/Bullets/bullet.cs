using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using RotaryHeart.Lib.SerializableDictionary;

public class bullet : MonoBehaviour
{
    [SerializeField] private Stats bStats;
    [SerializeField] private Immunity immuneSystem;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private float lifetime;
    [SerializeField] private float damage;
    [SerializeField] private float pierceModifier = 1f;
    private float time;
    private float pierces;

    private Vector3 initialScale = Vector3.zero;

    private Injector injector;
    private void Awake()
    {
        injector = GetComponent<Injector>();
        damage = damage / (PlayerInfo.GetGun().GetWeaponTypeInt() + 1);//damage is equally shared with all of the bullets
        bStats.numericals["moveSpeed"] = bStats.numericals["moveSpeed"] / ((PlayerInfo.GetGun().GetWeaponTypeInt() + 1)*0.5f);
    }

    private void OnEnable()
    {
        if (initialScale == Vector3.zero){
            initialScale = transform.localScale;
        }
        transform.localScale = initialScale*PlayerInfo.GetGun().stats.numericals["size"];
        pierces = PlayerInfo.GetGun().stats.numericals["pierces"];
        pierces = Mathf.RoundToInt(pierces*pierceModifier);
        rb.velocity = transform.forward * speed * PlayerInfo.GetGun().stats.numericals["moveSpeed"]*bStats.numericals["moveSpeed"];
        //Debug.Log(pierces);
        //Debug.Log(bStats.numericals["range"]);
    }

    private void Update()
    {
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
        damageable?.TakeDamage(damage * PlayerInfo.GetGun().stats.numericals["damage"], bStats,1f,0);
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
