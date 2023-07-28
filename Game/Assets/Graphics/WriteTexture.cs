using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WriteTexture : MonoBehaviour
{
    public RenderTexture texture;

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        Graphics.CopyTexture(destination, texture);
    }
}
