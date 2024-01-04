using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShowPlayerMoney : MonoBehaviour
{
    [SerializeField] private TMP_Text txt;
    
    void Update()
    {
        txt.text = PlayerInfo.GetMoney().ToString()+"*";
    }
}
