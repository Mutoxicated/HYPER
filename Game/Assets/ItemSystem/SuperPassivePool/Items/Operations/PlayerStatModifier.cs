using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatModifier : MonoBehaviour
{
    [SerializeField] private SuperPassive sp;
    [SerializeField] private string[] numericals;
    [SerializeField] private float[] numericalIncrements;

    [SerializeField] private float shieldAmount = -1;
    [SerializeField] private float shieldStep = -1;

    private void GiveShields(Scene scene, LoadSceneMode lsm){
        if (scene.name == "MainMenu" | scene.name == "Interoid")
            return;
        PlayerInfo.GetGun().stats.AddShield(Mathf.RoundToInt(shieldAmount));
    }

    private void Increment(int num){
        if (shieldAmount < 0){
            shieldAmount += shieldStep;
        }
        if (numericals.Length == 0)
            return;
        for (int i = 0; i < numericals.Length; i++)
        {
            PlayerInfo.GetGun().stats.numericals[numericals[i]] += numericalIncrements[i]*num;
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += GiveShields;
        sp.subs.Add(Increment);
    }

    private void OnDestroy(){
        SceneManager.sceneLoaded -= GiveShields;
    }
}
