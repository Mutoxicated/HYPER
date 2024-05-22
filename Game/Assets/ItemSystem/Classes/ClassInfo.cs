using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ClassInfo", menuName = "ScriptableObjects/ClassInfo")]
public class ClassInfo : ScriptableObject
{
    public classType _classType;
    public ClassHierarchy hierarchy;
    public Color color = Color.white;
    public Sprite image;
    public string desc;
}
