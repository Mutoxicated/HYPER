using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraBehaviour : MonoBehaviour//was made to provide different ways of dealing with UnityEvents in the editor
{
    public void EnabledOpposite(bool state){
        enabled = !state;
    }

    public void OnlyTrueEnabledOpposite(bool state){
        if (state) enabled = !state;
    }

    public void OnlyFalseEnabledOpposite(bool state){
        if (!state) enabled = !state;
    }

    public void OnlyTrueEnabled(bool state){
        if (state) enabled = state;
    }

    public void OnlyFalseEnabled(bool state){
        if (!state) enabled = state;
    }
}
