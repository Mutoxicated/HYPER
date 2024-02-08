using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 3)]
public class Item : ScriptableObject
{
    public ClassItem item;
    public int cost;
    public string itemName;
    public Color nameColor;
    public Sprite itemImage;
    public string description;
}
