using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialInfo
{
    public Material[] mats;
    public GameObject obj;

    public MaterialInfo(GameObject obj, Material[] mats)
    {
        this.obj = obj;
        this.mats = mats;
    }
}
