using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private int shootInterval;
    [SerializeField] private GameObject bulletPrefab;
    private float time;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > shootInterval)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
            time = 0;
        }
    }
}
