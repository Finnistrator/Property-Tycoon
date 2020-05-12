using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using System.Linq;

public class MainMenu : Singleton<MainMenu>
{
    [SerializeField] private MainMenuAnimationController animationController;
    [SerializeField] private GameObject abridgedMode;
    [SerializeField] private InputField abridgedLimit;

    private bool abridged;
    private int timeLimit;

    [SerializeField] private AudioMixer mixer;
    public void PlayGame()
    {
        abridged = abridgedMode.activeSelf;
        timeLimit = GetTimeLimit() * 60;

        for (int i = 0; i < InitialGameSettings.NumOfPlayers; i++)
        {
            InitialGameSettings.players[i] = animationController.PlayersPrefabs[i].getPlayerSettings();
        }
        //SceneManager.LoadScene("MainGame");
        StartLoading();
        ActivateScene();


    }

    public int GetLimit()
    {
        return timeLimit;
    }

    private int GetTimeLimit()
    {
        if(int.TryParse(abridgedLimit.text, out int result))
        {
            return result;
        }

        return 5;
    }

    public void ToggleAbridged()
    {
        abridgedMode.SetActive(!abridgedMode.activeSelf);
    }

    public bool IsAbridged()
    {
        return abridged;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void setVolume(float volume)
    {
        mixer.SetFloat("volume", volume);
    }

    public List<GameObject> FindSceneObjects(string sceneName)
    {
        List<GameObject> objs = new List<GameObject>();
        foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
        {
            if (obj.scene.name.CompareTo(sceneName) == 0)
            {
                objs.Add(obj);
            }
        }
        return objs;
    }

    public string levelName;
    AsyncOperation async;

    public void StartLoading()
    {
        StartCoroutine("load");
    }

    IEnumerator load()
    {
        Debug.LogWarning("ASYNC LOAD STARTED - " +
           "DO NOT EXIT PLAY MODE UNTIL SCENE LOADS... UNITY WILL CRASH");
        async = Application.LoadLevelAsync("MainGame");
        async.allowSceneActivation = false;
        yield return async;
    }

    public void ActivateScene()
    {
        async.allowSceneActivation = true;
    }

}
