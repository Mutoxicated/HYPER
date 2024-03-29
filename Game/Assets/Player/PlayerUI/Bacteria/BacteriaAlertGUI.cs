using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BacteriaAlertGUI : MonoBehaviour
{
    [SerializeField] private Feed feed;
    [SerializeField] private Transform[] positivePlacements;
    [SerializeField] private Transform[] negativePlacements;
    [SerializeField] private BacElement[] bacels;

    private Dictionary<string,BacElement> currentPositives = new Dictionary<string,BacElement>();
    private Dictionary<string,BacElement> currentNegatives = new Dictionary<string,BacElement>();

    private BacElement currentBacel;
    private int index = 0;

    private BacElement GetBacelByBac(Bacteria bac){
        foreach (BacElement bacel in bacels){
            if (bacel.gameObject.name == bac.gameObject.name)
                return bacel;
        }
        return null;
    }

    private bool AlreadyExistsInDict(Dictionary<string,BacElement> dict, BacElement bacel){
        return dict.ContainsKey(bacel.gameObject.name); 
    }

    private void DressDictionaries(){
        index = 0;
        foreach (BacElement bacel in currentNegatives.Values){
            bacel.transform.position = negativePlacements[index].position;
            index++;
        }
        index = 0;
        foreach (BacElement bacel in currentPositives.Values){
            bacel.transform.position = positivePlacements[index].position;
            index++;
        }
    }

    private void AddNewBacel(Dictionary<string,BacElement> dictToAdd, BacElement bacel, int totalPopulation){
        if (totalPopulation == -1) return;
        bacel.SetPopulationAbsolute(totalPopulation);
        dictToAdd.Add(bacel.gameObject.name, bacel);
        bacel.gameObject.SetActive(true);
    }

    private void ModifyBacel(Dictionary<string,BacElement> dict, BacElement bacel, int totalPopulation){
        if (totalPopulation == -1){
            dict[bacel.gameObject.name].SetPopulationAbsolute(0);
            dict[bacel.gameObject.name].gameObject.SetActive(false);
            dict.Remove(bacel.gameObject.name);
            return;
        }
        dict[bacel.gameObject.name].SetPopulationAbsolute(totalPopulation);
    }

    private void AddToCurrents(Dictionary<string,BacElement> dictToAdd, Bacteria bac, int totalPopulation){
        currentBacel = GetBacelByBac(bac);
        if (AlreadyExistsInDict(dictToAdd,currentBacel)){
            ModifyBacel(dictToAdd,currentBacel,totalPopulation);
        }else{
            AddNewBacel(dictToAdd,currentBacel,totalPopulation);
        }
    }

    public void AlertNew(Bacteria bac){
        if (bac.immunitySide == ImmunitySide.INVADER){
                feed.Message(rtColorToHex(bac.ID.color)+bac.ID.type.ToString()+"<color=#FFFFFF> invaded!");
            }else{
                feed.Message(rtColorToHex(bac.ID.color)+bac.ID.type.ToString()+"<color=#FFFFFF> allied!");
        }
    }

    public void AlertSystem(Bacteria bac, int population){
        if (bac.ID.character == BacteriaCharacter.NEGATIVE){
            AddToCurrents(currentNegatives,bac,population);
        }else{
            AddToCurrents(currentPositives,bac,population);
        }
        DressDictionaries();
    }

    public string DecToHex(int value){
        return value.ToString("X2");
    }

    public string floatNormToHex(float value){
        return DecToHex(Mathf.RoundToInt(value*255f));
    }

    public string rtColorToHex(Color color){
        string r = floatNormToHex(color.r);
        string g = floatNormToHex(color.g);
        string b = floatNormToHex(color.b);
        return "<color=#"+r+g+b+">";
    }
}
