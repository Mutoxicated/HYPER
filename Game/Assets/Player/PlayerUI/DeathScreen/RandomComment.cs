using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RandomComment : MonoBehaviour
{
    [SerializeField] private string[] comments;

    [SerializeField] private TMP_Text comment;

    private void OnEnable(){
        comment.text = comments[Random.Range(0,comments.Length-1)];
    }
}
