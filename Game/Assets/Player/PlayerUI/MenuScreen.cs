using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    [SerializeField] private GameObject menuScreen;
    [SerializeField] private GameObject gameScreen;
    [SerializeField] private GameObject arrow;

    private ButtonInput esc = new ButtonInput("Esc");
    private bool activated;

    public void SwitchState(){
        activated = !activated;
        menuScreen.SetActive(activated);

        arrow.SetActive(!activated);
        gameScreen.SetActive(!activated);
        if (activated){
            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
        }else{
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1f;
        }
    }

    private void Update(){
        esc.Update();
        if (esc.GetInputDown()){
            Debug.Log(activated);
            SwitchState();
        }
    }

    public void SetTimeToNormal(){
        Time.timeScale = 1f;
    }

}
