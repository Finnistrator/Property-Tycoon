using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class: Singleton
/// ------------------------
/// Used to ensure only one of 
/// the controllers can be created and
/// exist at one time.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Singleton<T> : MonoBehaviour where T : Component
{
    [SerializeField] private bool dontDestroy = true;

    public static T Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = this as T;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        if (dontDestroy)
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
