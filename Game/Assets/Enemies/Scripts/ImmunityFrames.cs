using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunityFrames : MonoBehaviour
{
    [SerializeField] private float immunityStart;
    private Collider coll;
    private float time;

    private void Start()
    {
        coll = GetComponent<Collider>();
    }

    private void Update()
    {
        if (time <= immunityStart)
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
