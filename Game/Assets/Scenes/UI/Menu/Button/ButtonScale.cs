using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScale : MonoBehaviour
{
    [SerializeField] private float lerpSpeed;
    [SerializeField] private float modifier;

    private Vector3 initScale;
    private Vector3 maxScale;

    private bool hovering = false;

    private void Start(){
        initScale = transform.localScale;
        maxScale = initScale*modifier;
    }

    private void Update(){
        if (hovering){
            transform.localScale = Vector3.Lerp(transform.localScale,maxScale,Time.unscaledDeltaTime*lerpSpeed);
        }else{
            transform.localScale = Vector3.Lerp(transform.localScale,initScale,Time.unscaledDeltaTime*lerpSpeed);
        }
    }

    public void SetHovering(bool state){
        hovering = state;
    }
}
