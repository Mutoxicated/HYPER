using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum injectorType
{
    ALL,
    PLAYER,
    ENEMIES
}

public class Injector : MonoBehaviour
{
    public List<Renderer> bodyParts = new List<Renderer>();
    public List<Injector> inheritInjectors = new List<Injector>();
    public bool injectEnabled = true;
    public float chance = 100f;
    public injectorType type;
    public List<string> bacteriaPools = new List<string>();
    [HideInInspector]
    public List<GameObject> allyBacterias = new List<GameObject>();

    private void OnEnable(){
        if (allyBacterias.Count > 0){
            return;
        }
        foreach (var bac in bacteriaPools.ToArray()){
            allyBacterias.Add(PublicPools.pools[bac+"_ALLY"].SendObject(gameObject));
            if (inheritInjectors.Count == 0){
                continue;   
            }
            foreach (var injector in inheritInjectors){
                allyBacterias.Add(PublicPools.pools[bac+"_ALLY"].SendObject(injector.gameObject));
            }
        }
    }

    public void AddBacteria(string name)
    {
        bacteriaPools.Add(name);
    }

    public void RemoveBacteria(string name)
    {
        bacteriaPools.Remove(name);
    }

    public void Clear()
    {
        bacteriaPools.Clear();
    }

    public void ChangeChance(float num)
    {
        chance = num;
    }

    public void SetInjectorActive(bool enabled)
    {
        injectEnabled = enabled; 
    }

    public void ChangeType(int num)
    {
        type = (injectorType)num;
    }
}
