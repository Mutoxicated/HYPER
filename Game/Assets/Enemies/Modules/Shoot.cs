using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField, Tooltip("If you use this, don't use bulletPrefab.")] private ObjectPoolManager objectPool;
    [SerializeField] private bool inheritInjector;
    [SerializeField] private bool inheritPriority;
    [SerializeField, Tooltip("Used when no object pool is used.")] private GameObject bulletPrefab;
    [SerializeField] private Injector injector;
    [SerializeField] private Transform transformRef;

    private bool instantiated;
    private GameObject instance;
    private Injector cachedInjector;

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
            if (PublicPools.pools.ContainsKey(bulletPrefab.name)){
                instance = PublicPools.pools[bulletPrefab.name].UseObject(transformRef.position, transformRef.rotation);
            }else{
                instance = Instantiate(bulletPrefab, transformRef.position, transformRef.rotation);
            //instance.SetActive(true);
            }
            if (inheritInjector){
                cachedInjector = instance.GetComponent<Injector>();
                cachedInjector?.InheritInjector(injector);
                if (inheritPriority){
                    cachedInjector.immuneSystem.stats.SetPriority(injector.immuneSystem.stats.GetPriority());
                }
            }
        }
    }
}
