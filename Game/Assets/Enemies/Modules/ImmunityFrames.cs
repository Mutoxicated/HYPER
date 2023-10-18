using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunityFrames : MonoBehaviour
{
    [SerializeField] private float immunityDuration;
    private Collider coll;
    private float time;

    private void OnEnable()
    {
        if (coll == null)
            coll = GetComponent<Collider>();
        coll.enabled = false;
        time = 0f;
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
