using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour, IEvent
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private bool rotate;

    public void Event(Vector3 position, Quaternion rotation) {
        if (rotate) {
            Instantiate(prefab, position, rotation);
        }else {
            Instantiate(prefab, position, Quaternion.identity);
        }
    }
}
