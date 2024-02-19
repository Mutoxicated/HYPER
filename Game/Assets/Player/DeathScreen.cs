using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    [Header("Stats")] 
    [SerializeField] private TMP_Text runTime;
    [SerializeField] private TMP_Text kills;
    [SerializeField] private TMP_Text purchases;
    [SerializeField] private TMP_Text netWorth;
    [SerializeField] private TMP_Text roundsFinished;
    [SerializeField] private TMP_Text damageTaken;
    [SerializeField] private TMP_Text damageDealt;

    private void OnEnable(){
        
    }

}
