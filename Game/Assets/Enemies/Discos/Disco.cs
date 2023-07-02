using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Disco : MonoBehaviour
{
    [SerializeField] private float shootInterval;
    [SerializeField] private float speedSlowdown;
    [SerializeField] private float laser;

    private float t = 0.0001f;
    private float alphaT;
    private GameObject player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    private void Update()
    {
        t += Time.deltaTime;
        alphaT = t/shootInterval;
        if (t > shootInterval)
        {
            t = 0.0001f;

        }
    }
}
