using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

[Serializable]
public class Stats : MonoBehaviour
{
    public GameObject explosionPrefab;
    public float VFXScale = 1f;
    
    public Dictionary<string, bool> conditionals = new Dictionary<string, bool>();
    public Dictionary<string, float> numericals = new Dictionary<string, float>();


    private void Awake()
    {
        conditionals.Add("explosive", false);

        numericals.Add("moveSpeed", 1f);
        numericals.Add("damage", 1f);
        numericals.Add("rate", 1f);
        numericals.Add("attackSpeed", 1f);
        numericals.Add("shootSpeed", 1f);
        numericals.Add("capacitor1", 1f);
        numericals.Add("capacitor2", 1f);
        numericals.Add("pierces", 1f);
        numericals.Add("shields", 1);
        numericals.Add("health", 100f);
    }


    public void EnableConditional(string name){
        conditionals[name] = true;
    }

    public void DisableConditional(string name){
        conditionals[name] = false;
    }

    public void ChangeNum(string name, float num){
        numericals[name] = num;
    }
}
