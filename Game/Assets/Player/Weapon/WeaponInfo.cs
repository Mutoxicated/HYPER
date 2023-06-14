using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Events;

public static class WeaponInfo
{
    public static float fireRate;
    public static float fireRateMod = 1f;
    public static Queue<GameObject> bulletQueue = new Queue<GameObject>();
    public static UnityEvent OnShootEvent = new UnityEvent();
    public static GameObject bulletPrefab;
}
