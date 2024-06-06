using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ClassInfoUI : MonoBehaviour
{
    public static ClassInfoUI ciui;

    [SerializeField] private Transform canvas;
    [SerializeField] private ShowClasses inventory;
    [SerializeField] private ActualItemSorter ais;
    [Header("Class")]
    [SerializeField] private Transform classHolder;
    [SerializeField] private TMP_Text classDesc;

    private Class currentClass;
    private float scaling = 1.5f;
    private bool open = false;

    private void Awake() {
        ciui = this;
        gameObject.SetActive(false);
    }

    public void Open(Class _class, Vector3 pos) {
        if (open) {
            Close();
        }
        open = true;
        var curPos = gameObject.transform.position;
        Debug.Log("Pos z given: "+pos.z);
        curPos.z = Mathf.Clamp(pos.z,-21.4f,-18.9f);
        gameObject.transform.position = curPos;

        currentClass = _class;
        currentClass.Interactable(false);
        Debug.Log("Class has "+currentClass.classItems.Count+" items.");
        foreach (var classItem in currentClass.classItems) {
            classItem.EnableUIMode(canvas);
            classItem.SellMode(true);
            ais.AddItem(classItem.transform);
        }
        ais.PrepareValues();
        ais.SortItems();

        currentClass.GoTo(classHolder);
        currentClass.transform.localScale *= scaling;
        classDesc.text = currentClass.PapersPlease().desc;
        
        gameObject.SetActive(true);
    }

    public void Close() {
        if (!open) return;
        open = false;
        currentClass.transform.localScale /= scaling;
        currentClass.GoBackToParent();
        currentClass.GoBackToPos();
        currentClass.Interactable(true);
        gameObject.SetActive(false);
        
        ais.RevertItemScaling();

        foreach (var classItem in currentClass.classItems) {
            classItem.DisableUIMode();
            classItem.SellMode(false);
        }

        ais.ClearItems();
    }
}
