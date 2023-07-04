using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LayerMask layerMask;   
    [SerializeField] private float duration;
    [SerializeField] private int damage;
    [SerializeField] private LineRenderer line;
    private float t = 1f;
    private float deathRate;

    private void Start()
    {
        deathRate = t / duration;
        var hits = Physics.SphereCastAll(transform.position, 0.5f, transform.TransformDirection(Vector3.forward), 100f, layerMask);
        foreach (var hit in hits)
        {
            Debug.Log(hit.transform.gameObject.name);
            hit.transform.gameObject.GetComponent<IDamagebale>()?.TakeDamage(damage, gameObject);
        }
    }

    private void Update()
    {
        t -= deathRate * Time.deltaTime;
        line.widthMultiplier = t;
        if (t <= 0.04f)
        {
            Destroy(gameObject);
        }
    }
}
