using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private float shootInterval;
    [SerializeField] private GameObject bulletPrefab;

    public void ShootPrefab()
    {
        Instantiate(bulletPrefab, transform.position, transform.rotation);
    }
}
