using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private UnityEvent OnCollision;
    [SerializeField] private GameObject[] ObjectsToDestroy;
    [SerializeField] private GameObject[] ChildrenToDetach;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask layersToIgnore;
    [SerializeField] private string[] tagsToIgnore;

    private void OnCollisionEnter(Collision collision)
    {
        if (tagsToIgnore != null)
        {
            foreach (var tag in tagsToIgnore)
            {
                if (collision.gameObject.tag == tag)
                    return;
            }
        }
        if ((layersToIgnore & (1 << collision.collider.gameObject.layer)) != 0)
            return;
        Detach();
        OnCollision.Invoke();
        DestroyStuff();
        if (damage > 0)
            collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(damage * stats.incrementalStat["damage"], gameObject);
    }

    private void DestroyStuff()
    {
        if (ObjectsToDestroy == null)
            return;
        foreach (var obj in ObjectsToDestroy)
        {
            Destroy(obj);
        }
    }

    private void Detach()
    {
        if (ChildrenToDetach == null)
            return;
        foreach (var obj in ChildrenToDetach)
        {
            obj.transform.parent = null;
        }
    }
}
