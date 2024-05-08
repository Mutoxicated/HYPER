using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class PlayerEvents 
{
    public static void Flush() {
        EnemyBulletKill = null;
    }

    public static UnityEvent<Vector3, Quaternion> EnemyBulletKill = new UnityEvent<Vector3, Quaternion>();
}
