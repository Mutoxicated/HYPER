using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BobInfo", menuName = "ScriptableObjects/BobInfo", order = 1)]
public class BobInformation : ScriptableObject {
    public Quaternion direction;
    public float speed;
}
