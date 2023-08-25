using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum injectorType
{
    ALL,
    PLAYER,
    ENEMIES
}

public class Injector : MonoBehaviour
{
    public bool injectEnabled = true;
    public float chance = 100f;
    public injectorType type;
    public List<string> bacteriaPools = new List<string>();
}
