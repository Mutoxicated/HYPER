using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWeapon : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    private void OnEnable(){
        PlayerInfo.playerGun.AddWeapon(weapon);
    }

    private void OnDisable(){
        if (PlayerInfo.playerGun.ContainsWeapon(weapon))
            PlayerInfo.playerGun.RemoveWeapon(weapon);
    }
}
