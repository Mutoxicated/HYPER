using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject inventory;

    public void Open(int classIndex) {
        inventory.SetActive(false);
        gameObject.SetActive(true);
    }
}
