using System;
using System.Collections;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public bool isPublic = true;
    private string poolID;
    public bool enableOnUse = true;
    public bool enableOnInstance = true;
    public GameObject prefab;

    private void Awake()
    {
        poolID = gameObject.name;
        if (isPublic){
            if (PublicPools.PoolsExists(poolID)){
                //Debug.Log("Dupe version of "+ poolID +" detected!");
                return;
            }
            PublicPools.pools.Add(poolID, this);
        }
        //Debug.Log(poolID);
    }

    public GameObject SendObject(GameObject receiver)
    {
        if (transform.childCount == 0){
            prefab.SetActive(false);
            var instance = Instantiate(prefab);
            instance.transform.SetParent(receiver.transform, false);
            instance.name = poolID;
            if (enableOnUse)
                instance.SetActive(true);
            return instance;
        }else{
            var instance = transform.GetChild(0).gameObject;
            instance.transform.SetParent(receiver.transform, false);
            if (enableOnUse)
                instance.SetActive(true);
            return instance;
        }
    }

    public GameObject UseObject(Vector3 position, Quaternion rotation)
    {
        if (transform.childCount == 0)
        {
            var instance = Instantiate(prefab, position, rotation);
            if (enableOnInstance)
                instance.SetActive(true);
            instance.transform.SetParent(transform.parent, false);
            instance.name = poolID;
            return instance;
        }
        else
        {
            var instance = transform.GetChild(0).gameObject;
            instance.transform.SetParent(transform.parent, false);
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            if (enableOnUse)
                instance.SetActive(true);
            return instance;
        }
    }

    public GameObject UseObject(Vector3 position, Quaternion rotation, out bool instantiated)
    {
        if (transform.childCount == 0)
        {
            var instance = Instantiate(prefab, position, rotation);
            if (enableOnInstance)
                instance.SetActive(true);
            instance.name = poolID;
            instantiated = true;
            return instance;
        }
        else
        {
            var instance = transform.GetChild(0).gameObject;
            instance.transform.SetParent(null, false);
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            if (enableOnUse)
                instance.SetActive(true);
            instantiated = false;
            return instance;
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
        if (package.activeSelf)
            package.SetActive(false);
        package.transform.SetParent(transform, false);
    }

    public void Reattach(GameObject package)
    {
        StartCoroutine(ReceiveObject(package));
    }
}
