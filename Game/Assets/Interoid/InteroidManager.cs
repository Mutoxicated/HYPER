using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteroidManager : MonoBehaviour
{
    private GameObject[] platforms;
    private TeleportToScene tts;
    void Start()
    {
        DeactivatePlatforms();
        tts = GameObject.FindWithTag("Teleporter").GetComponent<TeleportToScene>();
        tts.onTeleport.Add(ActivatePlatforms);
    }

    public void ActivatePlatforms(){
        foreach(GameObject platform in platforms){
            platform.SetActive(true);
        }
    }

    public void DeactivatePlatforms(){
        platforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach(GameObject platform in platforms){
            platform.SetActive(false);
        }
    }
}
