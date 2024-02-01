using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModifyMisc : MonoBehaviour
{
    [SerializeField] private SuperPassive sp;
    [Header("Equipment")]
    [SerializeField] private float equipmentEffectivenessModStep = -1f;
    [Header("Bullet")]
    [SerializeField] private float bulletEffectivenessModStep = -1f;
    [Header("TNT")]
    [SerializeField] private float tntEffectivenessModStep = -1f;
    [Header("Enemy Passive Pool")]
    [SerializeField] private float passiveEffectivenessStep = -1f;
    [Header("Platform Objective Shield Chance")]
    [SerializeField] private float shieldChanceStep = -1f;
    [Header("Shop cheapness")]
    [SerializeField] private float cheapnessModStep = -1f;

    private void Step(int num){
        if (equipmentEffectivenessModStep > 0f){
            EquipmentManager.effectivenessMod += equipmentEffectivenessModStep*num;
        }
        if (tntEffectivenessModStep > 0f){
            TNT.tntEffectiveness += tntEffectivenessModStep*num;
        }
        if (bulletEffectivenessModStep > 0f){
            bullet.bulletEffectiveness += bulletEffectivenessModStep*num;
        }
        if (passiveEffectivenessStep > 0f){
            PassivePool.effectivenessMod += passiveEffectivenessStep*num;
        }
        if (shieldChanceStep > 0f){
            PlatformObjective.shieldChance += shieldChanceStep*num;
        }
        if (cheapnessModStep > 0f){
            ItemShop.cheapnessMod += cheapnessModStep*num;
        }
    }

    private void Start()
    {
        sp.subs.Add(Step);
    }
}
