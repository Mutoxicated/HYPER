using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopMat : MonoBehaviour
{
    [SerializeField] private GameObject[] GUI;

    [SerializeField] private TransformLerper tl;
    [SerializeField] private TransformLerper tl2;
    [SerializeField] private GameObject waitingScreen;
    [SerializeField] private GameObject mainMenu;

    private Rigidbody rb;

    private void Start(){
        Debug.Log("Hppp.");
        tl.SetTrans(PlayerInfo.GetCam().transform);
        tl2.SetTrans(PlayerInfo.GetCam().transform);
        tl2.SetTransToLerpTo(PlayerInfo.GetCam().transform.parent);
        rb = PlayerInfo.GetMovement().GetRB();
    }

    private void OnCollisionEnter(){
        Lock();
    }

    private void Lock(){
        tl.EnableTransformLerper();
        PlayerIsLocked(true);
        waitingScreen.SetActive(false);
        mainMenu.SetActive(true);
        PlayerInfo.GetMovement().enabled = false;
        foreach (GameObject go in GUI){
            go.SetActive(false);
        }
    }

    public void Unlock(){
        tl2.EnableTransformLerper();
        waitingScreen.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void PlayerIsLocked(bool state){
        if (state){
            rb.constraints = RigidbodyConstraints.FreezeAll;
        }else{
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
        foreach (GameObject go in GUI){
            go.SetActive(!state);
        }
        PlayerInfo.GetMelee().enabled = !state;
        PlayerInfo.GetMovement().enabled = !state;
        PlayerInfo.GetPL().IsLocked(state);
        PlayerInfo.GetGun().IsLocked(state);
    }
}
