using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[Serializable]
public enum ClassHierarchy{
    Starter,
    Synergized,
    HYPER
}

[Serializable]
public enum classType {
    //STARTER CLASSES \/
    Evocus,
    Magicka,
    Bloodthirst,
    Bravery,
    //SYNERGIZED CLASSES \/
    Beta_Lanista,
    Veneficus,
    Alpha_Lanista,
    Sagacita,
    Elementum,
    //HYPER CLASSES \/
    Mooner,
    Xenia,
    Omega_Lanista,
    Artemis,
    Zeus,
    Dione,
    Mentor,
    Lizzard,
    Hra,
    Avatar,
}
[Serializable]
public enum ItemType {
    ACTIVE,
    PASSIVE
}

public class ClassItem : MonoBehaviour
{
    public Item itemInfo;
    [SerializeField] private InfoBoxPopper ibp;
    [SerializeField] private ItemType itemType;
    public ClassHierarchy classHierarchy;
    [SerializeField] private List<classType> classes = new List<classType>();//classes the class item is in
    [SerializeField] private Image image;

    [SerializeField] private bool reApplyEffectsOnSceneChange = true;
    [HideInInspector] public AddItemsToSorter aits;
    [HideInInspector] public bool sellable = false;

    public void IncreaseClassesBattery(){
        foreach (classType ct in classes){
            ClassSystem.IncrementClassBattery(ct);
        }
    }

    private void SceneCheck(Scene scene, LoadSceneMode lsm){
        if (scene.name == "MainMenu"){
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneCheck;
        DontDestroyOnLoad(transform);
        IncreaseClassesBattery();
    }

    private void OnDestroy(){
        foreach (classType ct in classes){
            ClassSystem.DecrementClassBattery(ct);
        }
        SceneManager.sceneLoaded -= SceneCheck;
    }

    public bool ReApply(){
        return reApplyEffectsOnSceneChange;
    }

    public List<classType> GetClasses(){
        return classes;
    }

    public void Sell() {
        if (!sellable) 
        {
            Debug.Log("Not sellable ("+gameObject.name+").", gameObject);
            return;
        };
        PlayerInfo.SetMoney(ItemShop.Processed(itemInfo.cost*ItemShop.sellMultiplier));
        aits.RemoveItem(this);
        if (ibp.popped) {
            InfoBox.ib.UnpopBox();
        }
        sellable = false;
    }

    public void SellMode(bool state) {
        sellable = state;
    }

    public void EnableUIMode(Transform trans){
        transform.SetParent(trans,true);
        transform.localRotation = Quaternion.identity;
        transform.localScale *= 0.1f;
        transform.localPosition = Vector3.zero;
        image.enabled = true;
    }

    public void DisableUIMode(){
        image.enabled = false;
        transform.localRotation = Quaternion.identity;
        transform.localScale *= 10f;
        transform.localPosition = Vector3.zero;
        transform.SetParent(null,true);
        DontDestroyOnLoad(transform);
    }
}
