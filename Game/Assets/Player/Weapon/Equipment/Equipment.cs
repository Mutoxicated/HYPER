using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Equipment")]
public class Equipment : ScriptableObject
{
    public Sprite symbol;
    public int cost;
}
