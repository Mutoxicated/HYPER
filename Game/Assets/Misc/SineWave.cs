using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SineWave : MonoBehaviour
{
    [SerializeField] private float interval;
    private float rate;
    private float angle = 0f;
    [HideInInspector]
    public float t;

    void Awake()
    {
        rate = Mathf.PI / (interval*Mathf.PI);
    }

    void Update()
    {
        angle += rate * Time.deltaTime;
        t = Mathf.Sin(angle);
        if (angle >= Mathf.PI)
        {
            angle = 0f;
        }
    }
}
