using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Derust : MonoBehaviour
{
    public GameObject holder;
    [SerializeField] private Renderer rend;
    [SerializeField] private ScorePopupCanvas spc;
    [SerializeField] private int scoreToGive = 2500;

    private float yOff;
    private Vector2 offset;
    private Vector2 offset2;

    private void Start(){
        float tx = Mathf.InverseLerp(0f,PlatformObjective.initPlatScale.x,holder.transform.parent.transform.localScale.x);
        yOff = Mathf.Lerp(0f,15f,tx);
        offset = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
        offset *= 0.001f;
        offset = new Vector2(Random.Range(-1f,1f),Random.Range(-1f,1f));
        offset *= 0.0005f;
    }

    public void EndObjective(){
        spc.transform.SetParent(null,false);
        spc.transform.position = new Vector3(transform.position.x,transform.position.y+yOff,transform.position.z);
        spc.transform.localRotation = Quaternion.identity;
        spc.transform.rotation = Quaternion.identity;
        if (!Difficulty.roundFinished){
            spc.PopScore(scoreToGive,3f,0f);
            PlayerInfo.AddScore(scoreToGive);
        }
        spc.Die();
        for (int i = 0; i < transform.parent.childCount;i++){
            if (transform.parent.GetChild(i) == transform)
                continue;
            Difficulty.spawnPoints.Add(transform.parent.GetChild(i).gameObject);
        }
        Destroy(holder);
    }

    private void MoveTexture(string name, Vector2 offset){
        rend.materials[2].SetTextureOffset(name, rend.materials[2].GetTextureOffset(name)+offset);
    }

    private void Update(){

        MoveTexture("_NoiseTexture2",offset);
        MoveTexture("_NoiseTexture1",offset2);
    }
}
