using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    [SerializeField] private Transform transformRef;
    [SerializeField] private YCheck yCheck;
    [SerializeField] private TeleportEffect tpe;

    private List<PlatformInfo> platformInfos = new List<PlatformInfo>();
    private Vector3 previousPos;
    private bool outOfHome = false;

    private void Effect(Vector3 start, Vector3 end){
        tpe.Occur(start,end);
    }
    
    public void TeleportToPLatform(){
        if (outOfHome){
            Effect(transformRef.position,previousPos);
            transformRef.position = previousPos;
            outOfHome = false;
            return;
        }

        previousPos = transformRef.position;
        platformInfos = PlatformGenerator.PG.GetNearbyPlatforms(transformRef.position);
        PlatformInfo chosenPlatform = platformInfos[Random.Range(0,platformInfos.Count)];
        Vector3 platformPos = chosenPlatform.mainPosition;
        float radius = (chosenPlatform.mainScale.x+chosenPlatform.mainScale.z)*0.125f;
        platformPos.y += 6f;
        platformPos.x += Random.Range(-radius,radius);
        platformPos.z += Random.Range(-radius,radius);
        transformRef.position = platformPos;
        transformRef.position = yCheck.VerifyPos(transformRef.position);
        outOfHome = true;
        Effect(previousPos,transformRef.position);
    }
}
