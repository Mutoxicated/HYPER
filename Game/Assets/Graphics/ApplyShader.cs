using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyShader : MonoBehaviour
{
    [SerializeField] private Shader shader;
	private Material material;

	public void Start() {
		material = new Material(shader);
	}

	public void OnRenderImage(RenderTexture inTexture, RenderTexture outTexture) {
        Graphics.Blit(inTexture, outTexture, material);
	}
}
