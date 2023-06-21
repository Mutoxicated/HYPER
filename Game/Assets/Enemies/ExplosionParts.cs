using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ExplosionParts : MonoBehaviour
{
    [SerializeField] private Rigidbody[] parts;
    private List<Material> materials = new List<Material>();

    [SerializeField] private float speed;
    [SerializeField] private Gradient color;
    [SerializeField,Range(0.005f,0.05f)] private float rate;

    private bool exploding = false;
    private int index = 0;
    private float t = 1f;

    private void Start()
    {
        foreach (var part in parts)
        {
            materials.Add(part.gameObject.GetComponent<Renderer>().material);
        }
    }

    private void Update()
    {
        if (!exploding)
            return;
        t -= rate;
        if (t <= 0f)
        {
            Destroy(gameObject);
        }
        foreach (var mat in materials)
        {
            mat.SetColor("_Color", color.Evaluate(t));
        }
    }

    public void ExplodeParts(Vector3 explosionPoint)
    {
        Debug.Log("ha");
        foreach (var part in parts)
        {
            var direction = part.transform.position - explosionPoint;
            part.gameObject.SetActive(true);
            part.AddForce(direction * speed, ForceMode.Impulse);
        }
        exploding = true;
    }
}
