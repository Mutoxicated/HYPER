using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePopupPool : MonoBehaviour
{
    public static ScorePopupPool spp;
    [SerializeField] private GameObject scorePopupPrefab;

    private List<ScorePopup> inactivePopups = new List<ScorePopup>();
    private List<ScorePopup> activePopups = new List<ScorePopup>();

    private void Awake(){
        spp = this;
    }

    public void GetObject(Transform parent, int score, float duration){
        if (inactivePopups.Count == 0){
            GameObject instance = Instantiate(scorePopupPrefab, parent.position,parent.rotation);
            instance.transform.SetParent(parent,false);
            instance.transform.rotation = parent.rotation;
            var popup  = instance.GetComponent<ScorePopup>();
            popup.SetText(score);
            popup.SetDuration(duration);
            activePopups.Add(popup);
        }else{
            inactivePopups[0].gameObject.transform.SetParent(parent,false);
            inactivePopups[0].transform.rotation = parent.rotation;
            inactivePopups[0].SetText(score);
            inactivePopups[0].SetDuration(duration);
            inactivePopups[0].gameObject.SetActive(true);
            AddAndRemove(inactivePopups[0],activePopups,inactivePopups);
        }
    }

    public void ReturnObject(ScorePopup scorePopup){
        scorePopup.transform.SetParent(transform,false);
        scorePopup.transform.rotation = Quaternion.identity;
        AddAndRemove(scorePopup,inactivePopups,activePopups);
    }

    private void AddAndRemove(ScorePopup obj, List<ScorePopup> listToAdd, List<ScorePopup> listToRemove){
        if (listToRemove.Contains(obj))
            listToRemove.Remove(obj);
        if (!listToAdd.Contains(obj))
            listToAdd.Add(obj);
    }
}
