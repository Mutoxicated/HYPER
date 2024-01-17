using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ButtonColor : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private Gradient gradient;
    [SerializeField,Range(0.01f,30f)] private float rate;

    private float t;
    private bool hovering;

    private void Update(){
        if (hovering){
            t += rate * Time.unscaledDeltaTime;
        }else{
            t -= rate * Time.unscaledDeltaTime;
        }
        t = Mathf.Clamp01(t);
        image.color = gradient.Evaluate(t);
    }

    public void SetHoveringState(bool state){
        hovering = state;
    }
}
