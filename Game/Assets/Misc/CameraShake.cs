using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public GameObject _cam;
    public int times;
    public float strength;
    public float interval;

    private int currentAmount;
    private float t;
    private Vector3 initialPos;
    private bool shaking = false;

    public void Shake()
    {
        initialPos = _cam.transform.localPosition;
        t = 0;
        currentAmount = 0;
        shaking = true;
    }

    private void Update()
    {
        if (!shaking) return;
        t += Time.deltaTime;
        if (t >= interval || currentAmount == 0)
        {
            _cam.transform.localPosition = new Vector3(initialPos.x+
            Random.Range(-strength, strength),
            initialPos.y+
            Random.Range(-strength, strength),
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
