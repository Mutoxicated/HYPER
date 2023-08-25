using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnEnable : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;

    private void OnEnable()
    {
        ps.Play();
    }
}
