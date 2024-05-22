using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassSystem : MonoBehaviour
{
    public static Dictionary<classType, Class> classDict = new Dictionary<classType, Class>();
    private static float starterEffectivenessMod = 1f;
    private static float synergizedEffectivenessMod = 1f;
    private static float HYPEREffectivenessMod = 1f;

    [SerializeField] private List<Class> classes = new List<Class>();

    public static List<Class> ClassList = new List<Class>();

    public static void Reset(){
        starterEffectivenessMod = 1f;
        synergizedEffectivenessMod = 1f;
        HYPEREffectivenessMod = 1f;
        classDict.Clear();
        ClassList.Clear();
    }

    private void Awake(){
        if (classDict.Count != 0) return;
        foreach (Class _class in classes){
            classDict.Add(_class.PapersPlease()._classType, _class);
        }
        ClassList = classes;
    }

    private void Start() {
        PlayerInfo.GetIP().AddItemsToClasses();
    }

    private static void EvaluateBoost(ClassHierarchy hierarchy, int cellAmount){
        if (cellAmount <= 0) return;
        if (hierarchy == ClassHierarchy.Starter){
            starterEffectivenessMod += cellAmount/10f;
        }else if (hierarchy == ClassHierarchy.Synergized){
            synergizedEffectivenessMod += cellAmount/10f;
        }else{
            HYPEREffectivenessMod += cellAmount/10f;
        }
    }

    public static void IncrementClassBattery(classType _classType){
        if (!classDict.ContainsKey(_classType)) return;
        int cellAmount = classDict[_classType].IncreaseBattery();
        EvaluateBoost(classDict[_classType].PapersPlease().hierarchy, cellAmount);
    }

    public static void DecrementClassBattery(classType _classType){
        if (!classDict.ContainsKey(_classType)) return;
        int cellAmount = classDict[_classType].DecreaseBattery();
        EvaluateBoost(classDict[_classType].PapersPlease().hierarchy, cellAmount);
    }
}
