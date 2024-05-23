using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ProTips : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private string[] tips = {
        "Use your gun to shoot!"
    };

    private void OnEnable(){
        text.text = "Pro Tip: "+tips[Random.Range(0,tips.Length)];
    }
}
