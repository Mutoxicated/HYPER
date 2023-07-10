using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float moveSpeedMod = 1f;
    public float attackSpeedMod = 1f;
    public float defenseMod = 1f;

    private float[] incrementalBuffs;//buffs that benefit the player/enemy when you increment the modifier
    private float[] decrementalBuffs;//buffs that benefit the player/enemy when you decrement the modifier

    private void Start()
    {
        incrementalBuffs = new float[]
        {
            moveSpeedMod
        };
        decrementalBuffs = new float[]
        {
            defenseMod,
            attackSpeedMod
        };
    }

    public void ModifyAllStats(float value, float duration)
    {

    }

    public void ModifyStat(string name, float value, float duration)
    {

    }

    public void RevertStats()
    {
        //reverts back to one on all stats
    }

    public void RevertStat(string name)
    {
        
    }
}
