using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Feed : MonoBehaviour
{
    public static readonly int maxIndex = 8;
    public static readonly float space = 60f;

    [SerializeField] private Transform messageBase;

    private List<Message> activeMsgs = new List<Message>();
    [SerializeField] private List<Message> inactiveMsgs = new List<Message>();

    public void RetrieveMessage(Message msg){
        activeMsgs.Remove(msg);
        inactiveMsgs.Add(msg);
    }

    public void Message(string text){
        Message chosenMsg = inactiveMsgs[0];
        chosenMsg.PopMessage(text,0);
        inactiveMsgs.Remove(chosenMsg);

        foreach (Message msg in activeMsgs){
            msg.IncrementIndex();
        }

        activeMsgs.Add(chosenMsg);
    }

    public Vector3 GetBasePosition(){
        return messageBase.localPosition;
    }
}
