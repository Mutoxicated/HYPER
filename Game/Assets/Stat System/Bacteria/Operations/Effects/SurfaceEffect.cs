using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Conditional;

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

    private void AddEffect(){
        if (sineWave != null)
            sineWave.enabled = true;
        bac.immuneSystem.stats.conditionals[SURFACE_FXED] = true;
        foreach(var part in bac.immuneSystem.injector.bodyParts){
            if (!part.surfaceEffectable)
                continue;
            var materials = part._renderer.sharedMaterials.ToList();
            materials.Add(mat);
            part._renderer.materials = materials.ToArray();
            mats.Add(part._renderer.materials[part._renderer.materials.Length-1]);
            if (part.particle) {
                mats[mats.Count-1].SetInt("_Rounded",1);
            }
        }
    }

    private void Update(){
        if (sineWave != null){
            sincos.x = sineWave.t * oscilatorMultiplier.x;
            sincos.y = sineWave.t * oscilatorMultiplier.y;
        }else{
            sincos.x = oscilatorMultiplier.x;
            sincos.y = oscilatorMultiplier.y;
        }

        offset.x += direction.x+sincos.x *Time.deltaTime;
        offset.y += direction.y+sincos.y *Time.deltaTime;
        //Debug.Log(offset*textureMultiplier.x);
        ApplyTextureOffset();
    }

    private void ApplyTextureOffset(){
        foreach (var mat in mats){
            //Debug.Log(bac.immuneSystem.gameObject.name);
            mat.SetTextureOffset("_NoiseTexture2",offset*textureMultiplier.x);
            mat.SetTextureOffset("_NoiseTexture1",offset*textureMultiplier.y);
        }
    }

      private void RevertTextureOffset(){
        foreach (var mat in mats){
            mat.SetTextureOffset("_NoiseTexture2",Vector2.zero);
            mat.SetTextureOffset("_NoiseTexture1",Vector2.zero);
        }
    }

    private void OnEnable(){
        if (mat == null)
            mat = Instantiate(Resources.Load<Material>(@"Materials/"+bac.ID.type.ToString()));
        AddEffect();
        if (randomizeDirection){
            direction.x = Random.Range(-2f,2f);
            direction.y = Random.Range(-2f,2f);
        }
        direction.Normalize();
        direction *= speed;
    }

    private void OnDisable(){
        if (sineWave != null)
            sineWave.enabled = false;
        RevertTextureOffset();
        bac.immuneSystem.stats.conditionals[SURFACE_FXED] = false;
        for (int i = 0; i < bac.immuneSystem.injector.bodyParts.Count; i++){
              if (!bac.immuneSystem.injector.bodyParts[i].surfaceEffectable)
                continue;
            var materials = bac.immuneSystem.injector.bodyParts[i]._renderer.sharedMaterials.ToList();

            materials.Remove(mats[i]);

            bac.immuneSystem.injector.bodyParts[i]._renderer.materials = materials.ToArray();
        }
        mats.Clear();
        enabled = false;
    }
}
