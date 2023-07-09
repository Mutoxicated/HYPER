using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float shootInterval;
    [SerializeField] private GameObject bulletPrefab;
    private float time;

    public void ShootPrefab()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }

    private void Update()
    {
        if (shootInterval < 0)
            return;
        time += Time.deltaTime;
        if (time > shootInterval)
        {
            ShootPrefab();
            time = 0;
        }
    }
}
