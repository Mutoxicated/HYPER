using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private LayerMask layerMask;   
    [SerializeField] private int damage;
    [SerializeField] private Injector injector;
    [SerializeField] private OnInterval interval;
    [SerializeField] private float maxWidth;
    [SerializeField] private GameObject objToIgnore;

    private RaycastHit[] hits = new RaycastHit[5];
    private float distance = 100f;
    private float pierces;
    private IDamageable cachedDamageable;

    private Vector3 scale;

    private RaycastHit[] SortRaycasts(RaycastHit[] hits, int hitAmount)
    {
        RaycastHit temp;
        RaycastHit[] sortedHits = hits;

        for (int i = 0; i <= hitAmount - 1; i++)
        {
            for (int j = i + 1; j < hitAmount; j++)
            {
                if (sortedHits[i].distance > sortedHits[j].distance)
                {
                    temp = sortedHits[i];
                    sortedHits[i] = sortedHits[j];
                    sortedHits[j] = temp;
                }
            }
        }
        return sortedHits;
    }

    private void OnEnable()
    {
        pierces = stats.numericals["pierces"];
        int hitAmount = Physics.SphereCastNonAlloc(transform.position, maxWidth*0.5f, transform.TransformDirection(Vector3.forward), hits, 100f, layerMask.value, QueryTriggerInteraction.UseGlobal);
        hits = SortRaycasts(hits, hitAmount);
        for (int i = 0; i < hitAmount; i++)
        {
            if (objToIgnore == hits[i].collider.gameObject)
                continue;
            distance = hits[i].distance;
            //Debug.Log(hits[i].collider.gameObject.name + " | " + i);
            cachedDamageable = hits[i].transform.gameObject.GetComponent<IDamageable>();
            cachedDamageable?.TakeDamage(damage, gameObject,1f,0);
            cachedDamageable?.TakeInjector(injector);
            if (pierces == 0f)
                break;
            pierces -= 1;
        }
        scale = transform.localScale;
        scale.z = distance;
        transform.localScale = scale;
    }

    private void Update()
    {
        if (interval.isPlaying){
            scale.x = scale.y = Mathf.Lerp(maxWidth,0f,interval.t);
            transform.localScale = scale;
        }
    }
}
