using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainMenuAnimationController : MonoBehaviour
{
    [Header("Main Menu Elements")]
    //[SerializeField] private GameObject MainMenu;
    [SerializeField] private GameObject MainMenuTitle;
    [SerializeField] private Button PlayButton;
    [SerializeField] private Button OptionsButton;
    [SerializeField] private Button QuitButton;

    [Header("Options Menu Elements")]
    [SerializeField] private GameObject OptionsMenu;

    [SerializeField] private TextMeshProUGUI NumOfPlayersText;
    [SerializeField] private Button leftArrow;
    [SerializeField] private Button rightArrow;

    [Header("Player Settings Menu")] [SerializeField]
    private GameObject PlayerSettingsMenu;
    [FormerlySerializedAs("PlayerCardPrefab")] [SerializeField] private EditPlayerSettings editPlayerCardPrefab;
    

    private EditPlayerSettings[] playersPrefabs = new EditPlayerSettings[InitialGameSettings.NumOfPlayers];
    void Start()
    {
        OptionsMenu.SetActive(false);
        StartCoroutine(OpenPlayButtons());
    }
    
    public void OptionsPressed()
    {
        StartCoroutine(ClosePlayButtons());
        StartCoroutine(OpenOptionsMenu());
    }
    
    public void OptionsBackButtonPresssed()
    {
        StartCoroutine(CloseOptionsMenu());
        StartCoroutine(OpenPlayButtons());
    }
    
    public void PlayPressed()
    {
        StartCoroutine(ClosePlayButtons());
        StartCoroutine(OpenPlayerSettings());

    }

    public IEnumerator OpenPlayButtons()
    {
        yield return new WaitForSeconds(0.3f);
        MainMenuTitle.SetActive(true);
        MainMenuTitle.GetComponent<Animator>().Play("mainMenuTitleOpened");
        yield return new WaitForSeconds(0.21f);
        PlayButton.gameObject.SetActive(true);
        PlayButton.GetComponent<Animator>().Play("playButtonOpened",0,0f);
        yield return new WaitForSeconds(0.21f);
        OptionsButton.gameObject.SetActive(true);
        OptionsButton.GetComponent<Animator>().Play("playButtonOpened",0,0f);
        yield return new WaitForSeconds(0.21f);
        QuitButton.gameObject.SetActive(true);
        QuitButton.GetComponent<Animator>().Play("playButtonOpened",0,0f);
    }
    
    private IEnumerator ClosePlayButtons()
    {
        yield return new WaitForSeconds(0.24f);
        QuitButton.GetComponent<Animator>().Play("playButtonClosed",0,0f);
        yield return new WaitForSeconds(0.24f);
        QuitButton.gameObject.SetActive(false);
        OptionsButton.GetComponent<Animator>().Play("playButtonClosed",0,0f);
        yield return new WaitForSeconds(0.24f);
        OptionsButton.gameObject.SetActive(false);
        PlayButton.GetComponent<Animator>().Play("playButtonClosed",0,0f);
        yield return new WaitForSeconds(0.24f);
        PlayButton.gameObject.SetActive(false);
        MainMenuTitle.GetComponent<Animator>().Play("mainMenuTitleClosed",0,0f);
        yield return new WaitForSeconds(0.3f);
        MainMenuTitle.SetActive(false);
    }
    

    private IEnumerator OpenOptionsMenu()
    {
        yield return new WaitForSeconds(1.3f);
        OptionsMenu.SetActive(true);
        OptionsMenu.GetComponent<Animator>().Play("optionsMenuOpened",0,0f);
    }

    private IEnumerator CloseOptionsMenu()
    {
        yield return new WaitForSeconds(0.25f);
        OptionsMenu.GetComponent<Animator>().Play("optionsMenuClosed",0,0f);
        yield return new WaitForSeconds(0.2f);
        OptionsMenu.SetActive(false);
    }



    public void NumOfPlayersLeftArrow()
    {
        if (InitialGameSettings.NumOfPlayers == 1)
        {
            InitialGameSettings.NumOfPlayers = InitialGameSettings.TotalNumberOfPlayersDefault;
        }
        else
        {
            InitialGameSettings.NumOfPlayers -= 1;
        }
    
        NumOfPlayersText.text = (InitialGameSettings.NumOfPlayers).ToString();
    }
    
    public void NumOfPlayersRightArrow()
    {

        InitialGameSettings.NumOfPlayers = (InitialGameSettings.NumOfPlayers + 1) % (InitialGameSettings.TotalNumberOfPlayersDefault+1);
        if (InitialGameSettings.NumOfPlayers ==0)
        {
            InitialGameSettings.NumOfPlayers = 1;
        }
        NumOfPlayersText.text = (InitialGameSettings.NumOfPlayers).ToString();
    }

    public void QuickModeToggle(bool isQuickMode)
    {
        InitialGameSettings.quickMode = isQuickMode;
        Debug.Log("quick mode is" + InitialGameSettings.quickMode);
    }
    
    //-------------------
    
    
    private IEnumerator OpenPlayerSettings()
    {
        yield return new WaitForSeconds(0.9f);
        PlayerSettingsMenu.SetActive(true);
        yield return new WaitForSeconds(0.25f);
        StartCoroutine(PlayerSettings());
    }
    

    private IEnumerator PlayerSettings()
    {
        EditPlayerSettings editPlayerCardCreator;
        
      //  int xAxis = -(int)Math.Floor(PlayerSettingsMenu.GetComponent<RectTransform>().sizeDelta.x);
      int xAxis = 0;

        for (int i = 1; i <= InitialGameSettings.NumOfPlayers; i++)
        {
            yield return new WaitForSeconds(0.25f);
            editPlayerCardCreator = Instantiate(editPlayerCardPrefab, new Vector3(xAxis,0,0), Quaternion.identity);
            editPlayerCardCreator.transform.GetChild(0).GetComponent<InputField>().text = ("Player " + (i)); // Bug in TextMeshPro in current version of unity
            editPlayerCardCreator.transform.SetParent(PlayerSettingsMenu.transform.GetChild(0).GetChild(0).GetChild(0),false); //sets the parent to the Content in the Scrollview
            
            editPlayerCardCreator.changePlayerName("Player " + i);
            playersPrefabs[i-1] = editPlayerCardCreator;
            //  InitialGameSettings.players[i] = editPlayerCardCreator.getPlayerSettings();
            xAxis += 100;
        }
    }

    public EditPlayerSettings[] PlayersPrefabs => playersPrefabs;
    
}

