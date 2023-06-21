using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColor : MonoBehaviour
{
    //changes the color of the wireframe material instance
    //so i don't have to make new materials for different colors

    [SerializeField] private Color color;
    private void Start()
    {
        Material[] mats = GetComponent<Renderer>().sharedMaterials;
        Material mat = mats[mats.Length-1];
        mat?.SetColor("_WireframeBackColour", color);
    }

    private void OnValidate()
    {
        Material[] mats = GetComponent<Renderer>().sharedMaterials;
        Material mat = mats[mats.Length - 1];
        mat?.SetColor("_WireframeBackColour", color);
    }
}
