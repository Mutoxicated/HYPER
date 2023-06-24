using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunityFrames : MonoBehaviour
{
    [SerializeField] private int immunityStart;
    private Collider coll;
    private int time;

    private void Start()
    {
        coll = GetComponent<Collider>();
    }

    private void Update()
    {
        if (time <= immunityStart)
        {
            coll.enabled = false;
            time++;
        }
        else
        {
            coll.enabled = true;
        }
    }
}
