using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToSender : MonoBehaviour
{
    public bool isPublic = true;
    public ObjectPoolManager localPool;
    private void OnDisable()
    {
        if (isPublic){
            PublicPools.pools[gameObject.name].Reattach(gameObject);
        }else{
            if (localPool == null){
                Destroy(gameObject);
                return;
            }
            localPool.Reattach(gameObject);
        }
    }
}
