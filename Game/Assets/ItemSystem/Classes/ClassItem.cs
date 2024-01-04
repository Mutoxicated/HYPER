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

public enum ItemType {
    PASSIVE,
    WEAPON
}

public class ClassItem : MonoBehaviour
{
    [SerializeField] private ItemType itemType;
    [SerializeField] private List<Class> classes = new List<Class>();//classes the class item is in
}
