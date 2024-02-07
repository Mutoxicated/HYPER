using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePixelSize : MonoBehaviour
{
    [SerializeField] private Renderer rend;
    [SerializeField] private int index = 0;
    [SerializeField] private float pixelSize = 10;

    private void Start(){
        rend.materials[index].SetFloat("_PixelSize",pixelSize);
    }
}
