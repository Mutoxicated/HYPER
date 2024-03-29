using UnityEngine;

public class TNT : MonoBehaviour
{
    public static float tntEffectiveness = 1f;

    [SerializeField] private float speed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject explosionPrefab;

    public void Explode()
    {
        PublicPools.pools[explosionPrefab.name].UseObject(transform.position,Quaternion.identity);
    }

    void Start()
    {
        rb.velocity = transform.forward * speed;
    }
}
