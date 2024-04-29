using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

[Serializable]
public class PlatformInfo {
    public PlatformInfo(Vector3 mainPosition,Vector3 mainScale, Quaternion rotation){
        this.mainPosition = mainPosition;
        this.mainScale = mainScale;
        this.rotation = rotation;
    }

    public Vector3 mainPosition;
    public Quaternion rotation;
    public Vector3 mainScale;
    public Vector3[] spawnPoints;
}

public class PlatformGenerator : MonoBehaviour
{
    public static PlatformGenerator PG;
    [Header("Generation Settings")]
    [SerializeField] private float Yoffset;
    [SerializeField] private Vector2 minMaxPlatformXScale;
    [SerializeField] private Vector2 minMaxPlatformYScale;
    [SerializeField] private float platformZScaleOffset;
    [SerializeField] private float distTolerance;
    [SerializeField] private int[] generationCycle;
    [Header("World pieces")]
    [SerializeField] private GameObject platformHolder;
    [Space]
    [SerializeField] private GameObject mainPlat;
    [SerializeField] private GameObject[] randomPlatformProps;
    [SerializeField] private GameObject[] poles;
    [Space]
    [Header("Misc")]
    [SerializeField] private PlatformEnhancer pe;
    [SerializeField] private bool platformActive;
    [SerializeField] private GameObject platformsHolder;

    private int maxPlatforms;
    private int extraLowerPlatformChance;
    private Scroll cycle;

    private List<PlatformInfo> oldPlatformData = new List<PlatformInfo>(){};

    private List<PlatformInfo> unusedPlatforms = new List<PlatformInfo>(){
        new PlatformInfo(new Vector3(-67,-10,-106),new Vector3(70,2,70), Quaternion.identity),
        new PlatformInfo(new Vector3(71,-10,-110),new Vector3(55,2,40), Quaternion.identity),
        new PlatformInfo(new Vector3(0,0,0),new Vector3(70,2,70), Quaternion.identity)
    };

    List<Vector3> preInstantiatedPlatformPositions = new List<Vector3>(){
        new Vector3(-67,-10,-106),
        new Vector3(71,-10,-110),
        new Vector3(0,0,0)
    };

    private PlatformInfo FindPlatform(){
        return unusedPlatforms[0];
    }

