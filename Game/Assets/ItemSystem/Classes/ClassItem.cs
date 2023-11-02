using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Class {
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
    Hephaestus,
    Baller,
    Avatar,
    Kappa_Lanista,
    Grinder,
    Lambda_Lanista
}

public class ClassItem : MonoBehaviour
{
    public List<Class> classesContainingThis = new List<Class>();

    private void Start(){
        ClassItemPool.publicItemPool.items.Add(this);
    }
}
