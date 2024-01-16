using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjects/Equipment")]
public class Equipment : ScriptableObject
{
    [SerializeField] private Sprite symbol;
    [SerializeField] private int cost;
}
