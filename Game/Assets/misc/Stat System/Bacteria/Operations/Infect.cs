using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BacteriaOperations{
    //radiation:
    //damage is minimal when not stacked, but when there are lots of stacks 
    //it’s able to have time to accelerate its damage and chance to give nearby enemies 
    //sloth/flabbergast/burning/eruption
    public class Infect : MonoBehaviour
    {
        public Bacteria bac;
        public string[] bacterias;
        [Range(0,100)] public float chance;
        [SerializeField] private float populationMod;

        public void InfectHost(){
            foreach (string bacName in bacterias){
                if (Random.Range(0f,100f) <= chance + populationMod * bac.population / Mathf.Clamp(bac.population*0.5f,1,10000))
                    PublicPools.pools[bacName].SendObject(transform.parent.gameObject);
            }
        }
    }
}