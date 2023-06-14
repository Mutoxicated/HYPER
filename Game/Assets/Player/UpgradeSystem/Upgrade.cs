using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Upgrade : MonoBehaviour
{
    public Class @class;
    public Class.type classType;

    private void Start()
    {
        @class._type = classType;
    }
}
