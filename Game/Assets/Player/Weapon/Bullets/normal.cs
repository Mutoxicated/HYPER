using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class normal : MonoBehaviour
{
    private static GunShooter gun;

    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private float lifetime;
    [SerializeField] private float damage;
    private float time;

    private void Awake()
    {
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
        Instantiate(particlePrefab, transform.position, transform.rotation);
        time = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Recycle();
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage * gun.stats.incrementalStat["damage"][0], gameObject);
    }
}
