using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private Sprite empty;
    [SerializeField] private GameObject selectObj;
    [SerializeField] private List<Image> images = new List<Image>();

    private GunShooter gun;

    private List<Weapon> weapons = new List<Weapon>();

    private void Start(){
        gun = PlayerInfo.GetGun();
    }

    private void UpdateImages(){
        weapons = gun.GetWeapons();
        for (int i = 0; i < images.Count; i++){
            if (i > weapons.Count-1){
                images[i].sprite = empty;
            }else{
                images[i].sprite = weapons[i].weaponSymbol;
            }
        }
    }

    private void UpdateSelected(){
        selectObj.transform.position = images[gun.scroll.index].transform.parent.position;
    }

    private void Update(){
        UpdateImages();
        UpdateSelected();
    }
}
