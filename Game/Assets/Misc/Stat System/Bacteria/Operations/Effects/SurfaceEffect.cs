using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SurfaceEffect : MonoBehaviour
{
    [SerializeField] private Bacteria bac;
    [SerializeField] private SineWave sineWave;
    [SerializeField] private float speed;
    [SerializeField] private bool randomizeDirection;
    [SerializeField] private Vector2 direction;
    [SerializeField] private Vector2 oscilatorMultiplier;
    [SerializeField] private Vector2 textureMultiplier;
    private Vector2 offset;
    private Vector2 sincos;
    private List<Material> mats = new List<Material>();
    private Material mat;

    private void Awake(){
        mat = Instantiate(Resources.Load<Material>(@"Materials/"+bac.type.ToString()));
        mat.name += " (Instance)";
    }

    private void AddEffect(){
        foreach(var _renderer in bac.immuneSystem.injector.bodyParts){
            mats.Add(mat);
            
            var materials = _renderer.sharedMaterials.ToList();
            materials.Add(mat);
            _renderer.materials = materials.ToArray();
        }
    }

    private void Update(){
        sincos.x = sineWave.t * oscilatorMultiplier.x;
        sincos.y = sineWave.t * oscilatorMultiplier.y;

        offset.x += direction.x+sincos.x *Time.deltaTime;
        offset.y += direction.y+sincos.y *Time.deltaTime;
        //Debug.Log(offset*textureMultiplier.x);
        ApplyTextureOffset();
    }

    private void ApplyTextureOffset(){
        foreach (Material mat in mats){
            mat.SetTextureOffset("_NoiseTexture2",offset*textureMultiplier.x);
            mat.SetTextureOffset("_NoiseTexture1",offset*textureMultiplier.y);
        }
    }

      private void RevertTextureOffset(){
        foreach (Material mat in mats){
            mat.SetTextureOffset("_NoiseTexture2",Vector2.zero);
            mat.SetTextureOffset("_NoiseTexture1",Vector2.zero);
        }
    }

    private void OnEnable(){
        Invoke("AddEffect",.01f);
        if (randomizeDirection){
            direction.x = Random.Range(-2f,2f);
            direction.y = Random.Range(-2f,2f);
        }
        direction.Normalize();
        direction *= speed;
    }

    private void OnDisable(){
        RevertTextureOffset();
        for (int i = 0; i < bac.immuneSystem.injector.bodyParts.Count; i++){
            var materials = bac.immuneSystem.injector.bodyParts[i].sharedMaterials.ToList();

            materials.Remove(mats[i]);

            bac.immuneSystem.injector.bodyParts[i].materials = materials.ToArray();
        }
    }
}
