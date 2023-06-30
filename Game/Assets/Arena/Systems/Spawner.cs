using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner 
{
    public Vector3 coords;
    public Quaternion rotation;

    public Spawner(Transform transform)
    {
        coords = transform.position;
        rotation = transform.rotation;
    }

}
