using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionParts : MonoBehaviour
{
    [SerializeField] private Rigidbody[] parts;
    private List<Material> materials = new List<Material>();

    [SerializeField] private float speedMin;
    [SerializeField] private float speedMax;
    [SerializeField, GradientUsage(true)] private Gradient color;
    [SerializeField,Range(0.5f,2f)] private float rate;

    private bool exploding = false;
    private float t = 1f;
    private MaterialColorChannel colorChannel;

    private void Start()
    {
        colorChannel = (MaterialColorChannel)1;
        foreach (var part in parts)
        {
            materials.Add(part.gameObject.GetComponent<Renderer>().material);
        }
    }

    private void FixedUpdate()
    {
        if (!exploding)
            return;
        t -= rate*Time.fixedDeltaTime;
        foreach (var mat in materials)
        {
            mat.SetColor(colorChannel.ToString(), color.Evaluate(t));
        }
        if (t <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void ExplodeParts(Transform explosionPoint)
    {
        if (explosionPoint == null)
        {
            explosionPoint = transform;
        }
        foreach (var part in parts)
        {
            var direction = part.transform.position - explosionPoint.position;
            part.gameObject.SetActive(true);
            part.AddForce(direction * Random.Range(speedMin,speedMax), ForceMode.Impulse);
        }
        exploding = true;
    }
}
