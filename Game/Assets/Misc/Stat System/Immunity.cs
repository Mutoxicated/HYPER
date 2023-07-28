using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EntityType
{
    ORGANIC,
    NON_ORGANIC
}

public class Immunity : MonoBehaviour
{
    public EntityType type;
    public float immunityAttackRate = 1f;
    public float immunityDamage = 10f;

    private List<Bacteria> foreignBacteria = new List<Bacteria>();
    private float t;

    public void NotifySystem(Bacteria bacteria)
    {
        Debug.Log("notified!");
        if (type == EntityType.ORGANIC)
        {
            if (bacteria.type == BacteriaType.RAIN)
            {
                Destroy(bacteria);
                return;
            }
        }
        else
        {
            if (bacteria.type == BacteriaType.POISON)
            {
                Destroy(bacteria);
                return;
            }
        }
        foreignBacteria.Add(bacteria);
    }

    private void Update()
    {
        if (foreignBacteria.Count == 0)
            return;
        t += Time.deltaTime;
        if (t >= immunityAttackRate)
        {
            t = 0f;
            foreach (Bacteria bacteria in foreignBacteria.ToArray())
            {
                bool died = bacteria.Degrade(immunityDamage);
                if (died)
                {
                    foreignBacteria.Remove(bacteria);
                }
            }
        }
    }
}
