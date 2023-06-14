using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBehaviour : MonoBehaviour
{
    [Header("Extra Shot")]
    [SerializeField] private float fireOffset;
    [SerializeField] private float fireChance;

    [Header("Bullet Replacement")]
    [SerializeField] private GameObject[] bulletPrefabs;
    [SerializeField, Range(0f, 100f)] private float replaceChance;

    [Header("Modifiers")]
    [SerializeField] private float fireRateMod;
    
}
