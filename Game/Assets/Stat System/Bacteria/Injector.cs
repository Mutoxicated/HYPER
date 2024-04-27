using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    public Injector injectorToInheritFrom;
    public List<Injector> injectorsToInherit = new List<Injector>();
    public List<string> bacteriaPools = new List<string>();
    [HideInInspector] public List<Bacteria> allyBacterias = new List<Bacteria>();
    [HideInInspector] public List<Bacteria> cachedInstances = new List<Bacteria>();
    // TODO: Make it spawn invader bacteria the same way TakeInjector does it (because it will create complications with the population of the bacteria)
    private void OnEnable(){
        if (immuneSystem != null){
            foreach (var bac in bacteriaPools){
                PublicPools.pools[bac].SendObject(immuneSystem.gameObject);
            }
        }
        if (injectorToInheritFrom != null){
            //Debug.Log(gameObject.name + " inherited from " + injectorToInheritFrom.gameObject.name);
            InheritInjector(injectorToInheritFrom);
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
        if (injector.immuneSystem == null)
            return;
        foreach(var bac in injector.immuneSystem.bacterias.Values){
            if (bac.ID.character == BacteriaCharacter.NEGATIVE && bac.immunitySide == ImmunitySide.INVADER)
                continue;
            for (int i = 0; i < bac.population; i++){
                PublicPools.pools[bac.gameObject.name].SendObject(immuneSystem.gameObject);
            }
        }
    }

    private void OnDisable(){
        allyBacterias.Clear();
    }

    public void AddBacteria(string name){
        if (immuneSystem == null)
            return;
        immuneSystem.AddBacteria(name);
    }

    public void AddBacterias(string name, int amount){
        if (immuneSystem == null)
            return;
        if (!PublicPools.pools.ContainsKey(name))
            return;
        for (int i = 0; i < amount; i++){
            immuneSystem.AddBacteria(name);
        }
    }

    public void RemoveBacteria(string name)
    {
        if (immuneSystem != null)
            return;
        foreach (var bac in immuneSystem.bacterias.Values.ToArray()){
            if (bac.gameObject.name == name){
                bac.Instagib();
            }
        }
    }

    public void ClearAllies()
    {
        foreach (var bac in allyBacterias.ToArray()){
            bac.Instagib();
        }
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
