using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();
    private List<GameObject> gameObjects = new List<GameObject>();

    private void Start(){
        PlayerInfo.SetIP(this);
    }

    private GameObject FindGoFromItem(Item item){
        string goName = item.name.Replace("A","");
        foreach (GameObject g in gameObjects){
            if (g.name == goName){
                return g;
            }
        }
        return null;
    }

    private GameObject FindPrefabFromItem(Item item){
        string goName = item.name.Replace("A","");
        Debug.Log("goName: "+goName);
        foreach (GameObject g in prefabs){
            if (g.name == goName){
                return g;
            }
        }
        return null;
    }

    public void AddItem(Item item){
        GameObject go = FindGoFromItem(item);
        if (go != null)
            return;
        GameObject prefab = FindPrefabFromItem(item);
        if (prefab != null){
            GameObject instance = Instantiate(prefab);
            gameObjects.Add(instance);
            instance.transform.SetParent(transform,true);
        }
    }

    public void RemoveItem(Item item){
        GameObject go = FindGoFromItem(item);
        if (go != null){
            gameObjects.Remove(go);
            Destroy(go);
        }
    }
}
