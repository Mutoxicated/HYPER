using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chimera : MonoBehaviour
{
    private static ChimeraEffect chimera;
    public Bacteria bacteria;
    private static List<Chimera> chimeras = new List<Chimera>();

    private void OnEnable()
    {
        if (chimera == null)
        {
            chimera = GameObject.FindWithTag("MainCamera").GetComponent<ChimeraEffect>();
        }
        chimeras.Add(this);
        chimera.enabled = true;
    }

    public void EndChimera()
    {
        if (chimeras.Count == 0)
        {
            chimera.EndEffect();
            return;
        }
        foreach (var chimera in chimeras)
        {
            if (chimera.bacteria.lifeSpan > bacteria.lifeSpan)
            {
                return;
            }
        }
        chimera.EndEffect();
    }

    private void OnDisable()
    {
        EndChimera();
        chimeras.Remove(this);
    }
}
