using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField, Tooltip("If you use this, don't use bulletPrefab.")] private ObjectPoolManager objectPool;
    [SerializeField] private bool inheritInjector;
    [SerializeField, Tooltip("Used when no object pool is used.")] private GameObject bulletPrefab;
    [SerializeField] private Injector injector;

    private bool instantiated;
    private GameObject instance;

    public void ShootPrefab()
    {
        if (objectPool != null)
        {
            instance = objectPool.UseObject(transform.position,transform.rotation, out instantiated);
            if (instantiated)
                instance.GetComponent<Injector>()?.InheritInjector(injector);
        }
        else
        {
            instance = Instantiate(bulletPrefab, transform.position, transform.rotation);
            instance.SetActive(true);
            if (inheritInjector){
                instance.GetComponent<Injector>()?.InheritInjector(injector);
            }
        }
    }
}
