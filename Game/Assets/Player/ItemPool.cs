using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ItemPool : MonoBehaviour
{
    [SerializeField] private bool receiver;
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();
    private static List<GameObject> gameObjects = new List<GameObject>();

    private void Awake(){
        if (!receiver){
            PlayerInfo.SetIP(this);
        }
    }

    private void Start(){
        if (!receiver)
            return;
        foreach (GameObject go in gameObjects){
            go.SetActive(true);
            //Debug.Log(go);
        }
    }

    private GameObject FindGoFromItem(Item item){
        string goName = item.name.Replace("A","");
        foreach (GameObject g in gameObjects){
            if (g.name.Replace("(Clone)","") == goName){
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

    public bool AddItem(Item item){
        GameObject go = FindGoFromItem(item);
        if (go != null)
            return false;
        GameObject prefab = FindPrefabFromItem(item);
        if (prefab != null){
            GameObject instance = Instantiate(prefab);
            gameObjects.Add(instance);
           DontDestroyOnLoad(instance);
        }
        return true;
    }

    public void RemoveItem(Item item){
        GameObject go = FindGoFromItem(item);
        if (go != null){
            gameObjects.Remove(go);
            Destroy(go);
        }
    }
}
