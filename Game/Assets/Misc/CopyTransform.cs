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
        if (copyPosition)
            transform.position = _transform.position;
        if (copyRotation)
            transform.rotation = _transform.rotation;
    }
}
