using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ManageLine : MonoBehaviour
{
    [SerializeField] private Gradient[] gradient;
    [SerializeField] private int index = 0;

    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private TrailRenderer trailRenderer;

    [SerializeField] private bool dontUpdate = false;

    private int previousindex = 90;

    public void ChangeColorIndex(int i)
    {
        index = i;
    }
    private void Start()
    {
        if (trailRenderer == null){
            lineRenderer.enabled = true;
        }
        else{
            trailRenderer.enabled = true;
        }
    }

    private void Update()
    {
        if (dontUpdate)
            return;
        if (index != previousindex)
        {
            if (trailRenderer == null)
            {
                var copyGradient = lineRenderer.colorGradient;
                copyGradient.SetKeys(gradient[index].colorKeys, gradient[index].alphaKeys);
                lineRenderer.colorGradient = copyGradient;
            }
            else
            {
                var copyGradient = trailRenderer.colorGradient;
                copyGradient.SetKeys(gradient[index].colorKeys, gradient[index].alphaKeys);
                trailRenderer.colorGradient = copyGradient;
            }
            previousindex = index;
        }
    }
}
