using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnInterval : MonoBehaviour
{
    [SerializeField] private float interval;
    [SerializeField] private UnityEvent onInterval;
    [SerializeField] private bool destroyUponEvent;

    [HideInInspector] public float t;
    private float time;

    private void Update()
    {
        time += Time.deltaTime;
        if (time >= interval)
        {
            time = 0f;
            onInterval.Invoke();
            if (destroyUponEvent)
                Destroy(gameObject);
        }
        if (time != 0f)
            t = time / interval;
        else t = 0f;
    }
}
