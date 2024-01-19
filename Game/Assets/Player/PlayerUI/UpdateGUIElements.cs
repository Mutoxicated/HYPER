using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateGUIElements : MonoBehaviour
{
    [SerializeField] private TMP_Text roundText;

    private void Start(){
        roundText.text = Difficulty.rounds.ToString();
    }
}
