using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SuperPassivePool : MonoBehaviour
{
    private static Dictionary<string, SuperPassive> superPassives = new Dictionary<string, SuperPassive>();
    
    public static void Clear(){
        superPassives.Clear();
    }

    private void GetSPs(){
        if (superPassives.Count != 0) return;
        Transform trans;
        for (int i = 0; i < transform.childCount;i++){
            trans = transform.GetChild(i);
            superPassives.Add(trans.gameObject.name,trans.gameObject.GetComponent<SuperPassive>());
        }
        if (RunDataSave.rData.superPassiveIterations.Count != 0){
            foreach (string name in superPassives.Keys){
                superPassives[name].SetIteration(RunDataSave.rData.superPassiveIterations[name]);
            }
        }
    }

    public static void UpdateRunData(){
        foreach (string name in superPassives.Keys){
            if (!RunDataSave.rData.superPassiveIterations.ContainsKey(name)){
                RunDataSave.rData.superPassiveIterations.Add(name,superPassives[name].GetIterations());
            }else{
                RunDataSave.rData.superPassiveIterations[name] = superPassives[name].GetIterations();
            }
        }
    }

    private void Awake(){
        GetSPs();
        DontDestroyOnLoad(transform.root);
    }

    public static int GetSuperPassiveCount(){
        return superPassives.Count;
    }

    public static SuperPassive GetPassiveByIndex(int index){
        foreach (string name in superPassives.Keys){
            if (superPassives[name].GetIndex() == index)
                return superPassives[name];
        }
        return null;
    }

    public static void SetDevelopState(string name, bool state){
        superPassives[name].SetDevelopState(state);
    }

    public static void DevelopPassiveByName(string name){
        if (!superPassives.ContainsKey(name)) return;
        Debug.Log(name+" is developing. "+superPassives[name].GetDevelopState());
        if (superPassives[name].GetDevelopState()){
            superPassives[name].gameObject.SendMessage("Develop");
        }
    }
}
