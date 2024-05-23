using UnityEngine;
using UnityEngine.Events;
using static Numerical;

public class OnInterval : ExtraBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private string specialTag = "Untagged";
    [SerializeField] private bool useUnscaledTime;
    [SerializeField] private bool dontResetTime;
    [SerializeField, Range(0, 100)] private int chance = 100;
    [SerializeField] private float interval;
    [SerializeField] private Vector2 minMaxInterval;
    [SerializeField] public UnityEvent onInterval;
    [SerializeField] private bool destroyUponEvent;
    [SerializeField] private bool recycleUponEvent;

    private bool useStats = false;
    [HideInInspector] public float t = 0f;
    [HideInInspector]
    public float time = 0f;
    [HideInInspector] public bool isPlaying;
    private float deltaTime;

    public Vector2 MinMax {
        get {
            return minMaxInterval;
        }
    }

    public void ResetEarly()//this is stupid but oh well lol
    {
        ResetInterval();
    }

    public void ResetEventless()
    {
        time = 0f;
        t = 0f;
    }

    private void ResetInterval()
    {
        if (Random.Range(0, 100) <= chance)
            onInterval.Invoke();
        if (destroyUponEvent)
            Destroy(gameObject);
        if (recycleUponEvent)
            PublicPools.pools[gameObject.name].Reattach(gameObject);
        if (!dontResetTime){
            time = 0f;
            t = 0f;
        }
    }

    public void Pause()
    {
        isPlaying = false;
    }

    public void Resume()
    {
        isPlaying = true;
    }

    public void ManipulateT(float t)
    {
        this.t = t;
    }

    public float GetInterval(){
        return interval;
    }

    public void ChangeInterval(float interval)
    {
        this.interval = interval;
    }

    public void ChangeMinMaxInterval(Vector2 newMinMaX) {
        minMaxInterval = newMinMaX;
    }

    public void RandomizeInterval() {
        if (minMaxInterval != Vector2.zero){
            interval = Random.Range(minMaxInterval.x,minMaxInterval.y);
        }
    }

    private void OnEnable()
    {
        //Debug.Log(gameObject.name);
        RandomizeInterval();
        time = 0f;
        t = 0f;
        isPlaying = true;
        if (stats != null)
            useStats = true;
    }

    private void Update()
    {
        if (!isPlaying)
            return;
        if (useUnscaledTime){
            deltaTime = Time.unscaledDeltaTime;
        }else{
            deltaTime = Time.deltaTime;
        }
        if (useStats)
            time += deltaTime * stats.numericals[RATE];
        else
            time += deltaTime;
        //Debug.Log(time);
        if (time >= interval)
        {
            ResetInterval();
        }
        if (time != 0f)
            t = time / interval;
        else t = 0f;
    }
}
