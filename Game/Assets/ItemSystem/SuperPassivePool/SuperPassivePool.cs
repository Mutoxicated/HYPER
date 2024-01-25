using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperPassivePool : MonoBehaviour
{
    [SerializeField] private static Dictionary<string, SuperPassive> superPassives = new Dictionary<string, SuperPassive>();

    private void Awake(){
        Transform trans;
        for (int i = 0; i < transform.childCount;i++){
            trans = transform.GetChild(i);
            superPassives.Add(trans.gameObject.name,trans.gameObject.GetComponent<SuperPassive>());
        }
    }

    public static void SetDevelopState(string name, bool state){
        superPassives[name].SetDevelopState(state);
    }

    public static void DevelopPassiveByName(string name){
        superPassives[name].gameObject.SendMessage("Develop");
    }
}
