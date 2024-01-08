using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ModifyWeapon : MonoBehaviour
{
    [SerializeField] private int amountToModify;
    [SerializeField] private float modifier;
    [SerializeField] private bool boolModifer;
    [SerializeField] private bool modifyFireRate;
    [SerializeField] private bool modifyExtra;

    private List<Weapon> wps = new List<Weapon>();

    private List<Weapon> GetRandomWeapons(){
        wps = PlayerInfo.GetGun().GetWeapons().ToArray().ToList();
        List<Weapon> weapons = new List<Weapon>();
        int rn = 0;
        int cap = wps.Count;
        if (cap == 0)
            return weapons;
        for (int i = 0; i < cap;i++){
            if (wps.Count == 0)
                break;
            rn = Random.Range(0,wps.Count);
            weapons.Add(wps[rn]);
            wps.Remove(wps[rn]);
        }

        return weapons;
    }

    private void OnEnable(){
        wps = GetRandomWeapons();
        foreach (Weapon wp in wps){
            if (modifyFireRate){
                wp.modifier += modifier;
            }
            if (modifyExtra){
                wp.extra = boolModifer;
            }
        }
    }

    private void OnDestroy(){
        if (wps.Count == 0)
            return;
        foreach (Weapon wp in wps){
            if (modifyFireRate){
                wp.modifier -= modifier;
            }
            if (modifyExtra){
                wp.extra = !boolModifer;
            }
        }
    }
}
