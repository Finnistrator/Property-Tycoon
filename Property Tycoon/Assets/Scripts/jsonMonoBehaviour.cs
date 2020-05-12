using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

/// <summary>
/// Class: jsonMonoBehaviour
/// </summary>
public class jsonMonoBehaviour : MonoBehaviour
{
    private List<PropertyImport> properties;

    void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Method: LoadProperties()
    /// --------------------------------------------------------------
    /// Loads the property in to the game
    /// </summary>
    /// <param name="path"></param>
    public void LoadProperties(string path)
    {
        string myloadMyProperty = JsonFileReader.LoadJsonAsResource(path);
        PropertyImport property = JsonUtility.FromJson<PropertyImport>(myloadMyProperty);
        properties.Add(property);
    }

}
