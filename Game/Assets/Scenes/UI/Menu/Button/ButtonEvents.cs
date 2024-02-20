using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour
{
    private void Start(){
        PlayerInfo.SetEchelon(0);
    }

    public void ExitApp(){
        Application.Quit();
    }

    public void ChooseEchelon(int index){
        PlayerInfo.SetEchelon(index);
    }

    public void StartRun(){
        RunDataSave.CreateData();
    }

    public void UpdateRunInfo(){
        RunDataSave.UpdateData();
    }

    public void DeleteRunInfo(){
        RunDataSave.RemoveData();
    }
}
