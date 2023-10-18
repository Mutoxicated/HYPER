using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class BodyPart : MonoBehaviour
{
    public Renderer _renderer;
    public HitGradient hitGradient;
    
    //effects body part is elligible for
    public bool surfaceEffectable;
    public bool outlineEffectable;
    public bool colorEffectable;

    public int matIndex; // for color effect
}
