using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private static Difficulty diff;
    public GameObject sequencePrefab;

    private bool finished = false;
    private int sequences = 0;
    private Sequence sequenceInfo;

    private void Start()
    {
        StartSequence();
        if (diff == null)
            diff = GameObject.FindWithTag("Difficulty").GetComponent<Difficulty>();
    }

    private void Update()
    {
        if (finished)
            return;
        if (sequenceInfo.EnemiesLeft() == 0 && sequenceInfo.SequenceEnded())
        {
            if (sequences >= diff.sequencePopulation)
            {
                finished = true;
                sequenceInfo.EndSequence();
                return;
            }
            Debug.Log("ha");
            NextSequence();
        }
    }

    public void StartSequence()
    {
        NextSequence();
    }

    public void NextSequence()
    {
        sequences++;
        if (sequenceInfo != null)
            sequenceInfo.EndSequence();
        var instance = Instantiate(sequencePrefab);
        instance.transform.SetParent(transform);
        sequenceInfo = instance.GetComponent<Sequence>();
    }

    public void EndWave()
    {
        Destroy(gameObject);
    }

    public bool isFinished()
    {
        return finished;
    }
}
