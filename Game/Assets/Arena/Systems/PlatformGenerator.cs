using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInfo {
    public PlatformInfo(Vector3 mainPosition,Vector3 mainScale,List<GameObject> objects){
        this.mainPosition = mainPosition;
        this.mainScale = mainScale;
        this.objects = objects;
    }

    public Vector3 mainPosition;
    public Vector3 mainScale;
    public List<GameObject> objects;
}

public class PlatformGenerator : MonoBehaviour
{
    [SerializeField] private Vector2 minMaxPlatformScale;//x is min, y is max
    [SerializeField] private Vector2 minMaxPlatformOffsetScale;//here each coord refers to the max offset value, so x would be computed as a min of -x and max of +x
    [SerializeField] private GameObject[] platformPrefabs;
    [SerializeField] private int generationAmount;

    private int maxPlatforms;

    private float totalDistAvg;
    private PlatformInfo firstPlat = new PlatformInfo(new Vector3(0,0,0),new Vector3(40,2,40),new List<GameObject>());

    private static List<PlatformInfo> platformData = new List<PlatformInfo>();

    private void CalculateTotalDistAvg(){
        totalDistAvg = 0;
        foreach (PlatformInfo plat in platformData){
            totalDistAvg += (plat.mainPosition.x+plat.mainPosition.z)/platformData.Count;
        }
    }

    private PlatformInfo FindFarthestPlatform(float acceptingRange){
        CalculateTotalDistAvg();
        PlatformInfo farthestPlat = platformData[Random.Range(0,platformData.Count+1)];
        foreach (PlatformInfo plat in platformData){
            float distAvg = (plat.mainPosition.x+plat.mainPosition.z)*0.5f;
            if (distAvg > totalDistAvg-acceptingRange && distAvg < totalDistAvg+acceptingRange){
                farthestPlat = plat;
            }
        }
        return farthestPlat;
    }

    private PlatformInfo FindNeighborPlatformRelative(PlatformInfo relativePlat, PlatformInfo blacklistedPlat){
        float currentDist = 0;
        PlatformInfo closestPlat = platformData[Random.Range(0,platformData.Count+1)];
        foreach (PlatformInfo plat in platformData){
            if (plat == blacklistedPlat)
                continue;
            if (plat == relativePlat)
                continue;
            float dist = Vector2.Distance(relativePlat.mainPosition,plat.mainPosition);
            if (currentDist > dist){
                currentDist = dist;
                closestPlat = plat;
            }
        }
        return closestPlat;
    }

    private float FindShortestDistanceRelative(Vector3 relativePosition, List<PlatformInfo> blacklistedPlat){
        float currentDist = 0;
        PlatformInfo closestPlat;
        foreach (PlatformInfo plat in platformData){
            if (blacklistedPlat.Contains(plat))
                continue;
            //trying to account for scale of the plat
            float dist = Vector2.Distance(relativePosition,plat.mainPosition+(plat.mainPosition-relativePosition).normalized*(((plat.mainScale.x+plat.mainScale.z)*0.5f)*0.5f));
            if (currentDist > dist){
                currentDist = dist;
                closestPlat = plat;
            }
        }
        return currentDist;
    }

    private Vector4 FindPositionThroughTriangle(List<Vector3> points, float minExpand, float maxExpand, float distMaxTolerance){
        Vector3 center = new Vector3((points[0].x+points[1].x+points[2].x)/3f,
                                     (points[0].y+points[1].y+points[2].y)/3f,
                                     (points[0].z+points[1].z+points[2].z)/3f);
        
        /* visulization:

           /\
          /  \
         ------
        
        each point of this triangle is a platform
        we take the bisector of each line, extrude the position outwards relative to the center of the triangle and check if the position is valid
        valid meaning there are no other platforms that are too close to it
        */

        for (int i = 0; i < points.Count;i++){
            Vector3 halfPoint;
            if (i+1 == points.Count){
                halfPoint = new Vector3((points[i].x+points[0].x)/2,
                        (points[i].y+points[0].y)/2,
                        (points[i].z+points[0].z)/2);
            }else{
                halfPoint = new Vector3((points[i].x+points[i+1].x)/2,
                        (points[i].y+points[i+1].y)/2,
                        (points[i].z+points[i+1].z)/2);
            }
            Vector3 theoreticalPosition = halfPoint+(halfPoint-center).normalized*Random.Range(minExpand,maxExpand);
            float dist = FindShortestDistanceRelative(theoreticalPosition,new List<PlatformInfo>());
            if (dist <= distMaxTolerance && i != points.Count-1){
                continue;
            }else{
                return new Vector4(theoreticalPosition.x,theoreticalPosition.y,theoreticalPosition.z,dist);
            }
        }
        return new Vector4(0,0,0,0);
    }

    private void GeneratePlatform(float Ymin, float Ymax){
        generationAmount--;
        PlatformInfo mainPlatform = FindFarthestPlatform(4f);
        List<PlatformInfo> neighboringPlatforms = new List<PlatformInfo>();
        neighboringPlatforms.Add(FindNeighborPlatformRelative(mainPlatform,mainPlatform));
        neighboringPlatforms.Add(FindNeighborPlatformRelative(mainPlatform,neighboringPlatforms[0]));
        List<Vector3> points = new List<Vector3>(){
            mainPlatform.mainPosition,
            neighboringPlatforms[0].mainPosition,
            neighboringPlatforms[1].mainPosition
        };
        Vector4 platformPos = FindPositionThroughTriangle(points, 6f, 17f, 30f);
        //create platform with scale according to the 4th coord that is the closest dist of a platform
    }

    private void Start(){
        if (!platformData.Contains(firstPlat)){
            platformData.Add(firstPlat);
        }
        maxPlatforms = generationAmount;
        
    }
}
