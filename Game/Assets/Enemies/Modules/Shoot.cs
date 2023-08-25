using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private bool usePool;
    [SerializeField] private GameObject bulletPrefab;

    public void ShootPrefab()
    {
        if (usePool)
        {
            PublicPools.pools[bulletPrefab.name].UseObject(transform.position,transform.rotation);
        }
        else
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }
}
