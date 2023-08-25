using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToSender : MonoBehaviour
{
    private void OnDisable()
    {
        PublicPools.pools[gameObject.name].Reattach(gameObject);
    }
}
