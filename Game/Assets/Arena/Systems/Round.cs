using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Round : MonoBehaviour
{
    private static Difficulty diff;

    public GameObject wavePrefab;
    public GameObject beamInstance;

    private Wave waveInfo;
    private int waves = 0;

    public void StartRound()
    {
        NextWave();
    }

    private void Start()
    {
        if (diff == null)
            diff = GameObject.FindWithTag("Difficulty").GetComponent<Difficulty>();
        StartRound();
    }

    private void Update()
    {
        if (waveInfo.isFinished())
        {
            NextWave();
        }
    }

    public void EndRound()
    {
        Destroy(gameObject);
        Sequence.RemoveSpawnPoints();
        beamInstance.SetActive(true);
    }

    public void NextWave()
    {
        if (waves >= diff.wavePopulation)
        {
            waveInfo.EndWave();
            EndRound();
            return;
        }
        else
        {
            Debug.Log("a");
            if (waveInfo != null)
                waveInfo.EndWave();
            var instance = Instantiate(wavePrefab);
            instance.transform.SetParent(transform);
            waveInfo = instance.GetComponent<Wave>();
            waves++;
        }
    }
}
