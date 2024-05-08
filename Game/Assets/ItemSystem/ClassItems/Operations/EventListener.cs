using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField] private ClassItem item;

    [SerializeReference] public Event Event;

    [SerializeField] private bool EnemyBulletKill;

    private void Start() {
        PlayerEvents.EnemyBulletKill.AddListener(Event.Instance.Event);
    }
}
