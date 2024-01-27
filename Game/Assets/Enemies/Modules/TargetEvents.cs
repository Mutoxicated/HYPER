using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class TargetEvents : MonoBehaviour
{
    [SerializeField] private Stats stats;
    [SerializeField] private UnityEvent<bool> targetInRange = new UnityEvent<bool>();
    [SerializeField] private UnityEvent<bool> targetInCloseRange = new UnityEvent<bool>();

    private bool switch1 = false;
    private bool switch2 = false;

    private float t = 0f;
    private float checkInSeconds = 1f;
    private float dist;
    private bool inRange = false;

    private void Start(){
        if (stats.entity == null){
            CallTargetRange(false);
            inRange = false;
            return;
        }
        dist = Vector3.Distance(transform.position,stats.entity.position);
        if (dist <= stats.numericals["range"]){
            CallTargetRange(true);
            inRange = true;
            switch2 = true;
                    
        }else if (dist > stats.numericals["range"]){
            CallTargetRange(false);
            inRange = false;
        }
    }

    private void PlayerEvents(){
        t += Time.deltaTime;
        if (t >= checkInSeconds){
            t = 0f;
            if (stats.entity == null){
                return;
            }
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
            if (inRange){
                if (dist <= stats.numericals["range"]*0.5f && !switch1){
                    CallCloseRange(true);
                    switch1 = true;
                }else if (dist > stats.numericals["range"]*0.5f && switch1){
                    CallCloseRange(false);
                    switch1 = false;
                }
            }else if (!inRange && switch1){
                CallCloseRange(false);
                switch1 = false;
            }
        }
    }

    private void CallCloseRange(bool state){
        if (targetInCloseRange.GetPersistentEventCount() != 0){
            targetInCloseRange.Invoke(state);
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
            if (stats.entity == null){
                return;
            }
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
            if (inRange){
                if (dist <= stats.numericals["range"]*0.65f && !switch1){
                    CallCloseRange(true);
                    switch1 = true;
                }else if (dist > stats.numericals["range"]*0.65f && switch1){
                    CallCloseRange(false);
                    switch1 = false;
                }
            }else if (!inRange && switch1){
                CallCloseRange(false);
                switch1 = false;
            }
        }
    }

    private void Update(){
        if (stats.GetCurrentLayer() == DeathFor.PLAYER){
            PlayerEvents();
        }else{
            EnemyEvents();
        }
    }
}
