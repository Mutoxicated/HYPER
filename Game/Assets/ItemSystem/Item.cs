using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item", order = 3)]
public class Item : ScriptableObject
{
    public GameObject item;
    public Sprite itemImage;
    public string description;
}
