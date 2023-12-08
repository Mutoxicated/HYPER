using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ExplosionParts : MonoBehaviour
{
    [SerializeField] private Rigidbody[] parts;
    private List<Material> materials = new List<Material>();

    [SerializeField] private float speedMin;
    [SerializeField] private float speedMax;
    [SerializeField] private Renderer render;
    [SerializeField] private int index = 0;
    [SerializeField,Range(0.5f,2f)] private float rate;

    private bool exploding = false;
    private float t = 0f;
    private Color cachedColor;
    private Color nothing = new Color(0f,0f,0f,0f);

    private List<Vector3> initialPositions = new List<Vector3>();
    private List<Quaternion> initialRotations = new List<Quaternion>();
    public UnityEvent OnFinished = new UnityEvent();

    private void Start()
    {
        foreach (var part in parts)
        {
            materials.Add(part.gameObject.GetComponent<Renderer>().material);
        }
    }

    public void ChangeColorIndex(int index)
    {
        this.index = index;
    }

    private void Update()
    {
        if (!exploding)
            return;
        t += rate*Time.deltaTime;
        foreach (var mat in materials)
        {
            mat.color = Color.Lerp(cachedColor,nothing,t);
        }
        if (t >= 1f)
        {
            for (int i = 0; i < parts.Length; i++)
            {
                OnFinished.Invoke();
                parts[i].gameObject.SetActive(false);
                parts[i].velocity = Vector3.zero;
                parts[i].transform.localPosition = initialPositions[i];
                parts[i].transform.localRotation = initialRotations[i];
                t = 0f;
                exploding = false;
            }
        }
    }

    public void ExplodeParts(Transform explosionPoint)
    {
        cachedColor = render.material.color;
        if (explosionPoint == null)
        {
            explosionPoint = transform;
        }
        foreach (var part in parts)
        {
            initialPositions.Add(part.transform.localPosition);
            initialRotations.Add(part.transform.localRotation);
            var direction = part.transform.position - explosionPoint.position;
            part.gameObject.SetActive(true);
            part.AddForce(direction * Random.Range(speedMin,speedMax), ForceMode.Impulse);
        }
        exploding = true;
    }

    public void Die(){
        Destroy(gameObject);
    }
}
