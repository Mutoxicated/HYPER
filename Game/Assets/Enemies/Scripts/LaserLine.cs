using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLine : MonoBehaviour
{
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private Transform looker;

    private RaycastHit hitInfo;

    private void Update()
    {
        Physics.Raycast(looker.position, looker.eulerAngles, out hitInfo, 100f);
        lineRenderer.SetPosition(1, hitInfo.point);
    }
}
