using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] private Immunity immuneSystem;
    [SerializeField] private Collider coll;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private float lifetime;
    [SerializeField] private float damage;
    private float time;
    private float pierces;

    private Injector injector;
    private void Awake()
    {
        injector = GetComponent<Injector>();
        damage = damage / (PlayerInfo.GetGun().GetWeaponTypeInt() + 1);//damage is equally shared with all of the bullets
        speed = speed / (PlayerInfo.GetGun().GetWeaponTypeInt() + 1);
    }

    private void OnEnable()
    {
        rb.velocity = transform.forward * speed;
        pierces = PlayerInfo.GetGun().stats.numericals["pierces"];
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= lifetime)
        {
            immuneSystem?.RecycleBacteria();
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11){
            return;
        }
        if (other.gameObject.layer != 8)
        {
            if (other.gameObject.layer != 9){
                Recycle();
                return;
            }
        }
        //Debug.Log(other.gameObject.name);
        var damageable = other.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(damage * PlayerInfo.GetGun().stats.numericals["damage"], PlayerInfo.GetGun().stats,1f,0);
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
        if (other.gameObject.layer == 11){
            return;
        }
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
        if (other.gameObject.layer == 11){
            return;
        }
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
