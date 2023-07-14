using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class FadeMatColor : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private MaterialColorChannel colorChannel;
    [SerializeField] private int index = 1;
    [SerializeField] private bool[] useMaterialValues = new bool[4];
    [SerializeField, GradientUsage(true)] private Gradient gradient;
    [SerializeField] private bool useOnInterval = false;
    [SerializeField] private OnInterval onInterval;
    [SerializeField] private float interval;
    private float rate;
    private Material mat;
    private const float tau = Mathf.PI * 2f;
    private float angle = 0f;

    private float[] colorValues = new float[4];
    private float t;
    private Color gradientColor;
    [HideInInspector] public Color color = new Color(0f,0f,0f,0f);

    void Start()
    {
        rate = Mathf.PI / (interval*Mathf.PI);
        if (tmpText == null)
        {
            var a = GetComponent<Renderer>().materials;
            mat = a[index];
        }
        ComputeColorValues();
    }

    private void ComputeColorValues()
    {
        for (int i = 0; i < useMaterialValues.Length; i++)
        {
            if (useMaterialValues[i])
            {
                if (tmpText == null)
                {
                    colorValues[i] = mat.GetColor(colorChannel.ToString())[i];
                }
                else
                {
                    colorValues[i] = tmpText.color[i];
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

    void Update()
    {
        if (!useOnInterval)
        {
            angle += rate * Time.deltaTime;
            t = Mathf.Sin(angle);
            if (angle >= Mathf.PI)
            {
                angle = 0f;
            }
        }
        else if (useOnInterval)
        {
            t = onInterval.t;
        }
        gradientColor = gradient.Evaluate(t);
        ComputeColorValues();
        if (tmpText == null)
            mat.SetColor(colorChannel.ToString(), color);
        else
        {
            tmpText.color = color;
        }
    }
}
