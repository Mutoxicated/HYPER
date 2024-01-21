using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnReload : MonoBehaviour
{
    private static Dictionary<string, DestroyOnReload> dict = new Dictionary<string, DestroyOnReload>();
    private void Awake() {

        if (dict.ContainsKey(gameObject.name) && dict[gameObject.name] != this) {
            Destroy(gameObject);
        }
        else {
            if (dict.ContainsKey(gameObject.name))
                dict[gameObject.name] = this;
            else dict.Add(gameObject.name,this);
        }
    }
}
