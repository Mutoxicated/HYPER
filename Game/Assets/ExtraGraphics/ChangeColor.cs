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
        Material mat = GetComponent<Renderer>().materials[1];
        mat?.SetColor("_WireframeBackColour", color);
    }

    private void OnValidate()
    {
        Material mat = GetComponent<Renderer>().sharedMaterials[1];
        mat?.SetColor("_WireframeBackColour", color);
    }
}
