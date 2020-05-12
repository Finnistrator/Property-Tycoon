using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// Class: InitialGameSettings
/// </summary>
public class InitialGameSettings: MonoBehaviour
{
    public static int TotalNumberOfPlayersDefault = 6;
    public static int NumOfPlayers = 2;
    public static float volume;
    public static bool quickMode;
    
    public static PlayerSettings[] players = new PlayerSettings[TotalNumberOfPlayersDefault];

    private void Start()
    {
        for (int i = 0; i < TotalNumberOfPlayersDefault; i++)
        {
            players[i] = new PlayerSettings(IconChoice.HatStand, "Default", true);
        }
    }
}

/// <summary>
/// Enum: IconChoice
/// </summary>
public enum IconChoice {
    Boot = 0,
    Cat = 1,
    Goblet = 2,
    HatStand = 3,
    Smartphone = 4,
    Spoon = 5
}

/// <summary>
/// Class: PlayerSettings
/// </summary>
public class PlayerSettings
{
    public IconChoice iconChoice;
    public string playerName;
    public bool isAI;
    public PlayerSettings(IconChoice iconChoice, string playerName, bool isAi)
    {
        this.iconChoice = iconChoice;
        this.playerName = playerName;
        isAI = isAi;
    }

}
