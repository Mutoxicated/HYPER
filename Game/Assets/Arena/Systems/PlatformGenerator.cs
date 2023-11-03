using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformInfo {
    public PlatformInfo(Vector3 mainPosition,Vector3 mainScale){
        this.mainPosition = mainPosition;
        this.mainScale = mainScale;
    }

    public Vector3 mainPosition;
    public Vector3 mainScale;
}

public class PlatformGenerator : MonoBehaviour
{
    [Header("Generation Settings")]
    [SerializeField] private float Yoffset;
    [SerializeField] private Vector2 minMaxPlatformXScale;
    [SerializeField] private Vector2 minMaxPlatformYScale;
    [SerializeField] private float platformZScaleOffset;
    [SerializeField] private float distTolerance;
    [SerializeField] private int generationAmount;
    [Header("World pieces")]
    [SerializeField] private GameObject platformHolder;
    [Space]
    [SerializeField] private GameObject mainPlat;
    [SerializeField] private GameObject[] randomPlatformProps;
    [SerializeField] private GameObject[] poles;

    private int maxPlatforms;
    private int extraLowerPlatformChance;

    private float totalDistAvg;

    private static List<PlatformInfo> oldPlatformData = new List<PlatformInfo>(){
        new PlatformInfo(new Vector3(-67,-10,-106),new Vector3(70,2,70)),
        new PlatformInfo(new Vector3(71,-10,-110),new Vector3(55,2,40))
    };

    private static List<PlatformInfo> unsearchedPlatforms = new List<PlatformInfo>(){
        new PlatformInfo(new Vector3(0,0,0),new Vector3(40,2,40))
    };

    private PlatformInfo FindPlatform(){
        return unsearchedPlatforms[0];
    }

    private PlatformInfo FindNeighborPlatformRelative(PlatformInfo relativePlat, PlatformInfo blacklistedPlat){
        float currentDist = 999999f;
        PlatformInfo closestPlat = oldPlatformData[Random.Range(0,oldPlatformData.Count-1)];
        foreach (PlatformInfo plat in oldPlatformData){
            if (plat == blacklistedPlat)
                continue;
            if (plat == relativePlat)
                continue;
            Vector3 flattenedPos = plat.mainPosition;
            flattenedPos.y = 0f;
            relativePlat.mainPosition.y = 0f;
            float dist = Vector3.Distance(flattenedPos,relativePlat.mainPosition);
            if (currentDist > dist){
                currentDist = dist;
                closestPlat = plat;
            }
        }
        return closestPlat;
    }

    private float GetDistanceOfObjectToScale(Vector3 scale){
        return Vector3.Distance(new Vector3(scale.x*0.5f,0f,scale.z*0.5f), Vector3.zero);
    }

    private Vector2 FindShortestDistanceRelative(Vector3 relativePosition){
        float currentDist = 999999f;
        PlatformInfo closestPlat = oldPlatformData[Random.Range(0,oldPlatformData.Count-1)];
        foreach (PlatformInfo plat in oldPlatformData){
            //trying to account for scale of the plat
            float dist = Vector3.Distance(relativePosition,plat.mainPosition+(relativePosition-plat.mainPosition).normalized*((plat.mainScale.x+plat.mainScale.z)*0.5f));
            if (currentDist > dist){
                currentDist = dist;
                closestPlat = plat;
            }
        }
        Debug.Log("Closest plat: " + closestPlat.mainPosition);
        return new Vector2(currentDist,GetDistanceOfObjectToScale(closestPlat.mainScale));
    }

