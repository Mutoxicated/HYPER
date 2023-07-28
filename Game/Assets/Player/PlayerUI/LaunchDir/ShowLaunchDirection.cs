using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowLaunchDirection : MonoBehaviour
{
    [SerializeField] private GameObject arrowHolder;
    private Transform playerTransform;
    private Movement movement;

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
        movement = playerTransform.GetComponent<Movement>();
    }

    private void LateUpdate()
    {
        //arrowHolder.transform.rotation = Quaternion.Lerp(arrowHolder.transform.rotation, 
        //    Quaternion.LookRotation(movement.launchPoint - playerTransform.position), 
        //    Time.deltaTime * 10f);
        if (movement.launchInterval.enabled)
        {
            arrowHolder.transform.rotation = Quaternion.LookRotation(movement.launchPoint - playerTransform.position);
            arrowHolder.SetActive(true);
            return;
        }
        arrowHolder.SetActive(false);
    }
}
