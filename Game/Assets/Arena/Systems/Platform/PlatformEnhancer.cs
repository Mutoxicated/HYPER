using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class PlatformEnhancer : MonoBehaviour
{
    [SerializeField] private static readonly float inc1 = 0.1f;
    [SerializeField] private static readonly float inc2 = 0.05f;
    [SerializeField] private static readonly float inc3 = 0.125f;
    [SerializeField, Range(0f,100f)] private static readonly float chance = 100f;

    [SerializeField] private static float sleepers = 0;
    [SerializeField] private static float trappers = 0;
    [SerializeField] private static float rechargers = 0;
    [SerializeField] private static float simoners = 0;
    [SerializeField] private static float derusters = 1;

    private static int currentsleepers = 0;
    private static int currenttrappers = 0;
    private static int currentrechargers = 0;
    private static int currentsimoners = 0;
    private static int currentderusters = 0;

    private List<PlatformObjective> objectives = new List<PlatformObjective>();

    private void ShuffleObjectives(IList<PlatformObjective> list)
    {
        RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
        int n = list.Count;
        while (n > 1)
        {
            byte[] box = new byte[1];
            do provider.GetBytes(box);
            while (!(box[0] < n * (byte.MaxValue / n)));
            int k = box[0] % n;
            n--;
            PlatformObjective value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public void GetObjective(PlatformObjective objective) {
        objectives.Add(objective);
    }

    private bool GiveObjective(ref int currentOf, PlatformObjectiveType pot, PlatformObjective po){
        if (currentOf <= 0){
            return false;
        }
        currentOf--;
        if (SeedGenerator.random.Next(0,100) <= chance){
            po.SetPot(pot);
        }
        return true;
    }

    public void ClearObjectives(){
        foreach (PlatformObjective po in objectives){
            po.RevertObjective();
        }
    }

    public void ApplyObjectivesToPlatforms(){
        currentderusters = Mathf.RoundToInt(derusters);
        currentrechargers = Mathf.RoundToInt(rechargers);
        currentsimoners = Mathf.RoundToInt(simoners);
        currentsleepers = Mathf.RoundToInt(sleepers);
        currenttrappers = Mathf.RoundToInt(trappers);
        List<PlatformObjective> shuffledObjectives = objectives;
        //Debug.Log("before: "+shuffledObjectives.Count);
        ShuffleObjectives(shuffledObjectives);
        //Debug.Log("after: "+shuffledObjectives.Count);
        foreach (PlatformObjective po in shuffledObjectives){//im a monster
            if (GiveObjective(ref currentderusters,PlatformObjectiveType.DERUST,po))
                continue;
            if (GiveObjective(ref currentrechargers,PlatformObjectiveType.RECHARGE,po))
                continue;
            if (GiveObjective(ref currentsimoners,PlatformObjectiveType.SIMON_SAYS,po))
                continue;
            if (GiveObjective(ref currentsleepers,PlatformObjectiveType.SLEEP,po))
                continue;
            if (GiveObjective(ref currenttrappers,PlatformObjectiveType.TRAP,po))
                continue;
            return;
        }
    }

    public void Progress(){
        sleepers += inc1;
        trappers += inc2;
        rechargers += inc3;
        simoners += inc2;
        derusters += inc1;
    }
}
