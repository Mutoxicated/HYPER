using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private AudioSource uponEnable;
    [SerializeField] private AudioSource uponStart;

    private void Start()
    {
        if (uponStart == null)
            return;
        uponStart.Play();
    }

    private void OnEnable()
    {
        if (uponEnable == null)
            return;
        uponEnable.Play();
    }
}
