using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddWeapon : MonoBehaviour
{
    [SerializeField] private Weapon weapon;

    private void OnEnable(){
        PlayerInfo.GetGun().AddWeapon(weapon);
    }

    private void OnDestroy(){
        if (PlayerInfo.GetGun().ContainsWeapon(weapon))
            PlayerInfo.GetGun().RemoveWeapon(weapon);
    }
}
