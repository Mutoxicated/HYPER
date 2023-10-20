using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField, Tooltip("If you use this, don't use bulletPrefab.")] private ObjectPoolManager objectPool;
    [SerializeField] private bool inheritInjector;
    [SerializeField, Tooltip("Used when no object pool is used.")] private GameObject bulletPrefab;
    [SerializeField] private Injector injector;
    [SerializeField] private Transform transformRef;

    private bool instantiated;
    private GameObject instance;

    public void ShootPrefab()
    {
        if (objectPool != null)
        {
            instance = objectPool.UseObject(transformRef.position,transformRef.rotation, out instantiated);
            if (instantiated){
                var rts = instance.GetComponent<ReturnToSender>();
                rts.localPool = objectPool;
                rts.isPublic = false;
                var inj = instance.GetComponent<Injector>();
                inj.injectorToInheritFrom = injector;
                inj.InheritInjector(injector);
            }
        }
        else
        {
            instance = Instantiate(bulletPrefab, transformRef.position, transformRef.rotation);
            //instance.SetActive(true);
            if (inheritInjector){
                instance.GetComponent<Injector>()?.InheritInjector(injector);
            }
        }
    }
}
