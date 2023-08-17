using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class ParticlePixelize : MonoBehaviour
{
    [SerializeField,ColorUsage(true,true)] private Color color;
    private ParticleSystemRenderer particleRenderer;


    private void Start()
    {
        particleRenderer = GetComponent<ParticleSystemRenderer>();
        particleRenderer.material.color = color;
    }
}
