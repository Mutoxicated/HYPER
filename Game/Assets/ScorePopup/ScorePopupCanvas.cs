using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePopupCanvas : MonoBehaviour
{
    [SerializeField] private Transform scoreSpawnPoint;
    private Quaternion lookRotation = Quaternion.identity;
    private Vector3 toEntity = Vector3.zero;

    private Vector3 initScale;
    private float scaleFactor;
    private bool hasScorePopups = true;
    private bool die;
    private float time;

    public void PopScore(int score, float duration){
        ScorePopupPool.spp.GetObject(scoreSpawnPoint,score,duration);
        hasScorePopups = true;
    }

    public bool HasScorePopups(){
        return hasScorePopups;
    }

    private void Start(){
        initScale = scoreSpawnPoint.localScale;
    }

    private void Update(){
        if (die && !hasScorePopups){
            Destroy(gameObject);
        }
        toEntity = PlayerInfo.GetPlayer().transform.position - transform.position;
        lookRotation = Quaternion.LookRotation(toEntity,Vector3.up);
        transform.rotation = lookRotation;

        scaleFactor = (Camera.main.transform.position - transform.position).magnitude*0.065f;
        scoreSpawnPoint.localScale = initScale*scaleFactor;
        if (scoreSpawnPoint.childCount == 0){
            hasScorePopups = false;
        }else{
            hasScorePopups = true;
        }
    }

    public void Die(){
        die = true;
    }
}
