using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tremble : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxRadnomness;
    [SerializeField] private Vector3 randomMultiplier = Vector3.one;
    [SerializeField,Range(0f,1f)] private float interval = 0.4f;

    private float time = 0f;
    private bool setBack = false;
    private Vector3 position;
    private Vector3 alteredPosition;

    private void Start(){
        position = transform.localPosition;
    }

    private void Update(){
        time += Time.deltaTime;
        if (time >= interval){
            time = 0f;
            if (!setBack){
                setBack = true;
                alteredPosition = transform.localPosition;
                alteredPosition.x += Random.Range(minMaxRadnomness.x,minMaxRadnomness.y)*randomMultiplier.x;
                alteredPosition.y += Random.Range(minMaxRadnomness.x,minMaxRadnomness.y)*randomMultiplier.y;
                alteredPosition.z += Random.Range(minMaxRadnomness.x,minMaxRadnomness.y)*randomMultiplier.z;
            }else{
                alteredPosition = position;
                setBack = false;
            }
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition,alteredPosition,Time.deltaTime*70f);
    }
}
