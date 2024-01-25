using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private void Awake(){
        PublicPools.SetSpawn(false);
        PlayerInfo.SetGetRunData(true);
    }
}
