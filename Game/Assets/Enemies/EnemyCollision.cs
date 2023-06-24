using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [SerializeField] private GameObject[] ObjectsToDestroy;
    [SerializeField] private GameObject particleUponCollision;
    [SerializeField] private bool detachChildren;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "bullets")
            return;
        if (particleUponCollision != null)
            Instantiate(particleUponCollision, collision.contacts[0].point, Quaternion.LookRotation(collision.contacts[0].normal));
        Detach();
        DestroyStuff();
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
        if (!detachChildren)
            return;
        transform.DetachChildren();
    }
}
