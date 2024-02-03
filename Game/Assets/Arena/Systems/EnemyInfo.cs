using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyInfo", menuName = "ScriptableObjects/EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    public GameObject enemyPrefab;
    public int importance = 1;
}
