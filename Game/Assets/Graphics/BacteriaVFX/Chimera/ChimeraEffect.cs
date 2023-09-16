using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChimeraEffect : MonoBehaviour
{
    public Material chimMat;
    public OnInterval startInterval;
    public OnInterval endInterval;
    private Vector2 offset1;
    private Vector2 offset2;

    private float rate = 0.01f;

    private void OnEnable()
    {
        StartEffect();
    }

    public void StartEffect()
    {
        offset1 = Vector2.zero;
        offset2 = Vector2.zero;
        if (!startInterval.enabled)
        {
            startInterval.enabled = true;
            startInterval.Resume();
        }
    }

    public void EndEffect()
    {
        offset1 = Vector2.zero;
        offset2 = Vector2.zero;
        endInterval.enabled = true;
    }

    private void LateUpdate()
    {
        offset1.x += rate;
        offset1.y += rate;
        offset2.x -= rate;
        offset2.y -= rate;
        chimMat.SetTextureOffset("_NoiseTex1", offset1);
        chimMat.SetTextureOffset("_NoiseTex2", offset2);

        if (startInterval.enabled && startInterval.isPlaying)
        {
            chimMat.SetFloat("_EffectT", Mathf.Abs(startInterval.t - 1));
            chimMat.SetFloat("_Saturation", Mathf.Abs(startInterval.t - 1));
        }
        if(endInterval.enabled)
        {
            chimMat.SetFloat("_EffectT", endInterval.t);
            chimMat.SetFloat("_Saturation", endInterval.t);
        }
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.Blit(source, destination,chimMat);
    }
}
