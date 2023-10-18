using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private Bacteria bac;

    public SurfaceEffect sfx;
    public OutlineEffect ofx;
    public ColorEffect cfx;

    private void OnEnable(){
        Invoke("SetEffect",0.01f);
    }

    private void SetEffect(){
        foreach (var allyBac in bac.immuneSystem.injector.allyBacterias){
            if (bac.gameObject == allyBac.gameObject){//put gameObject cuz maybe its more reliable
                if (!bac.immuneSystem.stats.conditionals["surfaceFXED"]){
                    sfx.enabled = true;
                    return;
                }
                if (!bac.immuneSystem.stats.conditionals["outlineFXED"]){
                    ofx.enabled = true;
                    return;
                }
                if (!bac.immuneSystem.stats.conditionals["colorFXED"]) {
                    cfx.enabled = true;
                    return;
                }
            }
        }
    }
}
