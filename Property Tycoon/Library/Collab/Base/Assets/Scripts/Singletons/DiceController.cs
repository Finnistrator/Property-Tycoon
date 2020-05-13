using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class: DiceController
/// </summary>
public class DiceController : Singleton<DiceController>
{
    [Header("DICE")]
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private Transform diceSpawnPoint;
    [Header("Sound Effects")]
    [SerializeField] private AudioClip rollDiceSound;
    [Header("DEBUG")]
    [SerializeField] private TextMeshProUGUI rollText;
    [SerializeField] private GameObject rollButton;

    private GameController gameController;
    private AudioController audioController;

    private int rolledNumber1 = 0;  
    private int rolledNumber2 = 0;

	private bool rolledDouble = false;
    private bool rolled = false;

    //Resets the stored rolled numbers upon start
    private void Start()
    {
        Reset();
        gameController = GameController.Instance;
        audioController = AudioController.Instance;
        
    }

    /// <summary>
    /// Method: RollDice()
    /// -------------------------------------------
    /// Rolls the dice.
    /// </summary>
    //Resets the stored rolled
    public void RollDice()
    {
        if(!gameController.GetTurnInProgress() && !rolled)
        {
            audioController.PlaySound(rollDiceSound, 35);
            Reset();
            CreateDice(3);
            CreateDice(-3);
            rolled = true;
            if (rollButton)
            {
                rollButton.SetActive(false);
            }
            
        }
    }

    //Instantiates a dice and calls the roll method attached to them
    private void CreateDice(float xOffset)
    {
        if (dicePrefab)
        {
            Instantiate(dicePrefab, diceSpawnPoint.transform.position + new Vector3(xOffset, 0, 0), Quaternion.identity).GetComponent<Dice>().RollDice(600, 600000);
        }
        
    }

    /// <summary>
    /// Method: SetRolledNumber()
    /// -------------------------------------------
    /// Saves the rolled dice numbers parsed into it
    /// </summary>
    /// <param name="number"></param>
    //Saves the rolled dice numbers parsed into it
    public void SetRolledNumber(int number)
    {
        if (number <= 6)
        {
            if (rolledNumber1 == 0)
            {
                rolledNumber1 = number;
            }
            else if (rolledNumber2 == 0)
            {
                rolledNumber2 = number;

				if(rolledNumber1 == rolledNumber2 && gameController.GetCurrentPlayer().GetAmountOfRolledDoubles() < 2)
				{
					gameController.GetCurrentPlayer().SetAmountOfRolledDoubles(gameController.GetCurrentPlayer().GetAmountOfRolledDoubles()+1);
					if(gameController.GetCurrentPlayer().GetAmountOfRolledDoubles() == 1)
					{
						//extra turn
						gameController.MovePlayer(GetRolledNumber());
						rollText.text = "Double " + (GetRolledNumber()/2) + " Extra Turn!";
						Invoke(nameof(Reset), 2);
						rolled = false;
					}
					else
					{
						gameController.GoToJail();
						rollText.text = "Go to Jail! Two consecutive doubles rolled!";
						Invoke(nameof(Reset), 2);
						rolled = false;
					}
				}
				else
				{
					gameController.GetCurrentPlayer().SetAmountOfRolledDoubles(0);
					rolledDouble = false;
					gameController.MovePlayer(GetRolledNumber());
					rollText.text = "Rolled " + (GetRolledNumber());
					Invoke(nameof(Reset), 2);
					rolled = false;
				}
            }
            else
            {
                Debug.LogError("Received 3 dice rolls without resetting!");
            }
        }
    }

    //Resets the saved dice numbers
    private void Reset()
    {
        rolledNumber1 = 0;
        rolledNumber2 = 0;
		rolledDouble = false;
        if (rollText)
        {
            rollText.text = "";
        }
    }
    
    public int GetRolledNumber()
    {
        //return rolledNumber1 + rolledNumber2;
		return 1;
    }

    public bool GetRolled()
    {
        return rolled;
    }

    public void SetGameController(GameController controller)
    {
        gameController = controller;
    }

    public void SetAudioController(AudioController audio)
    {
        audioController = audio;
    }

    public void SetAudioClip(AudioClip clip)
    {
        rollDiceSound = clip;
    }

    public void SetPreFabDice(Dice prefab)
    {
        dicePrefab = prefab;
    }
}
