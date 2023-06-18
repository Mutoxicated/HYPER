using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class staminaComms : MonoBehaviour
{
    [SerializeField] private staminaComms previousNeighbor;
    [SerializeField] private staminaComms nextNeighbor;
    private Vector3 minScale, maxScale;
    private float t = 100f;
    private bool chargeBack = false;
    private bool full = true;
    private float remainder;
    private bool activate;

    //im sorry this is a compact mess
    //idk wtf happened i was prolly high and shit

    private void TransferRemainingLoss(float remainder)
    {
        previousNeighbor.AcquireRemainingLoss(remainder);
    }

    public void AcquireRemainingLoss(float remainder)
    {
        LoseStamina(remainder);
    }

    public bool GetIsFull()
    {
        return full;
    }

    public float GetStamina()
    {
        return t;
    }

    public void LoseStamina(float loss)
    {
        if (t < loss && previousNeighbor != false)
        {
            remainder = loss - t;
            t = 0f;
            TransferRemainingLoss(remainder);
            chargeBack = false;
        }
        else if (t>= loss)
        {
            t = t - loss;
            chargeBack = false;
        }
        full = false;
        activate = true;
    }

    public void ActivateChargeback()
    {
        activate = true;
        chargeBack = true;
    }

    void Awake()
    {
        maxScale = new Vector3(1f, 1f, 1f);
        minScale = new Vector3(0f,1f,1f);
    }

    void FixedUpdate()
    {
        if (full)
            return;
        //lose
        if (!activate)
            return;
        if (chargeBack)
        {
            t = t + 0.5f;
        }
        if (t > 100f)
            t = 100f;

        //charge back
        //t += 0.01f;
        if (chargeBack)
            transform.localScale = Vector3.Lerp(minScale, maxScale, t / 100f);
        else
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxScale.x * t / 100f,transform.localScale.y, transform.localScale.z), Time.deltaTime*10f);
        //Debug.Log(gameObject.name);
        if (transform.localScale == maxScale)
        {
            full = true;
            chargeBack = false;
            if (nextNeighbor != null)
                nextNeighbor.ActivateChargeback();
            activate = false;
        }
        if (transform.localScale == new Vector3(maxScale.x * t / 100f, transform.localScale.y, transform.localScale.z))
        {
            if (previousNeighbor != null)
            {
                if (previousNeighbor.GetIsFull())
                {
                    chargeBack = true;
                }
                else
                {
                    activate = false;
                }
            }
            else
            {
                chargeBack = true;
            }
        }
    }
}
