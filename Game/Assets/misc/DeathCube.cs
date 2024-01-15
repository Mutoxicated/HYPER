using UnityEngine;

public class DeathCube : MonoBehaviour
{
    
    private void OnCollisionEnter(Collision coll){
        coll.gameObject.GetComponent<IDamageable>().TakeDamage(Mathf.Infinity,null,1,1);
    }
}
