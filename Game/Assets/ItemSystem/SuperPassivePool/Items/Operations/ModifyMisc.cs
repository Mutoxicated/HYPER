using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMisc : MonoBehaviour
{
    [Header("Equipment")]
    [SerializeField] private float equipmentEffectivenessMod = -1f;
    [SerializeField] private float equipmentEffectivenessModStep = -1f;
    [Header("TNT")]
    [SerializeField] private float tntEffectivenessMod = -1f;
    [SerializeField] private float tntEffectivenessModStep = -1f;

    private void Step(){
        if (equipmentEffectivenessMod >= 0f){
            equipmentEffectivenessMod += equipmentEffectivenessModStep;
        }
        if (tntEffectivenessMod >= 0f){
            tntEffectivenessMod += tntEffectivenessModStep;
        }
    }

    private void Develop(){
        Step();
    }
}
