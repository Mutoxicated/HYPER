using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfo", menuName = "ScriptableObjects/EnemyInfo")]
public class EnemyInfo : ScriptableObject
{
    public GameObject enemyPrefab;
    public int importance = 1;
    public int priority = 0; //0 is benign unless provoked, 1 is nuisance and 2 is serious threat

}
