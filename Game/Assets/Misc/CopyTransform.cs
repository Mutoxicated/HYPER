using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyTransform : MonoBehaviour
{
    [SerializeField] private Transform _transform;
    [SerializeField] private bool copyPosition;
    [SerializeField] private bool copyRotation;
    [Space]
    [SerializeField] private Vector3 positionMultiplier = Vector3.one;
    [SerializeField] private Vector3 rotationMultiplier = Vector3.one;
    [Space]
    [SerializeField] private Vector3 positionOffset;

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
            alteredRotation = Quaternion.Euler(
                alteredRotation.eulerAngles.x * rotationMultiplier.x,
                alteredRotation.eulerAngles.y * rotationMultiplier.y,
                alteredRotation.eulerAngles.z * rotationMultiplier.z);
            transform.rotation = alteredRotation;
        }
    }
}
