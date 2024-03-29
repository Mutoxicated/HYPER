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
    [SerializeField, GradientUsage(true)] private Gradient[] gradient;
    [SerializeField] private int gIndex;
    [SerializeField] private OnInterval onInterval;
    [SerializeField] private float interval;
    private float rate;
    private Material mat;
    private float angle = 0f;

    private Renderer _rend;
    private float[] colorValues = new float[4];
    private float t;
    private Color gradientColor;
    [HideInInspector] public Color color = new Color(0f,0f,0f,0f);

    private Renderer rend;

    void OnEnable()
    {
        rate = Mathf.PI / (interval*Mathf.PI);
        if (tmpTextAlt == null)
        {
            if (imageAlt == null)
            {
                rend = GetComponent<Renderer>();
                if (index <= rend.materials.Length-1)
                    mat = rend.materials[index];
            }
        }
        if (tmpTextAlt == null)
        {
            if (imageAlt == null)
            {
                if (mat == null)
                    return;
            }
        }
        CalculateT();
        ComputeColorValues();
        AssignColor();
    }

    private void ComputeColorValues()
    {
        gradientColor = gradient[gIndex].Evaluate(t);
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
                    if (mat == null)
                    {
                        mat = _rend.materials[index];
                    }
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

    public void ChangeGradientIndex(int index)
    {
        gIndex = index;
    }

    public void GoToNextGradientIndex()
    {
        if (gIndex + 1 > gradient.Length - 1)
        {
            gIndex = 0;
        }
        else
        {
            gIndex++;
        }
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
        if (tmpTextAlt == null)
        {
            if (imageAlt == null)
            {
                if (mat == null){
                    mat = rend.materials[index];
                    return;
                }
            }
        }
        CalculateT();
        ComputeColorValues();
        AssignColor();
    }
}
