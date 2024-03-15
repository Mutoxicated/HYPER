using UnityEngine;

public class playerLook : MonoBehaviour
{
    [SerializeField] private float sensX, sensY;
    private float rotationX, rotationY;
    private Quaternion lookRotation;

    [SerializeField] public Quaternion alterToRotation;
    [SerializeField] private bool alterRotation;

    private void Awake(){
        if (PlayerInfo.GetCam() == null){
            PlayerInfo.SetCam(GameObject.FindWithTag("MainCamera").GetComponent<Camera>());
        }
        PlayerInfo.SetPL(this);
    }

    private void Start()
    {
        if (alterRotation)
            AlterLookRotation(alterToRotation);
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f)
            return;
        rotationX -= Input.GetAxis("Mouse Y") * Time.fixedDeltaTime * (sensY * 100f);
        rotationY += Input.GetAxis("Mouse X") * Time.fixedDeltaTime * (sensX * 100f);
        //clamp it so that cam cant freely move
        rotationX = Mathf.Clamp(rotationX, -89f, 89f);
        lookRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, Time.fixedDeltaTime*37f);

        if (Input.GetMouseButtonDown(0)){
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void AlterLookRotation(Quaternion rotation)
    {
        var differenceX = rotationX - rotation.eulerAngles.x;
        var differenceY = rotation.eulerAngles.y - rotationY;
        rotationX -= differenceX;
        rotationY += differenceY;
    }

    public void IsLocked(bool state){
        this.enabled = !state;
        if (state){
            Cursor.lockState = CursorLockMode.None;
        }else{
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
