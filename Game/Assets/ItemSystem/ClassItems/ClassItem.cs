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

public delegate void State(bool state);


public class ClassItem : MonoBehaviour
{
    private static Sprite lockedImage;
    public Item itemInfo;
    [SerializeField] private InfoBoxPopper ibp;
    [SerializeField] private ItemType itemType;
    public ClassHierarchy classHierarchy;
    [SerializeField] private List<classType> classes = new List<classType>();//classes the class item is in
    [SerializeField] private Image image;
    [SerializeField] private Sprite unlockedImage;

    [SerializeField] private bool reApplyEffectsOnSceneChange = true;
    private bool sellable = false;

    public State state = new State(Empty);
    public delegate void OnEvent(ClassItem trans);

    public static void Empty(bool state) {}
    public static void Empty(ClassItem transform) {}

    private Transform ogTransform;

    public List<classType> Classes {
        get { return classes; }
    }

    private void SceneCheck(Scene scene, LoadSceneMode lsm){
        if (scene.name == "MainMenu" && gameObject){
            Destroy(gameObject);
        }
    }

    public void Start() {
        ogTransform = transform.parent;
    }

    public void Enable()
    {
        state.Invoke(true);
        lockedImage = image.sprite;
        image.sprite = unlockedImage;
        SceneManager.sceneLoaded += SceneCheck;
        foreach (var ct in classes) {
            ClassSystem.IncrementClassBattery(ct);
        }
    }

    public void Disable(){
        state.Invoke(false);
        image.sprite = lockedImage;
        SceneManager.sceneLoaded -= SceneCheck;
        foreach (var ct in classes) {
            ClassSystem.DecrementClassBattery(ct);
        }
    }

    public bool ReApply(){
        return reApplyEffectsOnSceneChange;
    }

    public List<classType> GetClasses(){
        return classes;
    }

    public void PendClassesBattery(bool state) {
        if (state) {
            foreach (var ct in classes) {
                ClassSystem.classDict[ct].PendBatteryDecrease();
            }
            return;
        }
        foreach (var ct in classes) {
            ClassSystem.classDict[ct].UnpendBattery();
        }
    }

    public void Sell() {
        if (image.sprite == lockedImage) return;
        if (!sellable) 
        {
            Debug.Log("Not sellable ("+gameObject.name+").", gameObject);
            return;
        };
        PlayerInfo.SetMoney(ItemShop.Processed(itemInfo.cost*ItemShop.sellMultiplier));
        PlayerInfo.GetIP().RemoveItem(itemInfo);
        ItemShop.IS.CheckItemsLeft();
        
        if (ibp.popped) {
            InfoBox.ib.UnpopBox();
        }
        ibp.enabled = false;
        SellMode(false);
    }

    public void SellMode(bool state) {
        if (state && !sellable) {
            ibp.state += PendClassesBattery;
            
        }else if (!state && sellable) {
            ibp.state -= PendClassesBattery;
        }
        sellable = state;
    }

    public void EnableUIMode(Transform trans){
        Debug.Log("Entering UI Mode");
        transform.SetParent(trans,true);
        Debug.Log("     Scale: "+transform.localScale);
        transform.localRotation = Quaternion.identity;
        transform.localScale *= 0.1f;
        transform.localPosition = Vector3.zero;
        Debug.Log("     Scale: "+transform.localScale);
        image.enabled = true;

        ibp.enabled = image.sprite != lockedImage;
    }

    public void DisableUIMode(){
        image.enabled = false;
        transform.localRotation = Quaternion.identity;
        transform.localScale *= 10f;
        transform.localPosition = Vector3.zero;
        transform.SetParent(ogTransform,true);
        DontDestroyOnLoad(transform.root);
    }
}