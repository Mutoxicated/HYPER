using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMatColor : MonoBehaviour
{
    //changes the color of the wireframe material instance
    //so i don't have to make new materials for different colors

    [SerializeField] private int matIndex = 1;
    [SerializeField,ColorUsage(true,true)] private Color color;
    private void Awake()
    {
        Material[] mats = GetComponent<Renderer>().materials;
        Material mat = mats[matIndex];
        mat.color = color;
    }
}
