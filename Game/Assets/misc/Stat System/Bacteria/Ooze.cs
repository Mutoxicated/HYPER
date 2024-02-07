using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ooze : MonoBehaviour
{
    private static int population = 1;

    [SerializeField] private Renderer meshRenderer;
    [SerializeField] private MeshFilter meshFilter;
    [SerializeField] private Vector2 minMaxRadius;
    [SerializeField] private Vector2 minMaxSpeed;
    [SerializeField] private float lifetimeSeconds = 3f;
    [SerializeField] private string bacName;
    [SerializeField,Range(0.5f,3f)] private float populationMod;

    private float[] angles;
    private float[] radiuses;
    private float[] speeds;
    
    private Material mat;
    private Mesh mesh;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> ogVertices = new List<Vector3>();
    private Vector3 currentModifiedVert = Vector3.zero;

    private float time;
    private float t;
    private float tStart;

    private Injector inject;
    private float timeColliding;
    private float lifetime;

    private void OnEnable(){
        lifetime = lifetimeSeconds;
        time = 0f;
        timeColliding = 0f;
        t = 0f;
        population = Mathf.CeilToInt(0.10f*Difficulty.rounds*populationMod);
        mat = meshRenderer.material;
        mesh = meshFilter.mesh;
        radiuses = new float[mesh.vertexCount];
        angles = new float[mesh.vertexCount];
        speeds = new float[mesh.vertexCount];
        PrepareVariables();
        MoveVertices();
    }

    private void PrepareVariables(){
        for(int i = 0; i < speeds.Length; i++){
            radiuses[i] = Random.Range(minMaxRadius.x,minMaxRadius.y);
            speeds[i] = Random.Range(minMaxSpeed.x,minMaxSpeed.y);
            if (speeds[i] < 0.5f && speeds[i] > -0.5f){
                if (Random.Range(0f,100f) <= 50f)
                    speeds[i] = 0.5f;
                else speeds[i] = -0.5f;
            }
            angles[i] = Random.Range(-180f,180f)*Mathf.Deg2Rad;
        }
        mesh.GetVertices(ogVertices);
    }

    private void MoveVertices(){
        mesh.GetVertices(vertices);
        
        for(int i = 0; i < speeds.Length; i++){
            currentModifiedVert = ogVertices[i];
            currentModifiedVert.x += Mathf.Cos(angles[i])*radiuses[i];
            currentModifiedVert.y += Mathf.Sin(angles[i])*radiuses[i];
            vertices[i] = currentModifiedVert*t*tStart;
            angles[i] += speeds[i]*Mathf.Deg2Rad;
        }
        mesh.SetVertices(vertices);
    }
    // private void ParametricBlend(){
    //     t = (t * t) / (2 * ((t * t) - t) + 1);
    // }

    private void Update(){
        if (time > lifetime){
            gameObject.SetActive(false);
            return;
        }
        tStart = Mathf.InverseLerp(0f,0.4f,time);
        tStart = Mathf.Clamp01(tStart);
        MoveVertices();
        time += Time.deltaTime;
        t = Mathf.InverseLerp(lifetime,lifetime*0.6f,time);
        t = Mathf.Clamp01(t);
    }

    private void OnTriggerEnter(Collider coll){
        //Debug.Log("TRIGGER ENTERED!");
        inject = coll.GetComponent<Injector>();
        inject?.AddBacterias(bacName,population);
    }

    // private void OnTriggerStay(Collider coll){
    //     timeColliding += Time.deltaTime;
    //     if (timeColliding >= 0.3f){
    //         inject = coll.GetComponent<Injector>();
    //         inject?.AddBacterias(bacName,population);
    //         timeColliding = 0f;
    //     }
    // }
}