    private PlatformInfo FindNeighborPlatformRelative(PlatformInfo relativePlat, PlatformInfo blacklistedPlat){
        float currentDist = 999999f;
        PlatformInfo closestPlat;
        if (oldPlatformData.Count != 0)
            closestPlat = oldPlatformData[SeedGenerator.random.Next(0,oldPlatformData.Count-1)];
        else closestPlat = unusedPlatforms[SeedGenerator.random.Next(0,unusedPlatforms.Count-1)];
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
        foreach (PlatformInfo plat in unusedPlatforms){
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

    private PlatformInfo FindNeighborPlatformRelative(Vector3 pos, PlatformInfo blacklistedPlat){
        float currentDist = 999999f;
        PlatformInfo closestPlat;
        if (oldPlatformData.Count != 0)
            closestPlat = oldPlatformData[0];
        else closestPlat = unusedPlatforms[0];
        foreach (PlatformInfo plat in oldPlatformData){
            if (plat == blacklistedPlat)
                continue;
            Vector3 flattenedPos = plat.mainPosition;
            flattenedPos.y = 0f;
            pos.y = 0f;
            float dist = Vector3.Distance(flattenedPos,pos);
            if (currentDist > dist){
                currentDist = dist;
                closestPlat = plat;
            }
        }
        foreach (PlatformInfo plat in unusedPlatforms){
            if (plat == blacklistedPlat)
                continue;
            Vector3 flattenedPos = plat.mainPosition;
            flattenedPos.y = 0f;
            pos.y = 0f;
            float dist = Vector3.Distance(flattenedPos,pos);
            if (currentDist > dist){
                currentDist = dist;
                closestPlat = plat;
            }
        }
        return closestPlat;
    }

    private float GetDistanceOfObjectToScale(Vector3 scale){
        return Vector3.Distance(Vector3.zero,new Vector3(scale.x*0.5f,0f,scale.z*0.5f));
    }

    private bool DetectOverlapBetweenPlatforms(ref Vector3 pos, Vector3 scale, PlatformInfo platTwo, out float fix){
        float dist = Vector3.Distance(pos,platTwo.mainPosition);
        float minDistAllowed = (scale.x+scale.z)*0.3f+(platTwo.mainScale.x+platTwo.mainScale.z)*0.3f;
        if (Mathf.Abs(pos.y-platTwo.mainPosition.y) < platTwo.mainScale.y){
           pos.y += -(platTwo.mainScale.y*0.5f+scale.y*0.5f)*0.5f+SeedGenerator.NextFloat(-Yoffset,-Yoffset*0.1f);
        }else{
            pos.y += SeedGenerator.NextFloat(-Yoffset,Yoffset);
        }
        if (dist < minDistAllowed){
            fix = dist-minDistAllowed;
            return true;
        }else{
            fix = 0f;
            return false;
        }
    }

    private bool DetectOverlapRelative(ref Vector3 pos, Vector3 scale, out float fix){

        foreach (PlatformInfo plat in oldPlatformData){
            if (!PositionIsValid(plat.mainPosition))
                continue;
            //trying to account for scale of the plat
            if (DetectOverlapBetweenPlatforms(ref pos,scale,plat,out fix)){
                return true;
            }
        }
        foreach (PlatformInfo plat in unusedPlatforms){
            if (!PositionIsValid(plat.mainPosition))
                continue;
            if (DetectOverlapBetweenPlatforms(ref pos,scale,plat,out fix)){
                return true;
            }
        }
        fix  = 0f;
        return false;
    }

    private List<Vector3> FindPositionsThroughTriangle(List<Vector3> points, List<Vector3> scales, float minExpand, float maxExpand){
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
        float potentialFix = 0f;
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
            extrudedTheoreticalPos = halfPoint+(halfPoint-center).normalized*SeedGenerator.NextFloat(scaleAvg+minExpand,scaleAvg+maxExpand);
            Debug.Log("Theoretical pos: "+extrudedTheoreticalPos);
            if (Vector3.Distance(Vector3.zero,extrudedTheoreticalPos) < Vector3.Distance(Vector3.zero,theoreticalPosition)){
                Debug.Log("!!!! Was going backwards !!!!");
                failedIt = i;
                theoreticalPositions.Add(Vector3.zero);
                continue;
            }
            if (!DetectOverlapRelative(ref extrudedTheoreticalPos,scales[i], out potentialFix)){
                theoreticalPositions.Add(extrudedTheoreticalPos);
            }else{
                extrudedTheoreticalPos += (halfPoint-center).normalized*(potentialFix+3f);
                Debug.Log("MODIFED theoretical pos: "+extrudedTheoreticalPos);
                scales[i] *= 0.9f;
                if (DetectOverlapRelative(ref extrudedTheoreticalPos,scales[i],out potentialFix)){
                    theoreticalPositions.Add(Vector3.zero);
                    continue;
                }
                theoreticalPositions.Add(extrudedTheoreticalPos);
            }
        }
        // if (failedIt != 0){
        //     theoreticalPosition = center;

        //     if (!DetectOverlapRelative(ref theoreticalPosition,scales[failedIt],out potentialFix)){
        //         theoreticalPositions[failedIt] = theoreticalPosition;
        //     }else{
        //         Vector3 theoreticalScale = new Vector3(scales[failedIt].x-(potentialFix)*0.5f,
        //                                                 scales[failedIt].y,
        //                                                 scales[failedIt].z-(potentialFix)*0.5f);
        //         Debug.Log("MODIFED scale: "+theoreticalScale);
        //         if ((theoreticalScale.x+theoreticalScale.z)*0.5f-1f <= minMaxPlatformXScale.x){
        //             return theoreticalPositions;
        //         }else{
        //             scales[failedIt] = theoreticalScale;
        //             theoreticalPositions[failedIt] = theoreticalPosition;
        //         }
        //     }
        // }
        return theoreticalPositions;
    }

    private List<Vector3> CreateScales(){
        List<Vector3> scales = new List<Vector3>();
        for (int i = 0; i<3; i++){
            Vector3 mainScale = new Vector3(SeedGenerator.NextFloat(minMaxPlatformXScale.x,minMaxPlatformXScale.y),
                                SeedGenerator.NextFloat(minMaxPlatformYScale.x,minMaxPlatformYScale.y),
                                0f);
            mainScale.z = mainScale.x+SeedGenerator.NextFloat(-platformZScaleOffset,platformZScaleOffset);
            scales.Add(mainScale);
        }
        return scales;
    }

    private void RegeneratePlatform(Vector3 position, Vector3 scale, Quaternion rotation){
        GameObject pHolder = Instantiate(platformHolder,position,rotation);
        GameObject main = Instantiate(mainPlat,position,Quaternion.identity);
        pe.GetObjective(main.GetComponent<PlatformObjective>());
        pHolder.transform.SetParent(platformsHolder.transform,true);
        main.transform.localScale = scale;
        main.transform.parent = pHolder.transform;
    }

    private void CreatePlatforms(Vector3[] positions, Vector3[] scales, int cap){
        int amountCreated = 0;
        for (int i = 0; i < positions.Length; i++){
            if (positions[i] == Vector3.zero){
                continue;
            }
            GameObject pHolder = Instantiate(platformHolder,positions[i],Quaternion.identity);
            GameObject main;
            if (SeedGenerator.NextFloat(0f,10f) < 2f) {
                main = Instantiate(mainPlat,positions[i],Quaternion.Euler(0f,SeedGenerator.NextFloat(0,359),0f));
            }else {
                main = Instantiate(mainPlat,positions[i],Quaternion.identity);
            }
            pe.GetObjective(main.GetComponent<PlatformObjective>());
            pHolder.transform.SetParent(platformsHolder.transform,true);
            main.transform.localScale = scales[i];
            main.transform.parent = pHolder.transform;
            //DontDestroyOnLoad(pHolder);
            unusedPlatforms.Add(new PlatformInfo(positions[i],scales[i], main.transform.rotation));
            amountCreated++;
            if (amountCreated >= cap){
                return;
            }
            if (SeedGenerator.NextFloat(0,101) <= extraLowerPlatformChance){
                //add extra platform
            }
        }
    }

    private void RemoveFromUnsearched(PlatformInfo plat){
        if (unusedPlatforms.Contains(plat))
            unusedPlatforms.Remove(plat);
        if (!oldPlatformData.Contains(plat))
            oldPlatformData.Add(plat);
    }

    public List<PlatformInfo> GetNearbyPlatforms(Vector3 position){
        PlatformInfo mainPlatform = FindNeighborPlatformRelative(position,null);
        List<PlatformInfo> neighboringPlatforms = new List<PlatformInfo>{
            FindNeighborPlatformRelative(mainPlatform, mainPlatform)
        };
        neighboringPlatforms.Add(FindNeighborPlatformRelative(mainPlatform,neighboringPlatforms[0]));
        neighboringPlatforms.Add(FindNeighborPlatformRelative(mainPlatform,neighboringPlatforms[1]));
        return neighboringPlatforms;
    }

    public void GeneratePlatforms(){
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
        List<Vector3> platformPos = FindPositionsThroughTriangle(points, scales, 20f, 50f);
        CreatePlatforms(platformPos.ToArray(),scales.ToArray(), maxPlatforms);
    }

    private void RandomizeGenerationCycle(){
        for (int n = generationCycle.Length - 1; n > 0; --n)
        {
            int k = SeedGenerator.random.Next(0,generationCycle.Length-1);
            int temp = generationCycle[n];
            generationCycle[n] = generationCycle[k];
            generationCycle[k] = temp;
        }
    }

    private void Start(){
        PG = this;
        if (cycle == null)
            cycle = new Scroll(0,generationCycle.Length);

        if (RunDataSave.rData.unusedPlatforms.Count == 0)
            return;
        oldPlatformData = RunDataSave.rData.oldPlatforms;
        unusedPlatforms = RunDataSave.rData.unusedPlatforms;
        cycle.AlterIndex(RunDataSave.rData.cycleIndex);
        generationCycle = RunDataSave.rData.generationCycle;
        RegeneratePlatforms();
    }

    public bool PositionIsValid(Vector3 position){
        foreach (Vector3 pos in preInstantiatedPlatformPositions){
            if (position.x == pos.x && position.z == pos.z)
                return false;
        }
        return true;
    }

    public void RegeneratePlatforms(){
        foreach (PlatformInfo pi in oldPlatformData){
            if (!PositionIsValid(pi.mainPosition))
                continue;
            RegeneratePlatform(pi.mainPosition,pi.mainScale, pi.rotation);
        }

        foreach (PlatformInfo pi in unusedPlatforms){
            if (!PositionIsValid(pi.mainPosition))
                continue;
            RegeneratePlatform(pi.mainPosition,pi.mainScale, pi.rotation);
        }
    }

    public void UpdateRunDataPlatforms(){
        RunDataSave.rData.oldPlatforms = oldPlatformData;
        RunDataSave.rData.unusedPlatforms = unusedPlatforms;
        RunDataSave.rData.cycleIndex = cycle.index;
        RunDataSave.rData.generationCycle = generationCycle;
    }

    public void ActiveState(string sceneName, bool state){
        if (sceneName == "Interoid"){
            RandomizeGenerationCycle();
            maxPlatforms = generationCycle[cycle.index];
            GeneratePlatforms();
            cycle.Increase();
            pe.Progress();
            UpdateRunDataPlatforms();
        }
        platformsHolder.SetActive(state);
        if (state){
            pe.ApplyObjectivesToPlatforms();
        }else {
            pe.ClearObjectives();
        }
    }
}