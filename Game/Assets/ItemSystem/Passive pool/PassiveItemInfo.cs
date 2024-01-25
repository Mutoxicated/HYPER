using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PassiveItemInfo", menuName = "ScriptableObjects/PassiveItemInfo", order = 3)]
public class PassiveItemInfo : ScriptableObject
{
    public PassiveItem item;
    public string itemName;
    public Color nameColor;
    public Sprite itemImage;
}

