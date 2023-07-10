using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignToBaseRender : MonoBehaviour
{
    [SerializeField] private RenderTexture[] renderTextures;
    [SerializeField] private Material[] materials;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        for (int i = 0; i < renderTextures.Length; i++)
        {
            Graphics.Blit(renderTextures[i],destination,materials[i]);
        }
    }
}
