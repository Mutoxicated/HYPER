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
    public Immunity immuneSystem;
    public List<BodyPart> bodyParts = new List<BodyPart>();
    public bool injectEnabled = true;
    public float chance = 100f;
    public injectorType type;
    public List<Injector> injectorsToInherit = new List<Injector>();
    public List<string> bacteriaPools = new List<string>();
    [HideInInspector]
    public List<Bacteria> allyBacterias = new List<Bacteria>();

    private void OnEnable(){
        if (allyBacterias.Count > 0){
            return;
        }
        foreach (var bac in bacteriaPools.ToArray()){
            PublicPools.pools[bac+"_ALLY"].SendObject(gameObject);
        }
        if (injectorsToInherit.Count > 0){
            foreach(var inj in injectorsToInherit){
                inj.InheritInjector(this);
            }
        }
    }

    public void InheritInjector(Injector injector){
        chance = injector.chance;
        injectEnabled = injector.injectEnabled;
        type = injector.type;
        foreach(var bac in injector.immuneSystem.foreignBacteria){
            PublicPools.pools[bac.gameObject.name].SendObject(gameObject);
            Debug.Log("hap");
        }
    }

    private void OnDisable(){
        allyBacterias.Clear();
        bacteriaPools.Clear();
    }

    public void AddAllyBacteria(string name)
    {
        bacteriaPools.Add(name);
        allyBacterias.Add(PublicPools.pools[name+"_ALLY"].SendObject(gameObject).GetComponent<Bacteria>());
    }

    public void AddInvaderBacteria(string name){
        PublicPools.pools[name].SendObject(gameObject);
    }

    public void RemoveBacteria(string name)
    {
        foreach (var allyBac in allyBacterias){
            if (allyBac.name == name){
                allyBacterias.Remove(allyBac);
                allyBac.Instagib();
                return;
            }
        }
        bacteriaPools.Remove(name);
    }

    public void Clear()
    {
        foreach (var bac in allyBacterias.ToArray()){
            allyBacterias.Remove(bac);
            bac.Instagib();
        }
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
