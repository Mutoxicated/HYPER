using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEffect : MonoBehaviour
{
    [SerializeField] private LineRenderer liner;
    [SerializeField,Min(2)] private int vertexAmount;

    private List<Vector3> positions = new List<Vector3>();
    private Vector3 position = Vector3.zero;
    private float offset = 3f;
    private float duration = 1.5f;
    private float time;
    private Color alteredColor;
    private float t;

    private List<Vector3> alteredPoses = new List<Vector3>();

    private void CreatePositions(Vector3 end, float distFactor){
        alteredPoses.Clear();
        positions.Clear();
        position = Vector3.zero;
        transform.LookAt(end);

        int actualVertexAmount = Mathf.RoundToInt(vertexAmount*(distFactor*0.02f));
        liner.positionCount = vertexAmount;
        float zIncrement = Vector3.Distance(transform.position,end)/vertexAmount;
        for (int i = 0; i < vertexAmount;i++){
            position.z = zIncrement*i;
            positions.Add(position);
            Vector3 alteredPos = position;
            if (i != 0 | i != vertexAmount-1){
                alteredPos.x += Random.Range(-offset,offset);
                alteredPos.y += Random.Range(-offset,offset);
                //alteredPos.z += Random.Range(-zIncrement*0.5f,zIncrement*0.5f);
            }
            alteredPoses.Add(alteredPos);
        }
    }

    public void Occur(Vector3 start, Vector3 end){
        gameObject.SetActive(true);
        transform.position = start;
        CreatePositions(end,Vector3.Distance(start,end));
        liner.SetPositions(positions.ToArray());
    }

    private void Update(){
        time += Time.deltaTime;
        t = Mathf.InverseLerp(0f,duration,time);
        alteredColor = liner.startColor;
        alteredColor.a = Mathf.Lerp(1f,0f,t)*0.5f;
        liner.startColor = alteredColor;
        alteredColor = liner.endColor;
        alteredColor.a = Mathf.Lerp(1f,0f,t);
        liner.endColor = alteredColor;
        
        liner.startColor = alteredColor;
        for (int i = 0; i < positions.Count; i++){
            alteredPoses[i] = Vector3.Lerp(alteredPoses[i],positions[i],Time.deltaTime);
        }
        liner.SetPositions(alteredPoses.ToArray());
        if (time >= duration){
            gameObject.SetActive(false);
            time = 0f;
        }
    }
}
