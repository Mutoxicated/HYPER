using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int HP;
    [SerializeField] private Gradient colorHit;
    [SerializeField] private ExplosionParts explosion;
    [SerializeField] private GameObject trail;
    private Material matInstance;
    private float t = 0f;
    private int currentHP;

    private void Start()
    {
        rb.velocity = transform.forward * speed;
        currentHP = HP;
        matInstance = GetComponent<Renderer>().material;
        matInstance.SetColor("_Color", colorHit.Evaluate(t));
    }

    private void Update()
    {
        t = Mathf.Clamp01(t - 0.05f);
        if (t != 0)
        {
            matInstance.SetColor("_Color", colorHit.Evaluate(t));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        t = 1f;
        if (collision.gameObject.tag != "bullets")
        {
            Destroy(gameObject);
            return;
        }
        currentHP--;
        if (currentHP <= 0)
        {
            if (explosion != null)
            {
                Destroy(trail);
                transform.DetachChildren();
                Destroy(gameObject);
                explosion.ExplodeParts(collision.contacts[0].point);
            }
        }
    }
}
