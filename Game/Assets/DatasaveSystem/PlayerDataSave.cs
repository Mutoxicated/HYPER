using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerDataSave : MonoBehaviour
{
    public static PlayerData pdata;


    private static string path = "";
    private static string persistentPath = "";

    private void Awake()
    {
        SetPaths();
        if (!File.Exists(path))
        {
            CreateData();
            Debug.Log("called");
        }
        else
        {
            RetrieveData();
        }
    }

    private static void CreateData()
    {
        pdata = new PlayerData(Color.red,70f,2f,2f,false);
        UpdateData();
    }

    public static void UpdateData()
    {
        string savePath = path;

        Debug.Log("Saving pData to path: "+ savePath);

        string json = JsonUtility.ToJson(pdata);
        Debug.Log(json);

        using StreamWriter writer = new StreamWriter(savePath);
        writer.Write(json);
    }

    public static void RetrieveData()
    {
        using StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();

        pdata = JsonUtility.FromJson<PlayerData>(json);
        //Debug.Log(pData.ToString());
    }

    private static void SetPaths()
    {
        path = Application.dataPath + Path.AltDirectorySeparatorChar + "SaveData.json";

        persistentPath = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "SaveData.json";
    }
}
