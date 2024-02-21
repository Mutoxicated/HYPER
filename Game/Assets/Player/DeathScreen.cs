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

    [Header("Info")]
    [SerializeField] private Transform guiCanvas;
    [SerializeField] private ActualItemSorter ais;

    private int seconds;
    private int minutes;
    private int hours;

    private string runTimeString;

    private List<ClassItem> cis = new List<ClassItem>();

    private void OnEnable(){
        //stats
        runTimeString = "";
        minutes = Mathf.FloorToInt(RunDataSave.rData.runTimeSeconds/60);
        seconds = RunDataSave.rData.runTimeSeconds-(60*minutes);
        hours = Mathf.FloorToInt(minutes/60);
        minutes = Mathf.FloorToInt(RunDataSave.rData.runTimeSeconds/60)-(60*hours);

        if (hours != 0){
            runTimeString += hours+"h ";
        }
        if (minutes != 0){
            runTimeString += minutes+"m ";
        }
        runTimeString += seconds+"s";
        runTime.text = runTimeString;

        roundsFinished.text = RunDataSave.rData.rounds.ToString();
        netWorth.text = RunDataSave.rData.money.ToString();

        //info
        cis = ItemPool.GetClassItems();
        foreach (ClassItem ci in cis){
            ci.UIMode(true);
            ci.transform.SetParent(transform,true);
            ci.transform.localRotation = Quaternion.identity;
            ci.transform.localScale *= 0.1f;
            ci.transform.localPosition = Vector3.zero;
            ci.gameObject.SetActive(false);
            ais.AddItem(ci.transform);
        }
        ais.enabled = true;
    }

    public void ActivateItems(){
        cis = ItemPool.GetClassItems();
        foreach (ClassItem ci in cis){
            ci.gameObject.SetActive(true);
        }
    }

    public void SetTimeScale(float t){
        Time.timeScale = t;
    }
}
