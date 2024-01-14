using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EffectManager : MonoBehaviour
{
    [SerializeField] private Bacteria bac;

    public SurfaceEffect sfx;
    public OutlineEffect ofx;
    public ColorEffect cfx;

    private int colorEffectableAmount;
    private int outlineEffectableAmount;
    private int surfaceEffectableAmount;

    public void SetEffect(){
        if (bac.immuneSystem.injector.bodyParts.Count != 0){
            foreach (BodyPart bodyPart in bac.immuneSystem.injector.bodyParts){
            if (bodyPart.colorEffectable)
                colorEffectableAmount++;
            if (bodyPart.outlineEffectable)
                outlineEffectableAmount++;
            if (bodyPart.surfaceEffectable)
                surfaceEffectableAmount++;
        }
        // Debug.Log(transform.parent.gameObject.name+ ", color: "+colorEffectableAmount);
        // Debug.Log(transform.parent.gameObject.name+ ", outline: "+outlineEffectableAmount);
        // Debug.Log(transform.parent.gameObject.name+ ", surface: "+surfaceEffectableAmount);
        }
        if (surfaceEffectableAmount > 0 && !bac.immuneSystem.stats.conditionals["surfaceFXED"]){
            sfx.enabled = true;
        }else if (outlineEffectableAmount > 0 && !bac.immuneSystem.stats.conditionals["outlineFXED"]){
            ofx.enabled = true;
        }else if (colorEffectableAmount > 0 && !bac.immuneSystem.stats.conditionals["colorFXED"]) {
            cfx.enabled = true;
        }
        outlineEffectableAmount = 0;
        colorEffectableAmount = 0;
        surfaceEffectableAmount = 0;
        return;
            
        
    }
}
