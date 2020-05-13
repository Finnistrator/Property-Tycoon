using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

/// <summary>
/// Class: EditPlayerSettings
/// </summary>
public class EditPlayerSettings : MonoBehaviour
{
    private IconChoice iconChoice = IconChoice.Boot;
    private string playerName = "Player";
    private bool isAi = true;

 //   private PlayerSettings thePlayerSettings;

    [SerializeField] private Sprite[] icons; //enum values must be in the correct order!

    private int numOfIcons = Enum.GetValues(typeof(IconChoice)).Length;
    private UnityEngine.UI.Image[] images;

//    private void Start()
//    {
//        thePlayerSettings = new PlayerSettings(IconChoice.Boot, "Player", true);
//    }
    /// <summary>    
    /// Method: changeIcon()
    /// ----------------------------------
    /// Changes the icon of the player
    /// </summary>
    public void changeIcon()
    {
        int iconChoiceInt = (int) iconChoice;
        iconChoiceInt++;
        if (iconChoiceInt == numOfIcons) iconChoiceInt = 0;
        iconChoice = (IconChoice) iconChoiceInt;

    //    Debug.Log("change icon" + iconChoiceInt + " " + thePlayerSettings.iconChoice);
        images = gameObject.GetComponentsInChildren<Image>();
        images[1].sprite = icons[iconChoiceInt]; //gets the image component from playerIcon gameobject
    }

    /// <summary>
    /// Method: changeAI()
    /// -----------------------------------------------------
    /// Changes if the player is AI or not
    /// </summary>
    /// <param name="isAI"></param>
    public void changeAI(bool isAI)
    {
        this.isAi= isAI;
    }

    /// <summary>
    /// Method: changePlayerName
    /// ---------------------------------------------------
    /// Changes the player name
    /// </summary>
    /// <param name="name"></param>
    public void changePlayerName(string name)
    {
        playerName = name;
    }

    public PlayerSettings getPlayerSettings()
    {
        return new PlayerSettings(iconChoice,playerName,isAi);
    }
}