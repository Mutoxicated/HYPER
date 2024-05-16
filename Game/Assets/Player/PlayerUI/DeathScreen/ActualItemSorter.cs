using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualItemSorter : MonoBehaviour
{
    [SerializeField] private Transform[] Width;
    [SerializeField] private Transform[] Height;

    [SerializeField] private List<Transform> items = new List<Transform>();
    [SerializeField] private Vector2 direction;

    [SerializeField] private float scaleMod = 1f;
    [SerializeField] private float transformScaleMod = 1f;
    [SerializeField] private float radiusReference = 3.5f;
    [SerializeField] private float maxSize = 8f;

    private float yIncrement;
    private float widthAvailable;
    private float heightAvailable;
    private float scaleOffset = 0.9f;

    private int lineGeneration = 2;
    private int columnGeneration;

    private int maxItemsGenerationable;
    private int limit = 10;
    private int recursionCount = 0;

    private Vector3 alteredPos;
    private Vector3 initScale;

    private void FindSmallestLineGenerationAvailable(){

        if (recursionCount > limit){
            return;
        }
        yIncrement = heightAvailable/lineGeneration;
        if (yIncrement > maxSize){
            yIncrement = maxSize;
        }

        Debug.Log("Line Gen:"+lineGeneration+",Y increment:"+yIncrement);
        columnGeneration = Mathf.FloorToInt(widthAvailable/yIncrement);
        maxItemsGenerationable = lineGeneration*columnGeneration;
        if (items.Count > maxItemsGenerationable){
            lineGeneration++;
            recursionCount++;
            FindSmallestLineGenerationAvailable();
        }
    }

    private void OnEnable(){
        if (items.Count == 0)
            return;
        PrepareValues();

        //Debug.Log("HeightAvail: "+heightAvailable+", WidthAvail: "+widthAvailable);

        SortItems();
    }

    private void SortItem(int lineIndex,int columnIndex, int itemIndex){
        items[itemIndex].position = Height[1].position;

        alteredPos = items[itemIndex].localPosition;

        //apply offset
        alteredPos.x += direction.x*(yIncrement*0.5f)*scaleMod;
        alteredPos.y += direction.y*(yIncrement*0.5f)*scaleMod;

        //apply offset based on current line index and current column index
        alteredPos.x += direction.x*(yIncrement*(columnIndex-1))*scaleMod;
        alteredPos.y += direction.y*(yIncrement*(lineIndex-1))*scaleMod;

        items[itemIndex].localPosition = alteredPos;

        float t = Mathf.InverseLerp(0f,radiusReference,yIncrement*0.5f);
        items[itemIndex].localScale = initScale*t*scaleOffset*transformScaleMod;
    }

    public void RevertItemScaling() {
        foreach (var item in items) {
            item.localScale = initScale;
        }
    }

    public void RevertItemScaling(Transform trans) {
        trans.localScale = initScale;
    }

    public void SortItems(){
        int currentline = 1;
        int currentColumn = 1;
        for (int i = 0; i < items.Count; i++){
            SortItem(currentline,currentColumn,i);
            currentColumn++;
            if (currentColumn > columnGeneration){
                currentline++;
                currentColumn = 1;
            }
        }
    }

    public void AddItem(Transform trans){
        items.Add(trans);
    }

    public void RemoveItem(Transform trans) {
        items.Remove(trans);
    }

    public void ClearItems() {
        items.Clear();
    }

    public void PrepareValues(){
        if (items.Count == 0) return;
        initScale = items[0].localScale;
        heightAvailable = Height[1].localPosition.y-Height[0].localPosition.y;
        widthAvailable = Width[1].localPosition.x-Width[0].localPosition.x;

        Debug.Log("HeightAvail: "+heightAvailable+", WidthAvail: "+widthAvailable);

        FindSmallestLineGenerationAvailable();
    }
}
