using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateGUIElements : MonoBehaviour
{
    [SerializeField] private TMP_Text roundText;

    private void Start(){
        if (RunDataSave.rData.rounds == 0){
            RunDataSave.rData.rounds = Difficulty.rounds;
        }else{
            Difficulty.rounds = RunDataSave.rData.rounds;
        }
        roundText.text = Difficulty.rounds.ToString();
    }
}
