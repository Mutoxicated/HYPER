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
            for (int i = 0; i < bac.immuneSystem.injector.bacteriaPools.Count; i++){
            if (bac.type.ToString() == bac.immuneSystem.injector.bacteriaPools[i]){
                if (i == 0){
                    sfx.enabled = true;
                }else if (i == 1){
                    ofx.enabled = true;
                }else{
                    cfx.enabled = true;
                }
            }/*else{
                Debug.Log("fail! at: "+i+" of "+ bac.immuneSystem.gameObject.name);
            }*/
        }
    }
}
