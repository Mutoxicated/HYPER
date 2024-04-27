using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BacteriaInfo", menuName = "ScriptableObjects/BacteriaInfo", order = 4)]
public class BacteriaInfo : ScriptableObject
{
    public Color color = Color.white;
    public BacteriaType type;
    public BacteriaCharacter character;
}
