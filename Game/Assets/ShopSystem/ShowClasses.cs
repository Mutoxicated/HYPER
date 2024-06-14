using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowClasses : MonoBehaviour
{
    [SerializeField] private Transform anchor;

    [Header("Settings")]
    [SerializeField]private float lineSpacing = 120;
    [SerializeField] private float starterXLength = 240;
    [SerializeField] private float synergizedXLength = 300;
    [SerializeField] private float hyperXLength = 600;
    [Header("Offsets")]
    [SerializeField] private float scaleOffset;
    [SerializeField] private float xOffset;

    private float starterStep;
    private float synergizedStep;
    private float hyperStep;

    private Vector3 curPos;
    private bool alreadyShowed;

    private List<Class> starters;
    private List<Class> synergizers;
    private List<Class> hypers;

    private void Awake() {
        starterStep = starterXLength/4;
        synergizedStep = synergizedXLength/5;
        hyperStep = hyperXLength/10;

        starters = ClassSystem.ClassList.GetRange(0, 4);
        synergizers = ClassSystem.ClassList.GetRange(4, 5);
        hypers = ClassSystem.ClassList.GetRange(9, 10);
    }

    private void SetupClassHierarchy(List<Class> classes,float hierarchyLength, float hierarchyStep, int line) {
        curPos = anchor.position;
        curPos.x -= hierarchyLength*0.5f+xOffset;
        curPos.y -= lineSpacing*line;

        foreach (var _class in classes) {
            _class.GoTo(anchor, curPos, true);
            _class.transform.localScale = _class.transform.localScale*scaleOffset;
            curPos.x += hierarchyStep;
        }
    }

    public void Show() {
        SetupClassHierarchy(starters, starterXLength, starterStep, 0);
        SetupClassHierarchy(synergizers, synergizedXLength, synergizedStep, 1);
        SetupClassHierarchy(hypers, hyperXLength, hyperStep, 2);
        alreadyShowed = true;
    }

    private void OnEnable() {
        if (!alreadyShowed)
            Show();
    }

    private void OnDisable() {
        foreach (var _class in ClassSystem.ClassList) {
            _class.transform.localScale = _class.transform.localScale/scaleOffset;
            _class.GoBack();
        }
        alreadyShowed = false;
    }
}
