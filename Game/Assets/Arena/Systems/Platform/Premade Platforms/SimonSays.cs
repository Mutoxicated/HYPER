using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimonSays : MonoBehaviour
{
    private static int steps = 3;

    [SerializeField] private GameObject padHolder;
    [SerializeField] private GameObject holder;
    [SerializeField] private Pad[] pads;
    [SerializeField] private int scoreToGive = 6000;
    [SerializeField] private int shieldToGive = 3;
    [SerializeField] private ScorePopupCanvas spc;
    [SerializeField] private TMP_Text turn;
    public OnInterval dieInterval;

    private bool playersTurn = false;
    private List<int> expectedSteps = new List<int>();
    private List<int> playerSteps = new List<int>();
    private List<Pad> padsChosen = new List<Pad>();

    private float waitTime = 0.8f;
    private float time;
    private float yOff;

    private int currentSteps;
    private Vector3 alteredScale;
    private int currentIndex = 0;

    private bool isPlaying = true;

    public bool IsPlayersTurn(){
        return playersTurn;
    }

    public void AddPlayerStep(int ID){
        playerSteps.Add(ID);
        if (!PlayerInputWasValid()){
            Fail();
            turn.gameObject.SetActive(false);
            ChangeTurn(false);
            return;
        }
        if (playerSteps.Count == expectedSteps.Count){
            ChangeTurn(false);
            if (currentSteps > steps){
                Win();
                turn.gameObject.SetActive(false);
            }
        }
    }

    public void Pause(){
        isPlaying = false;
    }

    public void Resume(){
        isPlaying = true;
    }

    private void ChangeTurn(bool state){
        playersTurn = state;
        if (state){
            turn.text = "Your turn";
            return;
        }
        turn.text = "Wait";
    }

    private bool PlayerInputWasValid(){
        Debug.Log("Player Steps total: "+playerSteps.Count);
        Debug.Log("Expected Steps total: "+expectedSteps.Count);
        for (int i = 0; i < playerSteps.Count;i++){
            Debug.Log(expectedSteps[i]+" | "+playerSteps[i]);
            if (expectedSteps[i] != playerSteps[i])
                return false;
        }
        return true;
    }

    private void Start(){
        float tx = Mathf.InverseLerp(0f,PlatformObjective.initPlatScale.x,holder.transform.parent.transform.localScale.x);
        float y = holder.transform.parent.transform.localScale.y-PlatformObjective.initPlatScale.y;
        if (y < 0f)
            y = 0f;
        //holder.transform.parent.transform.localScale.y-PlatformObjective.initPlatScale.y
        yOff = Mathf.Lerp(0f,15f,tx);
        padHolder.transform.localScale *= tx;

        alteredScale = padHolder.transform.localScale;
        padHolder.transform.position = new Vector3(padHolder.transform.position.x,padHolder.transform.position.y+y,padHolder.transform.position.z);
        padHolder.transform.localScale = alteredScale;    

        currentSteps = 1;
        holder.transform.localPosition = Vector3.zero;
    }

    private void Update(){
        if (dieInterval.enabled){
            return;
        }
        if (!isPlaying)
            return;

        if (!playersTurn){
            time += Time.deltaTime;
            //Debug.Log(time);
            if (time >= waitTime){
                time = 0f;
                if (currentIndex >= expectedSteps.Count){
                    int index = Random.Range(0,pads.Length);
                    expectedSteps.Add(pads[index].GetID());
                    padsChosen.Add(pads[index]);
                    pads[index].LightUp();
                    ChangeTurn(true);
                    playerSteps.Clear();
                    currentSteps++;
                    currentIndex = 0;
                }else{
                    padsChosen[currentIndex].LightUp();
                    currentIndex++;
                }
            }
        }
    }

    private void Fail(){
        dieInterval.enabled = true;
    }

    private void Win(){
        spc.transform.SetParent(null,false);
        if (!Difficulty.roundFinished){
            spc.PopScore(scoreToGive,4f,0f);
            PlayerInfo.AddScore(scoreToGive);
            if (Random.Range(0f,101f) <= PlatformObjective.shieldChance){
                spc.PopShield(shieldToGive,4f,0f);
                PlayerInfo.GetGun().stats.AddShield(scoreToGive);
            }
        }
        spc.transform.position = new Vector3(transform.position.x,transform.position.y+yOff,transform.position.z);
        spc.transform.localRotation = Quaternion.identity;
        spc.transform.rotation = Quaternion.identity;
        dieInterval.enabled = true;
    }

    public void Die(){
        spc.Die();
        Destroy(holder);
    }

}
