using System.Collections.Generic;
using UnityEngine;
using static Conditional;

public class ColorEffect : MonoBehaviour
{
    [SerializeField] private Bacteria bac;
    [SerializeField,GradientUsage(true)] private Gradient gradient;
    private List<Color> previousColor = new List<Color>();
    [SerializeField] private OnInterval interval;
    [ColorUsage(true,true)] private Color color;

    private List<Material> mats = new List<Material>();
    
    private void EvaluateColor(){
        if (interval != null){
            color = gradient.Evaluate(interval.t);
        }else{
            color = gradient.Evaluate(0);
        }
        foreach(var part in bac.immuneSystem.injector.bodyParts){
            if (!part.colorEffectable)
                continue;
            if (part.hitGradient != null){
                part.hitGradient.ChangeStartColor(color);
            }
            else{
                part._renderer.materials[part.matIndex].color = color;
            }
        }
    }

    private void AddEffect(){
        if (interval != null)
            interval.enabled = true;
        if (interval != null){
            color = gradient.Evaluate(interval.t);
        }else{
            color = gradient.Evaluate(0);
        }
        bac.immuneSystem.stats.conditionals[COLOR_FXED] = false;
        foreach(var part in bac.immuneSystem.injector.bodyParts){
            if (!part.colorEffectable){
                mats.Add(part._renderer.materials[part.matIndex]);//we're not gonna be accessing this, just have to add this so that the revert function works
                previousColor.Add(Color.white);
                continue;
            }
            mats.Add(part._renderer.materials[part.matIndex]);
            if (part.hitGradient != null){
                previousColor.Add(part.hitGradient.GetStartColor());
                part.hitGradient.ChangeStartColor(color);
            }
            else{
                previousColor.Add(part._renderer.materials[part.matIndex].color);
                part._renderer.materials[part.matIndex].color = color;
            }
        }
    }

    private void RevertEffect(){
        if (interval != null)
            interval.enabled = false;
        bac.immuneSystem.stats.conditionals[COLOR_FXED] = false;
        for (int i = 0; i < bac.immuneSystem.injector.bodyParts.Count; i++){
            if (!bac.immuneSystem.injector.bodyParts[i].colorEffectable)
                continue;
            if (bac.immuneSystem.injector.bodyParts[i].hitGradient != null){
                bac.immuneSystem.injector.bodyParts[i].hitGradient.ChangeStartColor(previousColor[i]);
            }
            else{
                mats[i].color = previousColor[i];
            }
        }
        previousColor.Clear();
        enabled = false;//Will look into why i did this later
    }
    
    //Felt a lil goofy with these lambda expressions B)

    private void LateUpdate() => EvaluateColor();

    private void OnEnable() => AddEffect();

    private void OnDisable() => RevertEffect();
}
