using UnityEngine;
using System.IO;
using System.Collections.Generic;

public static class JsonParser
{
    public static string DEFAULT_WAVES_FILENAME = "DefaultWaves.json";
    private static string WAVES_DATA_PATH = "/Data/Waves/";

    /// <summary>
    /// Read file from Assets/Data/Waves/[fileName] and parse it to WaveList
    /// </summary>
    /// <param name="fileName">[Name].[Extention] ex.: "EnemyWaves.json"</param>
    /// <returns></returns>
    public static WaveList ReadWavesFile(string fileName)
    {
        string json = File.ReadAllText(Application.dataPath + WAVES_DATA_PATH + fileName);
        return JsonUtility.FromJson<WaveList>(json);
    }

    /// <summary>
    /// Read file from Assets/Data/Waves/DefaultWaves.json and parse it to WaveList
    /// </summary>
    /// <returns></returns>
    public static WaveList ReadWavesFile()
    {
        string json = File.ReadAllText(Application.dataPath + WAVES_DATA_PATH + DEFAULT_WAVES_FILENAME);
        return JsonUtility.FromJson<WaveList>(json);
    }

    /// <summary>
    /// Write file to Assets/Data/Waves/[fileName] that contains JSON waves info
    /// </summary>
    /// <param name="waves">WaveList that contains waves info</param>
    /// <param name="fileName">[Name].[Extention] ex.: "EnemyWaves.json"</param>
    public static void WriteWavesFile(object waves, string fileName) 
    {
        if (fileName.Equals(DEFAULT_WAVES_FILENAME))
        {
            Debug.LogWarning("Disallowed to change default waves configuration");
            return;
        }
        string json = JsonUtility.ToJson(waves);
        File.WriteAllText(Application.dataPath + WAVES_DATA_PATH + fileName, json);
    }
}