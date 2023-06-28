using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class normal : MonoBehaviour, IPassInfo
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private GameObject particlePrefab;
    [SerializeField] private int lifetime;
    [SerializeField] private int damage;
    private int weaponType;
    private float time;

    public void PassInfo(object[] info)
    {
        weaponType = (int)info[0]+1;
        damage = damage/weaponType;//damage is equally shared with all of the bullets
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
        collision.gameObject.GetComponent<IDamagebale>()?.TakeDamage(damage, gameObject);
    }

    private void OnCollisionStay(Collision collision)
    {
        Recycle();
    }
}
