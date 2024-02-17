using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BacElement : MonoBehaviour
{
    public Image image;
    public TMP_Text populous;

    private int population;

    public void SetPopulation(int num){
        population += num;

        populous.text = "x"+population;
    }

    public void SetPopulationAbsolute(int num){
        population = num;

        populous.text = "x"+population;
    }
}
