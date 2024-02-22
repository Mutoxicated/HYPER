using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public bool isPublic = true;
    private string poolID;
    public bool enableOnUse = true;
    public bool enableOnInstance = true;
    public GameObject prefab;

    private List<GameObject> outsideGos = new List<GameObject>();

    private void Start()
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
            outsideGos.Add(instance);
            instance.transform.SetParent(receiver.transform, false);
            instance.name = poolID;
            if (enableOnUse)
                instance.SetActive(true);
            return instance;
        }else{
            var instance = transform.GetChild(0).gameObject;
            outsideGos.Add(instance);
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
            outsideGos.Add(instance);
            if (enableOnInstance)
                instance.SetActive(true);
            instance.transform.SetParent(transform.parent, false);
            instance.name = poolID;
            return instance;
        }
        else
        {
            var instance = transform.GetChild(0).gameObject;
            outsideGos.Add(instance);
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
            outsideGos.Add(instance);
            if (enableOnInstance)
                instance.SetActive(true);
            instance.name = poolID;
            instantiated = true;
            return instance;
        }
        else
        {
            var instance = transform.GetChild(0).gameObject;
            outsideGos.Add(instance);
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
        if (package == null){
            Debug.Log("Null found on "+gameObject.name);
            return;
        }
        outsideGos.Remove(package);
        if (package.activeSelf)
            package.SetActive(false);
        package.transform.SetParent(transform, false);
    }

    public void Reattach(GameObject package)
    {
        if (package == null){
            Debug.Log("Null found on "+gameObject.name);
            return;
        }
        outsideGos.Remove(package);
        if (gameObject.activeSelf)
            StartCoroutine(ReceiveObject(package));
    }

    public void ReattachAll(){
        foreach (GameObject go in outsideGos.ToArray()){
            ReattachImmediate(go);
        }
    }
}
