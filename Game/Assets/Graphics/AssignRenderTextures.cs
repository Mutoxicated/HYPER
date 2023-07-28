using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignRenderTextures : MonoBehaviour
{
    public Shader shader;
    public RenderTexture[] textures;
    private Material mat;

    private void OnEnable()
    {
        mat ??= new Material(shader);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        foreach (var tex in textures)
        {
            Graphics.Blit(tex, source);
        }
        Graphics.Blit(source, destination);
    }
}
