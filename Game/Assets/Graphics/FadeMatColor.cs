using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class FadeMatColor : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpTextAlt;
    [SerializeField] private Image imageAlt;
    [SerializeField] private int index = 1;
    [SerializeField] private bool[] useMaterialValues = new bool[4];
    [SerializeField, GradientUsage(true)] private Gradient gradient;
    [SerializeField] private OnInterval onInterval;
    [SerializeField] private float interval;
    private float rate;
    private Material mat;
    private float angle = 0f;

    private float[] colorValues = new float[4];
    private float t;
    private Color gradientColor;
    private Color color = new Color(0f,0f,0f,0f);

    void Awake()
    {
        rate = Mathf.PI / (interval*Mathf.PI);
        if (tmpTextAlt == null)
        {
            if (imageAlt == null)
            {
                var a = GetComponent<Renderer>().materials;
                mat = a[index];
            }
        }
        CalculateT();
        ComputeColorValues();
        AssignColor();
    }

    private void ComputeColorValues()
    {
        gradientColor = gradient.Evaluate(t);
        for (int i = 0; i < useMaterialValues.Length; i++)
        {
            if (useMaterialValues[i])
            {
                if (tmpTextAlt != null)
                {
                    colorValues[i] = tmpTextAlt.color[i];
                }
                else if (imageAlt != null)
                {
                    colorValues[i] = imageAlt.color[i];
                }
                else
                {
                    colorValues[i] = mat.color[i];
                }
            }
            else
            {
                colorValues[i] = gradientColor[i];
            }
            color[i] = colorValues[i];
        }
    }

    public void ChangeGradient(Gradient gradient)
    {
        this.gradient = gradient;
    }

    private void AssignColor()
    {
        if (tmpTextAlt != null)
        {
            tmpTextAlt.color = color;
        }
        else if (imageAlt != null)
        {
            imageAlt.color = color;
        }
        else
        {
            mat.color = color;
        }
    }

    private void CalculateT()
    {
        if (onInterval == null)
        {
            angle += rate * Time.deltaTime;
            t = Mathf.Sin(angle);
            if (angle >= Mathf.PI)
            {
                angle = 0f;
            }
        }
        else
        {
            t = onInterval.t;
        }
    }

    void Update()
    {
        CalculateT();
        ComputeColorValues();
        AssignColor();
    }
}
