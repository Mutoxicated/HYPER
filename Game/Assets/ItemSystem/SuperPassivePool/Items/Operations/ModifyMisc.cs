using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMisc : MonoBehaviour
{
    [SerializeField] private SuperPassive sp;
    [Header("Equipment")]
    [SerializeField] private float equipmentEffectivenessMod = -1f;
    [SerializeField] private float equipmentEffectivenessModStep = -1f;
    [Header("TNT")]
    [SerializeField] private float tntEffectivenessMod = -1f;
    [SerializeField] private float tntEffectivenessModStep = -1f;

    private void Step(int num){
        if (equipmentEffectivenessMod >= 0f){
            equipmentEffectivenessMod += equipmentEffectivenessModStep*num;
        }
        if (tntEffectivenessMod >= 0f){
            tntEffectivenessMod += tntEffectivenessModStep*num;
        }
    }

    private void Start()
    {
        sp.subs.Add(Step);
    }
}
