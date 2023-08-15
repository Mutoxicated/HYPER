using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private static List<CameraShake> cameraShakes = new List<CameraShake>();
    private static GameObject player;
    [Range(0,5),Tooltip("Lowest -> Highest")] public int priority;
    [SerializeField] private float maxDistance = 25f;
    [SerializeField] private bool shakeOnEnable = false;
    public GameObject _cam;
    public int times = 12;
    public float strength = 0.7f;
    public float interval = 0.02f;

    [HideInInspector] public bool shaking;
    private float actualStrength;
    private int currentAmount;
    private float t;
    private float distanceT;
    private Vector3 initialPos;

    public void Shake()
    {
        foreach (var cameraShake in cameraShakes)
        {
            if (cameraShake.shaking)
            {
                if (cameraShake.priority > priority)
                    return;
            }
        }
        initialPos = _cam.transform.localPosition;
        t = 0;
        currentAmount = 0;
        shaking = true;
        var distance = Vector3.Distance(gameObject.transform.position, player.transform.position);
        distanceT = Mathf.Clamp01(Mathf.Abs((distance/maxDistance)-1));
        actualStrength = strength * distanceT;
    }

    private void GetEssentials()
    {
        if (player == null)
        {
            player = GameObject.FindWithTag("Player");
        }
        if (_cam == null)
        {
            _cam = Camera.main.transform.parent.gameObject;
        }
    }

    private void OnEnable()
    {
        if (!shakeOnEnable)
            return;
        GetEssentials();
        Shake();
    }

    private void Start()
    {
        cameraShakes.Add(this);
        GetEssentials();
    }

    private void Update()
    {
        if (!shaking) return;
        t += Time.deltaTime;
        if (t >= interval || currentAmount == 0)
        {
            _cam.transform.localPosition = new Vector3(initialPos.x+
            Random.Range(-actualStrength, actualStrength),
            initialPos.y+
            Random.Range(-actualStrength, actualStrength),
            0f);
            currentAmount++;
        }
        if (currentAmount >= times)
        {
            shaking = false;
            _cam.transform.localPosition = initialPos;
        }
    }
}
