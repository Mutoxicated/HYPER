using UnityEngine;
using System.Collections;

public class ChromaAbb : MonoBehaviour {
	[SerializeField] private Shader shader;
	private Material material;
	[Range(0.0f, 1.0f)]
	public float chromaticAberration = 1.0f;

	public void Start() {
		material = new Material(shader);
		material.SetFloat("_ChromaticAberration", 0.01f * chromaticAberration);
	}

	public void OnRenderImage(RenderTexture inTexture, RenderTexture outTexture) {
    	Graphics.Blit(inTexture, outTexture, material);
	}
}