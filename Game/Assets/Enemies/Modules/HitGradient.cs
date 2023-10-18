using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitGradient : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;
    [SerializeField,ColorUsage(true,true)] private Color startColor;
    [SerializeField,ColorUsage(true,true)] private Color endColor;
    [SerializeField] private int matIndex = 1;

    private Material mat;
    [HideInInspector] public Color color;

    public void ChangeStartColor(Color color){
        startColor = color;
    }

    public Color GetStartColor(){
        return startColor;
    }

    private void OnEnable()
    {
        color = Color.Lerp(startColor,endColor,health.t);
    }

    private void Start()
    {
        mat = GetComponent<Renderer>().materials[matIndex];
    }

    private void Update()
    {
        if (health.immune)
            return;
        //Debug.Log(health.t);
        color = Color.Lerp(startColor,endColor,health.t);
        mat.color = color;
    }
}
