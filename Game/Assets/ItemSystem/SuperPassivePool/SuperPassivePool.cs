using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPassivePool : MonoBehaviour
{
    private static Dictionary<string, SuperPassive> superPassives = new Dictionary<string, SuperPassive>();

    private void Awake(){
        superPassives.Clear();
        Transform trans;
        for (int i = 0; i < transform.childCount;i++){
            trans = transform.GetChild(i);
            superPassives.Add(trans.gameObject.name,trans.gameObject.GetComponent<SuperPassive>());
        }
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
        if (superPassives[name].GetDevelopState())
            superPassives[name].gameObject.SendMessage("Develop");
    }
}
