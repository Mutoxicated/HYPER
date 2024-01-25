using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    //player
    public Color UIScreenColor;
    public float FOV;
    public float sensX;
    public float sensY;
    public bool showFPS;

    public PlayerData(Color UIScreenColor, float FOV, float sensX, float sensY, bool showFPS)
    {
        this.UIScreenColor = UIScreenColor;
        this.FOV = FOV;
        this.sensX = sensX;
        this.sensY = sensY;
        this.showFPS = showFPS;
    }
}
