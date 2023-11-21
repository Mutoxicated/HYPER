using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class TargetEvents : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private UnityEvent<bool> targetInRange = new UnityEvent<bool>();
    [SerializeField] private UnityEvent<bool> targetOnSameLevel = new UnityEvent<bool>();

    private bool switch1 = false;
    private bool switch2 = false;

    private float t = 0f;
    private float checkInSeconds = 1f;
    private float dist;
    private bool inRange = false;

    private bool SameYAxis(Transform transformComparison, float tolerance){
        if (Mathf.Abs(transform.position.y-transformComparison.position.y) <= tolerance){
            return true;
        }
        return false;
    }

    private void PlayerEvents(){
        t += Time.deltaTime;
        if (t >= checkInSeconds){
            t = 0f;
            dist = Vector3.Distance(transform.position,Difficulty.player.position);
            if (dist <= stats.numericals["range"] && !switch2){
                CallTargetRange(true);
                inRange = true;
                switch2 = true;
                
            }else if (dist > stats.numericals["range"] && switch2){
                CallTargetRange(false);
                inRange = false;
                switch2 = false;
            }
        }

        if (inRange){
            if (SameYAxis(Difficulty.player,3f) && !switch1){
                CallTargetSameYLevel(true);
                switch1 = true;
            }else if (!SameYAxis(Difficulty.player,3f) && switch1){
                CallTargetSameYLevel(false);
                switch1 = false;
            }
        }
    }

    private void CallTargetSameYLevel(bool state){
        if (targetOnSameLevel.GetPersistentEventCount() != 0){
            targetOnSameLevel.Invoke(state);
            Debug.Log("Target at same Y Level: " + state);
        }
    }

    private void CallTargetRange(bool state){
        if (targetInRange.GetPersistentEventCount() != 0){
            targetInRange.Invoke(state);
            Debug.Log("Target In Range: " + state);
        }
    }

    private void EnemyEvents(){
        t += Time.deltaTime;
        if (t >= checkInSeconds){
            t = 0f;
            dist = Vector3.Distance(transform.position,stats.entity.position);
            if (dist <= stats.numericals["range"] && !switch2){
                CallTargetRange(true);
                inRange = true;
                switch2 = true;
                
            }else if (dist > stats.numericals["range"] && switch2){
                CallTargetRange(false);
                inRange = false;
                switch2 = false;
            }
        }

        if (inRange){
            if (SameYAxis(stats.entity,3f) && !switch1){
                CallTargetSameYLevel(true);
                switch1 = true;
            }else if (!SameYAxis(stats.entity,3f) && switch1){
                CallTargetSameYLevel(false);
                switch1 = false;
            }
        }
    }

    private void Update(){
        if (stats.objective == DeathFor.PLAYER){
            PlayerEvents();
        }else{
            EnemyEvents();
        }
    }
}
