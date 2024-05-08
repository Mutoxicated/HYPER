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
    Sigma_Lanista,
    //HYPER CLASSES \/
    Mooner,
    Xenia,
    Omega_Lanista,
    Artemis,
    Zeus,
    Hekate,
    Dione,
    Mentor,
    Lizzard,
    Hra,
    Baller,
    Avatar,
    Kappa_Lanista,
    Grinder,
    Lambda_Lanista
}
[Serializable]
public enum ItemType {
    ACTIVE,
    WEAPON
}

public class ClassItem : MonoBehaviour
{
    [SerializeField] private ItemType itemType;
    [SerializeField] private List<classType> classes = new List<classType>();//classes the class item is in
    [SerializeField] private Image image;

    [SerializeField] private bool reApplyEffectsOnSceneChange = true;

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

    public void UIMode(bool state){
        image.enabled = state;
    }
}
