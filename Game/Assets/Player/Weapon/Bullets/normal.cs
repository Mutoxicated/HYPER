using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class normal : MonoBehaviour
{
    private static GunShooter gun;

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
        if (gun == null)
        {
            gun = GameObject.FindWithTag("Gun").GetComponent<GunShooter>();
        }
        damage = damage / (gun.GetWeaponTypeInt() + 1);//damage is equally shared with all of the bullets
        speed = speed / (gun.GetWeaponTypeInt() + 1);
    }

    private void OnEnable()
    {
        rb.velocity = transform.forward * speed;
        pierces = gun.stats.incrementalStat["pierces"][0];
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= lifetime)
        {
            gameObject.SetActive(false);
            time = 0;
        }
    }

    private void Recycle()
    {
        gameObject.SetActive(false);
        PublicPools.pools[particlePrefab.name].UseObject(transform.position, transform.rotation);
        time = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            Recycle();
            return;
        }
        var damageable = other.gameObject.GetComponent<IDamageable>();
        damageable?.TakeDamage(damage * gun.stats.incrementalStat["damage"][0], gameObject,1f,0);
        if (injector != null)
        {
            damageable?.TakeInjector(injector);
        }
        if (pierces == 0)
            Recycle();
        pierces -= 1;
        //Debug.Log("ENTER");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            Recycle();
            return;
        }
        //Debug.Log("STAY");
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag != "Enemy")
        {
            Recycle();
            return;
        }
        //Debug.Log("EXIT");
    }
}
