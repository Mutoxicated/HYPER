using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField] private bool usePool;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Injector injector;

    public void ShootPrefab()
    {
        if (usePool)
        {
            PublicPools.pools[bulletPrefab.name].UseObject(transform.position,transform.rotation);
        }
        else
        {
            var instance = Instantiate(bulletPrefab, transform.position, transform.rotation);
            if (injector != null & injector.bacteriaPools.Count > 0){
                foreach (string bac in injector.bacteriaPools){
                    PublicPools.pools[bac+"_ALLY"].SendObject(instance);
                }
            }
        }
    }
}
