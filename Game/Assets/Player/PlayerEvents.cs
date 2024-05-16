using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PlayerEvents 
{
    public delegate void Event(Vector3 pos, Quaternion rot);

    public static void Flush() {
        EnemyBulletKill = null;
    }

    public static void Empty(Vector3 pos, Quaternion rot) { }

    public static Event EnemyBulletKill = new Event(Empty);
}
