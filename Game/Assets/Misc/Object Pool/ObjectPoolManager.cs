using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPoolManager : MonoBehaviour
{
    private string poolID;
    public GameObject prefab;
    public Queue<GameObject> queue = new Queue<GameObject>();


    private void Awake()
    {
        poolID = gameObject.name;
        if (PublicPools.PoolsExists(poolID))
            return;
        PublicPools.pools.Add(poolID, this);
        Debug.Log(poolID);
    }

    public void SendObject(GameObject receiver)
    {
        if (queue.Count == 0 || queue.Peek().transform.parent == transform.parent || queue.Peek().activeSelf)
        {
            var instance = Instantiate(prefab);
            instance.name = poolID;
            instance.transform.SetParent(receiver.transform);
            instance.SetActive(true);
            queue.Enqueue(instance);
        }
        else
        {
            var instance = queue.Dequeue();
            instance.transform.SetParent(receiver.transform);
            instance.SetActive(true);
            queue.Enqueue(instance);
        }
    }

    public void UseObject(Vector3 position, Quaternion rotation)
    {
        if (queue.Count == 0 || queue.Peek().activeSelf)
        {
            var instance = Instantiate(prefab, position, rotation);
            instance.name = poolID;
            queue.Enqueue(instance);
        }
        else
        {
            var instance = queue.Dequeue();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.SetActive(true);
            queue.Enqueue(instance);
        }
    }

    private IEnumerator ReceiveObject(GameObject package)
    {
        yield return new WaitForSeconds(.001f);
        package.transform.SetParent(transform);
        if (package.activeSelf)
            package.SetActive(false);
        yield break;
    }

    public void ReattachImmediate(GameObject package)
    {
        package.transform.SetParent(transform);
        if (package.activeSelf)
            package.SetActive(false);
    }

    public void Reattach(GameObject package)
    {
        StartCoroutine(ReceiveObject(package));
    }
}
