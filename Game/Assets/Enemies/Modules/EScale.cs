using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EScale : MonoBehaviour
{
    [SerializeField] private bool randomScale = true;
    [SerializeField] private float interval;
    [SerializeField] private float minMultiplier;
    [SerializeField] private float maxMultiplier;
    [SerializeField] private float lerpSpeed;

    private float multiplier;
    private Vector3 currentScale;
    private float t;

    void Start()
    {
        currentScale = transform.localScale;
    }

    void Update()
    {
        t += Time.deltaTime;
        if (randomScale)
        {
            if (t > interval)
            {
                t = 0;
                multiplier = Random.Range(minMultiplier, maxMultiplier);
            }
            transform.localScale = Vector3.Lerp(transform.localScale, currentScale*multiplier, Time.deltaTime * lerpSpeed);
        }
        else
        {

        }
    }
}
