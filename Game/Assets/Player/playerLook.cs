using Unity.VisualScripting;
using UnityEngine;

public class playerLook : MonoBehaviour
{
    [SerializeField] private float sensX, sensY;
    private float rotationX, rotationY;
    private Quaternion lookRotation;

    [SerializeField] private Quaternion alterToRotation;
    [SerializeField] private bool alterRotation;

    private void Start()
    {
        if (alterRotation)
            AlterLookRotation(alterToRotation);
    }

    private void Update()
    {
        rotationX -= Input.GetAxis("Mouse Y") * Time.deltaTime * (sensY * 100f);
        rotationY += Input.GetAxis("Mouse X") * Time.deltaTime * (sensX * 100f);
        //clamp it so that cam cant freely move
        rotationX = Mathf.Clamp(rotationX, -89f, 89f);
        lookRotation = Quaternion.Euler(rotationX, rotationY, 0f);
        transform.rotation = lookRotation;

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
}
