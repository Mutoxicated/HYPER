using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    private static Difficulty diff;
    public GameObject sequencePrefab;

    private bool finished = false;
    private Sequence sequenceInfo;

    private void Start()
    {
        StartSequence();
        if (diff == null)
            diff = GameObject.FindWithTag("Difficulty").GetComponent<Difficulty>();
    }

    private void Update()
    {
        
    }

    public void StartSequence()
    {
        NextSequence();
    }

    public void NextSequence()
    {
        var instance = Instantiate(sequencePrefab);
        instance.transform.SetParent(transform);
        sequenceInfo = instance.GetComponent<Sequence>();
    }

    public bool isFinished()
    {
        return finished;
    }
}
