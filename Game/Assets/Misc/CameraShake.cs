using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake
{
    public Camera _cam;
    public int duration;
    public float strength;
    public float speed;

    private int t;

    public CameraShake(Camera cam, int duration, float strength, float speed)
    {
        _cam = cam;
        this.duration = duration;
        this.strength = strength;
        this.speed = speed;
    }

    public void Shake() 
    { 
        while(t < duration)
        {

        }
    }
}
