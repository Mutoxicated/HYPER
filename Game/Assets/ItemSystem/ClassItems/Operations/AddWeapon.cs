using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AddWeapon : MonoBehaviour
{
    [SerializeField] private ClassItem ci;

    [SerializeField] private Weapon weapon;

    private void ApplyEffect(){
        PlayerInfo.GetGun().AddWeapon(weapon);
    }

    private void ApplyEffect(Scene scene, LoadSceneMode lsm){
        PlayerInfo.GetGun().AddWeapon(weapon);
    }

    private void Start(){
        ApplyEffect();
        if (ci.ReApply())
            SceneManager.sceneLoaded += ApplyEffect;
    }

    private void OnDestroy(){
        if (ci.ReApply())
            SceneManager.sceneLoaded -= ApplyEffect;
        if (PlayerInfo.GetGun().ContainsWeapon(weapon))
            PlayerInfo.GetGun().RemoveWeapon(weapon);
    }
}
