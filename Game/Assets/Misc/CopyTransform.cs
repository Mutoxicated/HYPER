using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private bool copyPosition;
    [SerializeField] private bool copyRotation;

    void Update()
    {
        if (_transform == null)
        {
            Debug.Log("ERROR! Transform has become null, possibly due to being destroyed. Disabling component.");
            this.enabled = false;
            return;
        }
        if (copyPosition)
            transform.position = _transform.position;
        if (copyRotation)
            transform.rotation = _transform.rotation;
    }
}
