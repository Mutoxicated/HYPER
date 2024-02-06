using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ooze : MonoBehaviour
{
    [SerializeField] private Renderer render;

    private Material mat;

    private void Start(){
        mat = render.material;
    }

    private void Update(){
        
    }
}
