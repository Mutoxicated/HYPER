using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassItemPool : MonoBehaviour
{
    public static ClassItemPool publicItemPool;

    public List<ClassItem> items = new List<ClassItem>();
    
    public Stats playerStats;
    public GunShooter playerGun;

    private void Awake(){
        publicItemPool = this;
    }

    private void Start(){
        playerStats = Difficulty.player.GetComponent<Stats>();
        playerGun = Difficulty.player.GetComponent<GunShooter>();
    }
}
