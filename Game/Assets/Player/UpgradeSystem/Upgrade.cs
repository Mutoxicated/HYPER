using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public struct Modifier
{
    public float fireRateMult;
    public float damageMult;


    public GameObject bonusCosmetic;
}

public class Upgrade 
{
    Modifier modifier;
    Class @class;

    public Upgrade(Modifier modifier, Class @class)
    {
        this.modifier = modifier;
        this.@class = @class;
    }
}
