using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObjective : MonoBehaviour
{
    [SerializeField] private GameObject objectivePrefab;

    [SerializeField] private int touchTolerance = 1;
    private int currentTouches = 0;

    private GameObject instance;

    private void OnCollisionEnter(Collision collision){
        if (collision.gameObject.tag != "Player")
            return;
        currentTouches++;
        if (currentTouches == touchTolerance){
            if (objectivePrefab != null)
                instance = Instantiate(objectivePrefab,transform,true);
            instance.transform.position = transform.position;
        }
    }
}
