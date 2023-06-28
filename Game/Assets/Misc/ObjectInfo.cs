using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectInfo
{
    public Material[] mats;
    public GameObject obj;

    public ObjectInfo(GameObject obj, Material[] mats)
    {
        this.obj = obj;
        this.mats = mats;
    }
}
