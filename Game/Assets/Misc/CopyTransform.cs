using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private bool copyPosition;
    [SerializeField] private bool copyRotation;
    [Space]
    [Header("Extra")]
    [SerializeField] private Vector3 positionMultiplier = Vector3.one;
    [SerializeField] private Vector3 rotationMultiplier = Vector3.one;
    [Space]
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Quaternion rotationOffset;

    private Quaternion alteredRotation;
    private Vector3 alteredPosition;

    void LateUpdate()
    {
        if (_transform == null)
        {
            Debug.Log("ERROR! Transform has become null, possibly due to being destroyed. Disabling component.");
            enabled = false;
            return;
        }
        if (copyPosition)
        {
            alteredPosition = _transform.position;
            alteredPosition.x *= positionMultiplier.x;
            alteredPosition.y *= positionMultiplier.y;
            alteredPosition.z *= positionMultiplier.z;
            alteredPosition += positionOffset;
            transform.position = alteredPosition;
        }
        if (copyRotation)
        {
            alteredRotation = _transform.rotation;
            if ((rotationOffset.x+rotationOffset.y+rotationOffset.z) > 0f)
                alteredRotation = alteredRotation * rotationOffset;
            transform.rotation = alteredRotation;
        }
    }
}
