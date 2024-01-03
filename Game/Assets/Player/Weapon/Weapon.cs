using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/Weapon", order = 2)]
public class Weapon : ScriptableObject 
{
  //in the future there will be weapon wymbols and weapon sfx
  public string bulletPool;
  public float recoilModifier;//0 to 1
  public float fireRate;//in seconds

  public bool extra;
  public string extraBulletPool;
  public float extraRecoilModifier;
  public float extraFireRate;
}
