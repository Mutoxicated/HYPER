using System.Linq;
using UnityEngine;

public class applyWire : MonoBehaviour
{

    [SerializeField]
    public Color outlineColor = Color.white;

    public void ChangeColor(Color color)
    {
        outlineColor = color;
        wireframeMat.SetColor("_WireframeBackColour", outlineColor);
    }

    private Renderer[] renderers;
    private Material wireframeMat;

    void Awake()
    {
        // Cache renderers
        renderers = GetComponentsInChildren<Renderer>();

        // Instantiate outline materials
        wireframeMat = Instantiate(Resources.Load<Material>(@"Materials/wireframeMat"));

        wireframeMat.name = "OutlineFill (Instance)";

        wireframeMat.SetColor("_WireframeBackColour", outlineColor);
    }

    void OnEnable()
    {
        foreach (var renderer in renderers)
        {

            // Append outline shaders
            var materials = renderer.sharedMaterials.ToList();

            materials.Add(wireframeMat);

            renderer.materials = materials.ToArray();
        }
    }

    private void OnDestroy()
    {
        Destroy(wireframeMat);
    }
}
