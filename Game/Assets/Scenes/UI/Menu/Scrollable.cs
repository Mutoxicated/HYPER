using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scrollable : MonoBehaviour
{
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private GameObject holder;
    [SerializeField] private float upAmount;

    private float anchorPoint;
    private Vector3 currentPos;

    private void Start(){
        currentPos = holder.transform.position;
        anchorPoint = currentPos.y;
    }

    private void Update(){
        currentPos = holder.transform.position;
        currentPos.y = Mathf.Lerp(anchorPoint,anchorPoint+upAmount,scrollbar.value);
        holder.transform.position = currentPos;
    }
}
