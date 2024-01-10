using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathCube : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision coll){
        coll.gameObject.GetComponent<IDamageable>().TakeDamage(Mathf.Infinity,null,1,1);
    }
}
