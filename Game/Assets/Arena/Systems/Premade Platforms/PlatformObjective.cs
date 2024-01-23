using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformObjectiveType{
    NONE,
    RECHARGE,
    SLEEP,
    SIMON_SAYS,
    DERUST,
    TRAP
}

public class PlatformObjective : MonoBehaviour
{
    public static readonly Vector3 initPlatScale = new Vector3(72f,2f,72f);

    private static readonly Color normalColor = new Color(0.5f,0.5f,0.5f,1f);
    private static readonly Color sleepColor = new Color(0.35f,0.35f,0.35f,1f);
    private static readonly Color rechargeColor = new Color(0.35f,0.65f,0.75f,1f);
    private static readonly Color simonColor = new Color(0.35f,0.55f,0.1f,1f);

    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private Renderer rend;
    [SerializeField] private PlatformObjectiveType pot = PlatformObjectiveType.NONE;
    [SerializeField] private GameObject rechargePrefab;
    [SerializeField] private GameObject simonPrefab;
    [SerializeField] private GameObject derustPrefab;
    [SerializeField] private GameObject trapPrefab;
    [SerializeField] private Sleep sleepComponent;

    [SerializeField] private int touchTolerance = 1;
    private int currentTouches = 0;
    private GameObject instance;

    private void InstatiateObjectivePrefab(GameObject prefab){
        instance = Instantiate(prefab,transform,true);
        instance.transform.position = transform.position;
    }

    private void SetColor(Color color){
        rend.materials[1].color = color;
    }

    public void SetPot(PlatformObjectiveType pot){
        this.pot = pot;
        EvaluateCharacteristics();
    }

    private void RemoveSpawnsFromArray(){
        foreach (GameObject go in spawnPoints){
            Difficulty.spawnPoints.Remove(go);
        }
    }

    private void EvaluateCharacteristics(){
        switch(pot){
            case PlatformObjectiveType.NONE:
                SetColor(normalColor);
                break;
            case PlatformObjectiveType.RECHARGE:
                SetColor(rechargeColor);
                break;
            case PlatformObjectiveType.SLEEP:
                SetColor(sleepColor);
                touchTolerance = 3;
                break;
            case PlatformObjectiveType.SIMON_SAYS:
                SetColor(simonColor);
                break;
            case PlatformObjectiveType.DERUST:
                InstatiateObjectivePrefab(derustPrefab);
                RemoveSpawnsFromArray();
                SetColor(simonColor);
                break;
            case PlatformObjectiveType.TRAP:
                SetColor(rechargeColor);
                break;
            default:
                break;
        }
    }

    private void OnEnable(){
        currentTouches = 0;
        touchTolerance = 1;
        EvaluateCharacteristics();
    }

    private void ObjectiveLogic(){
        switch(pot){
            case PlatformObjectiveType.RECHARGE:
                InstatiateObjectivePrefab(rechargePrefab);
                break;
            case PlatformObjectiveType.SLEEP:
                sleepComponent.enabled = true;
                break;
            case PlatformObjectiveType.SIMON_SAYS:
                InstatiateObjectivePrefab(simonPrefab);
                break;
            case PlatformObjectiveType.TRAP:
                InstatiateObjectivePrefab(trapPrefab);
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision collision){
        if (pot == PlatformObjectiveType.NONE)
            return;
        if (!enabled)
            return;
        if (collision.gameObject.tag != "Player")
            return;
        currentTouches++;
        if (currentTouches == touchTolerance){
            ObjectiveLogic();
        }
    }
}
