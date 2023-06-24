using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private int shootInterval;
    [SerializeField] private GameObject bulletPrefab;
    private int time;

    private void FixedUpdate()
    {
        time++;
        if (time > shootInterval)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
            time = 0;
        }
    }
}
