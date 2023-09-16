using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitiliazeCamDepth : MonoBehaviour
{
    private void Start()
    {
        Camera cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth;
    }
}
