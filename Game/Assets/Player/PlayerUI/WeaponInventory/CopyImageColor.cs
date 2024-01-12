using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CopyImageColor : MonoBehaviour
{
    [SerializeField] private Image imageToCopy;
    [SerializeField] private Image imageToPaste;

    private void Update(){
        imageToPaste.color = imageToCopy.color;
    }
}
