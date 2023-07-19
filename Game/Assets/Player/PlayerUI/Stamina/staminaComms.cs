using UnityEngine;
using UnityEngine.UI;

public class staminaComms : MonoBehaviour
{
    public bool locked = false;
    [SerializeField] private Image image;
    [SerializeField] private Image backImage;
    [SerializeField] private staminaComms previousNeighbor;
    [SerializeField] private staminaComms nextNeighbor;
    private Color lockedColor = new Color(0.5f, 0.5f, 0.5f, 0.20f);
    private Vector3 minScale, maxScale;
    private float t = 100f;
    private bool chargeBack = false;
    private bool full = true;
    private float remainder;
    private bool activate;
    private float lerp;

    //im sorry this is a compact mess
    //idk wtf happened i was prolly high and shit

    public void SetLockState(bool state)
    {
        locked = state;
        backImage.gameObject.SetActive(!state);
        if (state == false)
            image.color = previousNeighbor.GetComponent<Image>().color;
        else
            image.color = lockedColor;
    }

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
        if (locked)
        {
            TransferRemainingLoss(loss);
            return;
        }
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
        if (locked)
        {
            image.color = lockedColor;
        }
        maxScale = new Vector3(1f, 1f, 1f);
        minScale = new Vector3(0f,1f,1f);
    }

    void Update()
    {
        if (locked)
            return;
        if (full)
            return;
        if (!activate)
            return;
        if (chargeBack)
        {
            t += 32f*Time.deltaTime;
        }
        if (t > 100f)
            t = 100f;

        if (Mathf.Approximately(t, 0f))
            lerp = 0f;
        else
            lerp = t / 100f;

        //charge back
        if (chargeBack)
            transform.localScale = Vector3.Lerp(minScale, maxScale, lerp);
        else
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(maxScale.x * lerp, transform.localScale.y, transform.localScale.z), Time.deltaTime*10f);
        
        if (transform.localScale == maxScale)
        {
            full = true;
            chargeBack = false;
            if (nextNeighbor != null)
                nextNeighbor.ActivateChargeback();
            activate = false;
        }
        if (transform.localScale == new Vector3(maxScale.x * lerp, transform.localScale.y, transform.localScale.z))
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
