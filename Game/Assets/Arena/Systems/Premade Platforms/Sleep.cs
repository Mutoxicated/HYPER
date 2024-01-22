using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sleep : MonoBehaviour
{
    private static readonly float initDamage = 200f;

    [SerializeField] private Renderer rend;

    private float damage;
    private float time;
    private float interval = 0.5f;

    private void OnEnable(){
        damage = initDamage;
        rend.materials[1].color = Color.red;
    }

    private void OnCollisionEnter(Collision coll){
        if (coll.gameObject.tag != "Player" | !enabled) return;
        PlayerInfo.GetPH().TakeDamage(damage,1f,0);
    }

    private void OnCollisionStay(Collision coll){
        if (coll.gameObject.tag != "Player" | !enabled) return;
        time += Time.deltaTime;
        if (time >= interval){
            PlayerInfo.GetPH().TakeDamage(damage,1f,0);
            time = 0f;
        }
    }
}
