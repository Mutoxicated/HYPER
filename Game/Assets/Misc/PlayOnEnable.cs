using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayOnEnable : MonoBehaviour
{
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private AudioSource _as;

    private void OnEnable()
    {
        if (ps != null)
            ps.Play();
        if (_as != null)
            _as.Play();
    }
}
