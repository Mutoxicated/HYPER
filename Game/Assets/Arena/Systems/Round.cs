using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Round : MonoBehaviour
{
    private static Difficulty diff;
    private static int rounds = 0;

    public GameObject wavePrefab;
    public GameObject beamInstance;
    public TMP_Text wavesText;
    public TMP_Text roundText;

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
        rounds++;
        roundText.text = rounds.ToString();
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
        beamInstance.SetActive(true);
        Sequence.RemoveSpawnPoints();
        Destroy(gameObject);
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
            if (waveInfo != null)
                waveInfo.EndWave();
            var instance = Instantiate(wavePrefab);
            instance.transform.SetParent(transform);
            waveInfo = instance.GetComponent<Wave>();
            waves++;
        }
        wavesText.text = waves.ToString();
    }
}
