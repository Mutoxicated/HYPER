using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ObjectPoolManager : MonoBehaviour
{
    private string poolID;
    public GameObject prefab;

    private void Awake()
    {
        poolID = gameObject.name;
        if (PublicPools.PoolsExists(poolID))
            return;
        PublicPools.pools.Add(poolID, this);
        Debug.Log(poolID);
    }

    public GameObject SendObject(GameObject receiver)
    {
        if (transform.childCount == 0){
            prefab.SetActive(false);
            var instance = Instantiate(prefab);
            instance.transform.SetParent(receiver.transform, false);
            instance.name = poolID;
            instance.SetActive(true);
            return instance;
        }else{
            var instance = transform.GetChild(0).gameObject;
            instance.transform.SetParent(receiver.transform, false);
            instance.SetActive(true);
            return instance;
        }
    }

    public void UseObject(Vector3 position, Quaternion rotation)
    {
        if (transform.childCount == 0)
        {
            var instance = Instantiate(prefab, position, rotation);
            instance.name = poolID;
        }
        else
        {
             var instance = transform.GetChild(0).gameObject;
            instance.transform.SetParent(null, false);
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            instance.SetActive(true);
        }
    }

    private IEnumerator ReceiveObject(GameObject package)
    {
        yield return new WaitForSeconds(.001f);
        package.transform.SetParent(transform, false);
        if (package.activeSelf)
            package.SetActive(false);
        yield break;
    }

    public void ReattachImmediate(GameObject package)
    {
        package.transform.SetParent(transform, false);
        if (package.activeSelf)
            package.SetActive(false);
    }

    public void Reattach(GameObject package)
    {
        StartCoroutine(ReceiveObject(package));
    }
}
