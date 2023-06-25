using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParts : MonoBehaviour
{
    [SerializeField] private Rigidbody[] parts;
    private List<Material> materials = new List<Material>();

    [SerializeField] private float speedMin;
    [SerializeField] private float speedMax;
    [SerializeField] private Gradient color;
    [SerializeField,Range(0.005f,0.05f)] private float rate;

    private bool exploding = false;
    private float t = 1f;

    private void Start()
    {
        foreach (var part in parts)
        {
            materials.Add(part.gameObject.GetComponent<Renderer>().material);
        }
    }

    private void FixedUpdate()
    {
        if (!exploding)
            return;
        t -= rate;
        foreach (var mat in materials)
        {
            mat.SetColor("_Color", color.Evaluate(t));
        }
        if (t <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void ExplodeParts(Vector3 explosionPoint)
    {
        foreach (var part in parts)
        {
            var direction = part.transform.position - explosionPoint;
            part.gameObject.SetActive(true);
            part.AddForce(direction * Random.Range(speedMin,speedMax), ForceMode.Impulse);
        }
        exploding = true;
    }
}
