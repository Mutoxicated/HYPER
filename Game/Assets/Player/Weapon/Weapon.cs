using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 2)]
public class Weapon : ScriptableObject 
{
  //in the future there will be weapon sfx
  public Sprite weaponSymbol;
  public string bulletPool;
  public float recoilModifier;//0 to 1
  public float fireRate;//in seconds
  public float modifier;

  public bool extraEnabled;
  public Weapon extra;
}
