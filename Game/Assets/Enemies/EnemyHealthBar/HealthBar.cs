using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private EnemyHealth health;

    [SerializeField, GradientUsage(true)] private Gradient healthBarGradient;
    [SerializeField, GradientUsage(true)] private Gradient backBarGradient;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject backBar;

    [SerializeField] private float lerpSpeed;
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private OnInterval onInterval;

    private Vector3 initialScale;
    private Vector3 finalScale;
    private float t;

    private List<MaterialInfo> pack = new List<MaterialInfo>();
    private bool active = false;

    public void Activate()
    {
        objectToActivate.SetActive(true);
        active = true;
        if (!onInterval.enabled)
            onInterval.enabled = true;
        else
            onInterval.time = 2;
    }

    public void Deactivate()
    {
        objectToActivate.SetActive(false);
        active = false;
        onInterval.enabled = false;
    }

    private void Start()
    {
        Deactivate();
        initialScale = healthBar.transform.parent.localScale;
        finalScale = initialScale;
        pack.Add(new MaterialInfo(healthBar, healthBar.GetComponent<Renderer>().materials));
        pack.Add(new MaterialInfo(backBar, backBar.GetComponent<Renderer>().materials));
    }

    private void Update()
    {
        if (!active) return;
        if (health.currentHP != 0f)
            t = health.currentHP / health.maxHP;
        else t = 0f;
        finalScale.x = Mathf.Lerp(0f, initialScale.x, t);

        healthBar.transform.parent.localScale = Vector3.Lerp(healthBar.transform.parent.localScale,finalScale,Time.deltaTime*lerpSpeed);
        pack[0].mats[0].color = healthBarGradient.Evaluate(t);
        pack[1].mats[0].color = backBarGradient.Evaluate(t);
    }
}
