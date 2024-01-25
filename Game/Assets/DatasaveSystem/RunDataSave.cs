using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class RunDataSave : MonoBehaviour
{
    public static RunData rData;


    private static string path = "";
    private static string persistentPath = "";

    private void Awake()
    {
        SetPaths();
        if (File.Exists(path))
        {
            UpdateData();
        }
    }

    public static void CreateData()
    {
        RemoveData();
        rData = new RunData();
        UpdateJsonData();
        UpdateData();
    }

    public static void UpdateJsonData()
    {
        string savePath = path;

        Debug.Log("Saving rData to path: "+ savePath);

        string json = JsonUtility.ToJson(rData);
        Debug.Log(json);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public static void UpdateData()
    {
        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();

        rData = JsonUtility.FromJson<RunData>(json);
        //Debug.Log(pData.ToString());
    }

    public static void RemoveData(){
        if (File.Exists(path))
            File.Delete(path);
        rData = null;
    }

    private static void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "RunData.json";

        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "RunData.json";
    }
}
