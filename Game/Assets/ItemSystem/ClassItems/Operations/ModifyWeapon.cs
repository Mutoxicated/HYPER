using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModifyWeapon : MonoBehaviour
{
    [SerializeField] private ClassItem ci;

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
            rn = SeedGenerator.random.Next(0,wps.Count);
            weapons.Add(wps[rn]);
            wps.Remove(wps[rn]);
        }

        return weapons;
    }

    private void ApplyEffect(Scene scene, LoadSceneMode lsm){
        wps = GetRandomWeapons();
        foreach (Weapon wp in wps){
            //Debug.Log("wp: "+wp);
            if (modifyFireRate){
                wp.fireRateModifier += modifier;
            }
            if (wp.fireRateModifier < 0){
                modifier = modifier - wp.fireRateModifier;
                wp.fireRateModifier = 0;
            }
            if (modifyExtra){
                wp.extraEnabled = boolModifer;
            }
        }
    }

     private void ApplyEffect(){
        wps = GetRandomWeapons();
        foreach (Weapon wp in wps){
            //Debug.Log("wp: "+wp);
            if (modifyFireRate){
                wp.fireRateModifier += modifier;
            }
            if (wp.fireRateModifier < 0){
                modifier = modifier - wp.fireRateModifier;
                wp.fireRateModifier = 0;
            }
            if (modifyExtra){
                wp.extraEnabled = boolModifer;
            }
        }
    }

    private void Start(){
        ApplyEffect();
        if (ci.ReApply())
            SceneManager.sceneLoaded += ApplyEffect;
       
    }

    private void OnDestroy(){
        if (ci.ReApply())
            SceneManager.sceneLoaded -= ApplyEffect;
        if (wps.Count == 0)
            return;
        foreach (Weapon wp in wps){
            if (modifyFireRate){
                wp.fireRateModifier -= modifier;
            }
            if (modifyExtra){
                wp.extraEnabled = !boolModifer;
            }
        }
    }
}
