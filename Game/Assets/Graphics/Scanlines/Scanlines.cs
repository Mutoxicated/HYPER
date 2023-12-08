using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanlines : MonoBehaviour
{
    [SerializeField] private Shader shader;
	private Material material;
	[Range(0.001f, 10.0f)]
	public float lineSize = 1.0f;
    public int modulo = 2;

	public void Start() {
		material = new Material(shader);
		material.SetFloat("_LineSize", lineSize);
        material.SetInt("_Modulo", modulo);
	}

	public void OnRenderImage(RenderTexture inTexture, RenderTexture outTexture) {
        Graphics.Blit(inTexture, outTexture, material);
	}
}
