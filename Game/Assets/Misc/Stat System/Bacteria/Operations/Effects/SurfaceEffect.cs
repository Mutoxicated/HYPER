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

    private void AddEffect(){
        foreach(var part in bac.immuneSystem.injector.bodyParts){
            if (!part.surfaceEffectable)
                continue;
            part.hasSurfaceFX = true;
            var materials = part._renderer.sharedMaterials.ToList();
            materials.Add(mat);
            part._renderer.materials = materials.ToArray();
            mats.Add(part._renderer.materials[part._renderer.materials.Length-1]);
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
            mat = Instantiate(Resources.Load<Material>(@"Materials/"+bac.type.ToString()));
        AddEffect();
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
              if (!bac.immuneSystem.injector.bodyParts[i].surfaceEffectable)
                continue;
            bac.immuneSystem.injector.bodyParts[i].hasSurfaceFX = false;
            var materials = bac.immuneSystem.injector.bodyParts[i]._renderer.sharedMaterials.ToList();

            materials.Remove(mats[i]);

            bac.immuneSystem.injector.bodyParts[i]._renderer.materials = materials.ToArray();
        }
        mats.Clear();
        enabled = false;
    }
}
