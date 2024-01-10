using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BacteriaOperations{
    public class Damage : MonoBehaviour
    {
        public Bacteria bac;
        public float damage;
        public int hurtScreenIndex;
        [SerializeField] private bool exponential;
        private int _;
        private IDamageable cachedDamageable;

        public void DamageEntity()
        {
            if (exponential)
                cachedDamageable.TakeDamage(damage*bac.population,null, ref _, 0.1f, hurtScreenIndex);
            else
                cachedDamageable.TakeDamage(damage*(bac.population*Mathf.Clamp(bac.population*0.5f,1,10000)),null, ref _, 0.1f, hurtScreenIndex);
        }
        // Start is called before the first frame update
        private void OnEnable()
        {
            cachedDamageable = GetComponentInParent<IDamageable>();
        }
    }
}