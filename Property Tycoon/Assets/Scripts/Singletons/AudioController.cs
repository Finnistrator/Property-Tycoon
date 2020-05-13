using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class: AudioController
/// </summary>
public class AudioController : Singleton<AudioController>
{
    private List<AudioSource> audioSources = new List<AudioSource>();

    //This is for testing
    private bool playingTheSong = false;

    /// <summary>
    /// Method: PlayeSound
    /// ------------------------------------------------------
    /// Plays an audio clip at certain volume and have a vartiey 
    /// to change the pitch.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    /// <param name="variety"></param>
    public void PlaySound(AudioClip clip, int volume, float variety)
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                source.clip = clip;
                source.volume = (volume / 100f);
                source.pitch = Random.Range(1 - variety, 1 + variety);
                source.loop = false;
                source.playOnAwake = false;
                source.Play();
                return;
            }
        }

        AudioSource newSource = new GameObject("Audio Source").AddComponent<AudioSource>();
        newSource.transform.SetParent(transform);

        newSource.clip = clip;
        newSource.volume = (volume / 100f);
        newSource.pitch = Random.Range(1 - variety, 1 + variety);
        newSource.loop = false;
        newSource.playOnAwake = false;
        newSource.Play();

        audioSources.Add(newSource);
    }

    /// <summary>
    /// Method: PlaySound
    /// -------------------------------------------------
    /// Plays audio clip. Can adjust the volime of clip but 
    /// not pitch.
    /// </summary>
    /// <param name="clip"></param>
    /// <param name="volume"></param>
    public void PlaySound(AudioClip clip, int volume)
    {
        if (clip == null)
        {
            playingTheSong = true;
        }
        else
        {
            PlaySound(clip, volume, 0);
        }
    }

    /// <summary>
    /// Method: PlaySound
    /// ------------------------------------
    /// Plays an audio clip at volume 100.
    /// </summary>
    /// <param name="clip"></param>
    public void PlaySound(AudioClip clip)
    {
        if (clip == null)
        {
            playingTheSong = true;
        }
        else
        {
            PlaySound(clip, 100, 0);
        }
    }

    public bool GetPlayingTheSong()
    {
        return playingTheSong;
    }
}
