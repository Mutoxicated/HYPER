using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SuperPassive : MonoBehaviour
{
    [SerializeField,Range(0,16)] private int index;
    
    public delegate void Sub(int num);
    public List<Sub> subs = new List<Sub>();

    private int iterations = 0;
    private bool develop = false;

    public int GetIndex(){
        return index;
    }

    public void SetDevelopState(bool state){
        develop = state;
    }

    public bool GetDevelopState(){
        return develop;
    }

    public void Iterate(int num){
        NotifySubs(num);
        iterations += num;
    }

    public void SetIteration(int num){
        int diff = num-iterations;
        NotifySubs(diff);
        iterations = num;
    }

    public void SetIteration(float num){
        int diff = Mathf.RoundToInt(num)-iterations;
        NotifySubs(diff);
        iterations = Mathf.RoundToInt(num);
    }

    public int GetIterations(){
        return iterations;
    }

    private void Develop(){
        Debug.Log(gameObject.name+" DEVELOPED!!");
        iterations++;
    }

    private void NotifySubs(int num){
        foreach (Sub sub in subs){
            sub.Invoke(num);
        }
    }
}
