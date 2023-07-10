using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignToBaseRender : MonoBehaviour
{
    [SerializeField] private Shader conflateShader;
    [SerializeField] private RenderTexture[] textures;
    private RenderTexture finalTex;
    private Material mat;

    private void Start()
    {
        mat  = new Material(conflateShader);
        finalTex = new RenderTexture(textures[0]);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        foreach (var tex in textures)
        {
            mat.SetTexture("_Tex", tex);
            Graphics.Blit(tex, destination, mat);
        }
        //Graphics.Blit(finalTex, destination);
    }
}
