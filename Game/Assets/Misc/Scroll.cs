using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scroll 
{
    public int index;
    private int maxIndex;

    public Scroll(int index, int maxIndex)
    {
        this.index = index;
        this.maxIndex = maxIndex;
    }

    public void Increase()
    {
        if (index >= maxIndex)
        {
            index = 0;
        }else
        {
            index++;
        }
    }

    public void Decrease()
    {
        if (index <= 0)
        {
            index = maxIndex;
        }
        else
        {
            index--;
        }
    }

    public int GetMaxIndex(){
        return maxIndex;
    }

    public void AlterMaxIndex(int maxI)
    {
        maxIndex = maxI;
        if (index > maxI) {
            index = maxI;
        }
    }

    public void AlterIndex(int I)
    {
        if (I > maxIndex){
            index = maxIndex;
            return;
        }
        index = I;
    }
}
