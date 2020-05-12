using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class: JsonFileReader()
/// </summary>
public class JsonFileReader
{
    /// <summary>
    /// Method: LoadJsonAsResource()
    /// ------------------------------------------------
    /// Loads Json files as resource
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string LoadJsonAsResource(string path)
    {
        string jsonFilePath = path.Replace(".json", "");
        TextAsset loadJsonfile = Resources.Load<TextAsset>(jsonFilePath); 
        return loadJsonfile.text;
    }
}
