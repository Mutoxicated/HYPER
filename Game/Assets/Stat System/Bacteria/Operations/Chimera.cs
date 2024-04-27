using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BacteriaOperations {
    public class Chimera : MonoBehaviour
    {
        private ChimeraEffect chimera;
        public Bacteria bacteria;
        public ParticleSystem _particleSystem;
        [HideInInspector] public bool atPlayer;

        private void OnEnable()
        {
            if (transform.parent.gameObject.tag != "Player"){
                _particleSystem.Play();
                return;
            }
            atPlayer = true;
            _particleSystem.Stop();
            if (chimera == null)
            {
                chimera = GameObject.FindWithTag("MainCamera").GetComponent<ChimeraEffect>();
            }
            chimera.enabled = true;
        }

        public void EndChimera()
        {
            if (transform.parent.gameObject.tag != "Player"){
                return;
            }
            chimera.EndEffect();
        }

        private void OnDisable()
        {
            EndChimera();
        }
    }
}