    private List<Vector3> FindPositionsThroughTriangle(List<Vector3> points, List<Vector3> scales, float minExpand, float maxExpand, int cap){
        Vector3 center = new Vector3((points[0].x+points[1].x+points[2].x)/3f,
                                     (points[0].y+points[1].y+points[2].y)/3f,
                                     (points[0].z+points[1].z+points[2].z)/3f);
        Debug.Log("Center: "+center);
        int currentIt = 0;
        int failedIt = 0;
        /* visualization:

           /\
          /  \
         ------
        
        each point of this triangle is a platform
        we take the bisector of each line, extrude the position outwards relative to the center of the triangle and check if the position is valid
        valid meaning there are no other platforms that are too close to it
        */
        List<Vector3> theoreticalPositions = new List<Vector3>();
        Vector3 theoreticalPosition;
        Vector3 extrudedTheoreticalPos;
        for (int i = 0; i < points.Count;i++){
            currentIt++;
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
            theoreticalPosition = halfPoint;
            float scaleAvg = (scales[i].x+scales[i].z)*0.5f;
            Debug.Log("Scale: "+scales[i]);
            extrudedTheoreticalPos = halfPoint+(halfPoint-center).normalized*Random.Range(scaleAvg+minExpand,scaleAvg+maxExpand);
            Debug.Log("Theoretical pos: "+extrudedTheoreticalPos);
            Vector2 distAndPlatScale = FindShortestDistanceRelative(extrudedTheoreticalPos);
            float scaleDist = GetDistanceOfObjectToScale(scales[i]);
            float minDistAllowed = (scaleDist*0.5f+distAndPlatScale.y*0.5f)*0.8f;
            Debug.Log("Minimum Distance Allowed: "+minDistAllowed + " || compared to distance between two platforms: "+distAndPlatScale.x);
            if (Vector3.Distance(extrudedTheoreticalPos,Vector3.zero) < Vector3.Distance(theoreticalPosition,Vector3.zero)){
                failedIt = i;
                theoreticalPositions.Add(Vector3.zero);
                continue;
            }
            if (distAndPlatScale.x > minDistAllowed){
                theoreticalPositions.Add(extrudedTheoreticalPos);
                if (currentIt >= cap){
                    return theoreticalPositions;
                }
            }else{
                extrudedTheoreticalPos += (halfPoint-center).normalized*(minDistAllowed-distAndPlatScale.x);
                Debug.Log("MODIFED theoretical pos: "+extrudedTheoreticalPos);
                theoreticalPositions.Add(extrudedTheoreticalPos);
                if (currentIt >= cap){
                    return theoreticalPositions;
                }
            }
        }
        if (failedIt != 0){
            theoreticalPosition = center;
            Vector2 distAndPlatScale = FindShortestDistanceRelative(theoreticalPosition);
            float scaleDist = GetDistanceOfObjectToScale(scales[failedIt]);
            float minDistAllowed = (scaleDist*0.5f+distAndPlatScale.y*0.5f)*0.8f;
            if (distAndPlatScale.x > minDistAllowed){
                theoreticalPositions.Add(theoreticalPosition);
            }else{
                Vector3 theoreticalScale = new Vector3(scales[failedIt].x-(minDistAllowed-distAndPlatScale.x)*0.5f,
                                                        scales[failedIt].y,
                                                        scales[failedIt].z-(minDistAllowed-distAndPlatScale.x)*0.5f);
                Debug.Log("MODIFED scale: "+theoreticalScale);
                if ((theoreticalScale.x+theoreticalScale.z)*0.5f-1f <= minMaxPlatformXScale.x){
                    return theoreticalPositions;
                }else{
                    scales[failedIt] = theoreticalScale;
                    theoreticalPositions[failedIt] = theoreticalPosition;
                }
            }
        }
        return theoreticalPositions;
    }

    private List<Vector3> CreateScales(){
        List<Vector3> scales = new List<Vector3>();
        for (int i = 0; i<3; i++){
            Vector3 mainScale = new Vector3(Random.Range(minMaxPlatformXScale.x,minMaxPlatformXScale.y),
                                Random.Range(minMaxPlatformYScale.x,minMaxPlatformYScale.y),
                                0f);
            mainScale.z = mainScale.x+Random.Range(-platformZScaleOffset,platformZScaleOffset);
            scales.Add(mainScale);
        }
        return scales;
    }

    private void CreatePlatforms(Vector3[] positions, Vector3[] scales){
        for (int i = 0; i < positions.Length; i++){
            if (positions[i] == Vector3.zero){
                continue;
            }
            positions[i].y = positions[i].y+Random.Range(-Yoffset,Yoffset);
            GameObject pHolder = Instantiate(platformHolder,positions[i],Quaternion.identity);
            GameObject main = Instantiate(mainPlat,positions[i],Quaternion.identity);
            main.transform.localScale = scales[i];
            main.transform.parent = pHolder.transform;
            DontDestroyOnLoad(pHolder);
            unsearchedPlatforms.Add(new PlatformInfo(positions[i],scales[i]));
            if (Random.Range(0,101) <= extraLowerPlatformChance){
                //add extra platform
            }
        }
    }

    private void RemoveFromUnsearched(PlatformInfo plat){
        if (unsearchedPlatforms.Contains(plat))
            unsearchedPlatforms.Remove(plat);
        if (!oldPlatformData.Contains(plat))
            oldPlatformData.Add(plat);
    }

    public void GeneratePlatforms(int cap){
        PlatformInfo mainPlatform = FindPlatform();
        List<PlatformInfo> neighboringPlatforms = new List<PlatformInfo>();
        RemoveFromUnsearched(mainPlatform);
        neighboringPlatforms.Add(FindNeighborPlatformRelative(mainPlatform,mainPlatform));
        neighboringPlatforms.Add(FindNeighborPlatformRelative(mainPlatform,neighboringPlatforms[0]));
        Debug.Log("platforms: (1) "+mainPlatform.mainPosition+" (2) "+ neighboringPlatforms[0].mainPosition+" (3) "+neighboringPlatforms[1].mainPosition);
        List<Vector3> points = new List<Vector3>(){
            mainPlatform.mainPosition,
            neighboringPlatforms[0].mainPosition,
            neighboringPlatforms[1].mainPosition
        };
        List<Vector3> scales = CreateScales();
        List<Vector3> platformPos = FindPositionsThroughTriangle(points, scales, 20f, 50f,cap);
        CreatePlatforms(platformPos.ToArray(),scales.ToArray());
    }

    private void Start(){
        maxPlatforms = generationAmount;
        GeneratePlatforms(maxPlatforms);
    }
}
