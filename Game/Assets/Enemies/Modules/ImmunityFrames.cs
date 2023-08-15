using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunityFrames : MonoBehaviour
{
    [SerializeField] private float immunityDuration;
    private Collider coll;
    private float time;

    private void Awake()
    {
        coll = GetComponent<Collider>();
        coll.enabled = false;
    }

    private void Update()
    {
        if (time <= immunityDuration)
        {
            coll.enabled = false;
            time += Time.deltaTime;
        }
        else
        {
            coll.enabled = true;
        }
    }
}
