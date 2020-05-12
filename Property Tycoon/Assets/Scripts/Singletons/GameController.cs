using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// This the class for GameController
/// </summary>
public class GameController : Singleton<GameController>
{
    [Header("References")]
    [SerializeField] private Transform propertyParent;
    [SerializeField] private Queue<Player> players = new Queue<Player>();
    [SerializeField] private TextMeshProUGUI playerTurnText;
    [SerializeField] private TextMeshProUGUI finishTurnText;
    [SerializeField] private Animator playerTurnAnimator;
    [SerializeField] private Transform jailPos;
    [SerializeField] private Transform ownedPropertiesList;
    [SerializeField] private PropertyDisplay propertyDisplayPrefab;
    [SerializeField] private GameObject rollButton;
    [SerializeField] private GameObject finishTurnButton;
    [SerializeField] private ScrollRect propertyListScrollRect;
    [SerializeField] private TextMeshProUGUI propertiesListHeader;
    [SerializeField] private TextMeshProUGUI raiseFundsText;
    [SerializeField] private GameObject getOutOfJailFee;
    [SerializeField] private GameObject optionCard;
    [SerializeField] private TextMeshProUGUI abridgedTimeText;
    [SerializeField] private GameObject winnerScreen;
    [SerializeField] private TextMeshProUGUI winnerText;

    [Header("Player Funds")]
    [SerializeField] private Transform playerFundsParent;
    [SerializeField] private PlayerFunds playerFundsPrefab;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip passSound;
    [SerializeField] private AudioClip nextTurnSound;
    [SerializeField] private AudioClip payRentSound;

    [Header("Player Icons")]
    [SerializeField] private Sprite[] icons;

    [Header("AI Chances")]
    [SerializeField] private int AIPayToLeaveJail = 50;
    [SerializeField] private int AIPurchaseProperty = 50;
    [SerializeField] private int AIBidChance = 50;
    [SerializeField] private int AIPurchaseHouseChance = 50;

    private PropertyController[] properties;
    
    private bool turnInProgress = false;
    private Property currentProperty;
    private bool raiseFunds = false;
    private BankController bankController = BankController.Instance;
    private bool abridged;
    private float abridgedTimeLeft;
    private bool gameOver = false;

    private void Start()
    {
        abridged = MainMenu.Instance.IsAbridged();
        abridgedTimeLeft = MainMenu.Instance.GetLimit();
        abridgedTimeText.gameObject.SetActive(abridged);

        finishTurnButton.SetActive(false);
        rollButton.SetActive(false);
        winnerScreen.SetActive(false);

        properties = propertyParent.GetComponentsInChildren<PropertyController>();
        List<Property> importedProperties = ImportController.Instance.ImportProperties();
        int importCounter = 0;
        for (int p = 0; p < properties.Length; p++)
        {
            if(!properties[p].IsPermanent())
            {
                properties[p].SetProperty(importedProperties[importCounter]);
                importCounter++;
            }
        }

        Player[] foundPlayers = FindObjectsOfType<Player>();
        int i = 0;
        
        for (; i < InitialGameSettings.NumOfPlayers; i++)
        {
            PlayerSettings player = InitialGameSettings.players[i] != null ? InitialGameSettings.players[i] : new PlayerSettings(IconChoice.Boot, "Player " + i, true);

            foundPlayers[i].SetAI(player.isAI);
            foundPlayers[i].SetIcon(icons[(int)player.iconChoice]);
            foundPlayers[i].SetPlayerName(player.playerName);
            foundPlayers[i].SetModel(player.iconChoice);
            players.Enqueue(foundPlayers[i]);
            Instantiate(playerFundsPrefab, playerFundsParent).SetPlayer(foundPlayers[i]);
        }

        for (; i < foundPlayers.Length; i++)
        {
            foundPlayers[i].gameObject.SetActive(false);
        }

        StartNextTurnAnimation();
    }

    private void FixedUpdate()
    {
        bankController = BankController.Instance;

        if (abridged)
        {
            abridgedTimeLeft -= Time.deltaTime;

            int minutes = (int)(abridgedTimeLeft / 60f);
            int seconds = (int)(abridgedTimeLeft % 60f);

            if(seconds == 60)
            {
                seconds = 0;
                minutes++;
            }


            abridgedTimeText.text = minutes + ":" + seconds;

            if(abridgedTimeLeft <= 0)
            {
                Player winner = null;
                float netWorth = 0;

                foreach (Player player in players)
                {
                    if(BankController.Instance.GetTotalNetWorth(player) > netWorth)
                    {
                        winner = player;
                        netWorth = BankController.Instance.GetTotalNetWorth(player);
                    }
                }
                
                Winner(winner, "\nwith a value of £" + netWorth);
            }
        }

        if (players.Count == 1)
        {
            Winner(players.Peek());
        }
    }

    /// <summary>
    /// Method: Winner
    /// -----------------------------------------------
    /// This method finishes the game and displays the winner
    /// </summary>
    /// <param name="player"></param>
    /// <param name="extra"></param>
    private void Winner(Player player, string extra = "")
    {
        gameOver = true;
        winnerScreen.SetActive(true);
        winnerText.text = player.GetPlayerName() + "\nHAS WON THE GAME" + extra + "!";
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Method: MovePlayer
    /// -----------------------------------------------
    /// This method moves a player any number of steps. The
    /// step paramter is an integer.
    /// </summary>
    /// <param name="steps"></param>
    public void MovePlayer(int steps)
    {
        GetCurrentPlayer().Move(steps);
        turnInProgress = true;
    }

    /// <summary>
    /// Method: StartTurn
    /// ----------------------------------------------
    /// This method starts the turn for the player. If 
    /// the player is an AI it will roll the dice. If the
    /// player is in jail it will reduce the counter for turn
    /// left in jail and end the turn
    /// </summary>
    public void StartTurn()
    {
        if(GetCurrentPlayer().IsInJail())
        {
			GetCurrentPlayer().ReduceTurnsInJail();

            FinishTurn();
        }
        else
        {
            finishTurnButton.SetActive(false);

            GetCurrentPlayer().StartTurn();

            if(!GetCurrentPlayer().IsAI())
            {
                rollButton.SetActive(true);
            }

            TogglePropertiesList(true);
        }
    }

    /// <summary>
    /// Method: TogglePropertiesList
    /// ----------------------------------------------
    /// This method activates the property list for a player.
    /// If the player requires to raise funds to pay another player
    /// or an active event then it will toggle the raise funds UI. This will 
    /// display if state is true
    /// </summary>
    /// <param name="state"></param>
    public void TogglePropertiesList(bool state)
    {
        if (propertiesListHeader)
        {
            raiseFunds = true;
            ToggleRaiseFunds();

            foreach (Transform trans in ownedPropertiesList)
            {
                if (trans != ownedPropertiesList)
                {
                    Destroy(trans.gameObject);
                }
            }

            if (state)
            {
                foreach (PurchaseableProperty property in GetCurrentPlayer().GetOwnedProperties())
                {
                    PropertyDisplay display = Instantiate(propertyDisplayPrefab, ownedPropertiesList);
                    display.SetProperty(property, GetCurrentPlayer());
                    display.transform.SetSiblingIndex((int)property.GetGroup());
                }
            }

            propertyListScrollRect.normalizedPosition = new Vector2(0, 1);

            if (state)
            {
                propertiesListHeader.text = GetCurrentPlayer().GetPlayerName() + "'s Turn";
            }
            else
            {
                propertiesListHeader.text = "";
            }
        }
    }

    /// <summary>
    /// Method: ToggleRaiseFunds
    /// ------------------------------------------------
    /// This method toggles a UI which activates if players 
    /// needs to raise money to pay for an event or player. If player
    /// doen't then it shows if player can purchaces houses.
    /// </summary>
    public void ToggleRaiseFunds()
    {
        if (raiseFundsText)
        {
            raiseFunds = !raiseFunds;

            foreach (PropertyDisplay display in ownedPropertiesList.GetComponentsInChildren<PropertyDisplay>())
            {
                display.ToggleRaiseFunds(raiseFunds);
            }

            if (raiseFunds)
            {
                raiseFundsText.text = "Add Houses";
            }
            else
            {
                raiseFundsText.text = "Raise Funds";
            }
        }

    }

    /// <summary>
    /// Method: HideFinishTurnButton
    /// -----------------------------------------------------
    /// Disables the finish turn button on the UI.
    /// </summary>
    public void HideFinishTurnButton()
    {
        finishTurnButton.SetActive(false);
    }
    /// <summary>
    /// Method: ShowFinishTurnButton
    /// ----------------------------------------------
    /// If the player is AI the button doesn't appear. If
    /// the player is a person, the button is enabled and it will check how many times it rolled
    /// double, if amoiunt of roll double is equal to 1 then finishTurnText 
    /// equals "Roll Again" else it equals "End Turn". 
    /// </summary>
    public void ShowFinishTurnButton()
    {
        if(!GetCurrentPlayer().IsAI())
        {
            finishTurnButton.SetActive(true);
            UIController.Instance.HidePurchasePropertyWindow();

            if(GetCurrentPlayer().GetAmountOfRolledDoubles() >= 1)
            {
                finishTurnText.text = "Roll Again";
            }
            else
            {
                finishTurnText.text = "End Turn";
            }
        }
    }

    /// <summary>
    /// Method: OnArriveAtProperty()
    /// --------------------------------------------------------------
    /// This method deals with the events that happens when a player lands on
    /// a property. If the property is own by the player or has no affect then 
    /// nothing happens. If the player lands on another player's if its not 
    /// Mortgaged then that player pays the rent otherwise nothing happens.
    /// If the property is part of groups income tax or super tax, the money goes to 
    /// freeparking. If its a card type like opportunity knocks, player will draw 
    /// a card corresponding to the correct pile. If the player is AI, and the property 
    /// has no interaction, it will just end its turn.
    /// </summary>
    /// <param name="property"></param>
    public void OnArriveAtProperty(Property property)
    {
        currentProperty = property;

        if (property is PurchaseableProperty && property.GetGroup() != Group.IncomeTax && property.GetGroup() != Group.SuperTax && GetCurrentPlayer().HasCompletedOneLap())
        {
            if (BankController.Instance.PropertyAvailable((PurchaseableProperty)property) && GetCurrentPlayer().HasCompletedOneLap())
            {
                if (GetCurrentPlayer().IsAI())
                {
                    if(Random.Range(0, 100) < AIPurchaseProperty)
                    {
                        BankController.Instance.CurrentPlayerPurchaseProperty();
                        FinishTurn();
                    }
                    else
                    {
                        UIController.Instance.ShowPurchasePropertyWindow((PurchaseableProperty)currentProperty);
                        UIController.Instance.ShowAuction(true);
                        BankController.Instance.StartAuction();
                    }
                }
                else
                {
                    UIController.Instance.ShowPurchasePropertyWindow((PurchaseableProperty)property);
                }
            }
            else if(GetCurrentPlayer().HasCompletedOneLap())
            {
                Player owner = BankController.Instance.GetOwner((PurchaseableProperty)property);

                if(!((PurchaseableProperty)property).IsMortgaged() && !owner.IsInJail() && owner != GetCurrentPlayer())
                {
                    AudioController.Instance.PlaySound(payRentSound, 35);
                    BankController.Instance.PayRent(GetCurrentPlayer(), (PurchaseableProperty)currentProperty);
                }
                

                if(GetCurrentPlayer().IsAI())
                {
                    FinishTurn();
                }
                else
                {
                    ShowFinishTurnButton();
                }
            }
        }
        else
        {
            switch (property.GetGroup())
            {
                case Group.Go:
                    if(GetCurrentPlayer().IsAI())
                    {
                        FinishTurn();
                    }
                    else
                    {
                        ShowFinishTurnButton();
                    }
                    break;
                case Group.JustVisiting:
                    if (GetCurrentPlayer().IsAI())
                    {
                        FinishTurn();
                    }
					else{
						ShowFinishTurnButton();
					}
                    break;
                case Group.FreeParking:
                    BankController.Instance.AddBalance(GetCurrentPlayer(), BankController.Instance.EmptyFreeParking());
                    if (GetCurrentPlayer().IsAI())
                    {
                        FinishTurn();
                    }
                    else
                    {
                        ShowFinishTurnButton();
                    }
                    break;
                case Group.GoToJail:
                    GoToJail();
                    FinishTurn();
                    break;
                case Group.OpportunityKnocks:
                    CardController.Instance.DrawOpportunityKnocks();
                    break;
                case Group.PotLuck:
                    CardController.Instance.DrawPotLuck();
                    break;
                case Group.IncomeTax:
                    BankController.Instance.AddFreeParking(GetCurrentPlayer(), ((PurchaseableProperty)property).GetCost());
                    AudioController.Instance.PlaySound(payRentSound, 35);
                    if (GetCurrentPlayer().IsAI())
                    {
                        FinishTurn();
                    }
                    else
                    {
                        ShowFinishTurnButton();
                    }
                    break;
                case Group.SuperTax:
                    BankController.Instance.AddFreeParking(GetCurrentPlayer(), ((PurchaseableProperty)property).GetCost());
                    AudioController.Instance.PlaySound(payRentSound, 35);
                    if (GetCurrentPlayer().IsAI())
                    {
                        FinishTurn();
                    }
                    else
                    {
                        ShowFinishTurnButton();
                    }
                    break;
                default:
                    if (GetCurrentPlayer().IsAI())
                    {
                        FinishTurn();
                    }
                    else
                    {
                        ShowFinishTurnButton();
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// Method: RemovePlayer()
    /// ---------------------------------------------------
    /// Removes player from the game.
    /// </summary>
    /// <param name="player"></param>
    public void RemovePlayer(Player player)
    {
        if(GetCurrentPlayer() == player)
        {
            FinishTurn();
        }

        Queue<Player> temp = new Queue<Player>();

        while(players.Count > 0)
        {
            Player curr = players.Dequeue();
            if (curr != player)
            {
                temp.Enqueue(curr);
            }
            else
            {
                player.gameObject.SetActive(false);
                if (playerFundsParent)
                {
                    foreach (PlayerFunds funds in playerFundsParent.GetComponentsInChildren<PlayerFunds>())
                    {
                        if (funds.GetPlayer() == player)
                        {
                            Destroy(funds.gameObject);
                        }
                    }
                }
            }
        }

        while(temp.Count > 0)
        {
            players.Enqueue(temp.Dequeue());
        }
    }

    /// <summary>
    /// Method: FinishTurn
    /// ------------------------------------------------------------------
    /// Finishes the turn of current player.
    /// </summary>
    public void FinishTurn()
    {
        if(gameOver)
        {
            return;
        }

        Debug.Log(GetCurrentPlayer().GetPlayerName() + " - Finish turn");
        //If player is AI, adds small pause before finishing turn
        if (UIController.Instance)
        {
            if (GetCurrentPlayer().IsAI())
            {
                TogglePropertiesList(true);

                Invoke(nameof(AIPurchaseHouse), 0.1f);
                Invoke(nameof(FinishTurnDelay), 1);
            }
            else
            {
                FinishTurnDelay();
            }
            getOutOfJailFee.SetActive(false);
        }
    }

    private void AIPurchaseHouse()
    {
        foreach (PropertyDisplay display in ownedPropertiesList.GetComponentsInChildren<PropertyDisplay>())
        {
            if (Random.Range(0, 100) < AIPurchaseHouseChance)
            {
                display.PurchaseHouse();
            }
        }
    }

    private void FinishTurnDelay()
    {
        TogglePropertiesList(false);
        UIController.Instance.HidePurchasePropertyWindow();
        turnInProgress = false;
        
		if(GetCurrentPlayer().GetAmountOfRolledDoubles() != 1 && GetCurrentPlayer().GetAmountOfRolledDoubles() != 2)
		{
			players.Enqueue(players.Dequeue());
            StartNextTurnAnimation();
        }
        else
        {
            DiceController.Instance.RollDice();
        }
        
        finishTurnButton.SetActive(false);
    }

    private void StartNextTurnAnimation()
    {
        AudioController.Instance.PlaySound(nextTurnSound, 35);
        CameraController.Instance.ChangeCameraRotation(GetCurrentPlayer().GetCurrentPos());
        playerTurnText.text = GetCurrentPlayer().GetPlayerName() + "'s Turn";
        playerTurnAnimator.Play("Turn Animation");
    }

    public PropertyController[] GetProperties()
    {
        return properties;
    }

    public bool GetTurnInProgress()
    {
        return turnInProgress;
    }

    public Queue<Player> GetPlayers()
    {
        return players;
    }

    public Player GetCurrentPlayer()
    {
        return players.Peek();
    }

    public void ShowOptionCardWindow()
    {
        optionCard.SetActive(true);
    }

    public void HideOptionCardWindow()
    {
        optionCard.SetActive(false);
    }

    public void PayChoiceFine(float amount)
    {
        BankController.Instance.AddFreeParking(GetCurrentPlayer(), amount);
    }

    /// <summary>
    /// Method: GoToJail()
    /// -------------------------------------------------------
    /// If player has get out of jail free card, it will say if the player
    /// wants to use it else it will send the player to jail.
    /// </summary>
    public void GoToJail()
    {
        if (CardController.Instance)
        {
            if (CardController.Instance.GetJailCardDeck(GetCurrentPlayer()) == "Pot Luck" || CardController.Instance.GetJailCardDeck(GetCurrentPlayer()) == "Opportunity Knocks")
            {
                if(!GetCurrentPlayer().IsAI())
                {
                    UIController.Instance.ToggleGetOutOfJailWindow(true);
                }
                else
                {
                    CardController.Instance.RemovePlayersGetOutOfJailCard();
                }
            }
            else
            {
                SendPlayerToJail();
            }
        }
        else
        {
            SendPlayerToJail();
        }
    }

    /// <summary>
    /// Method: SendPlayerToJail()
    /// ------------------------------------------------------------
    /// Sends player to jail and moves the token for that player in the 
    /// jail property area.
    /// </summary>
	public void SendPlayerToJail()
	{
        if(jailPos)
        {
            GetCurrentPlayer().transform.position = jailPos.position;
            GetCurrentPlayer().SetCurrentPos(10);
        }
        GetCurrentPlayer().GoToJail();

        if (GetCurrentPlayer().IsAI())
        {
            if (BankController.Instance.HasEnoughBalance(GetCurrentPlayer(), 50) && Random.Range(0, 100) < AIPayToLeaveJail)
            {
                PayToLeaveJail();
            }
            else
            {
                FinishTurn();
            }
        }
        else
        {
            ShowGetOutOfJailFee();
        }
	}

    /// <summary>
    /// Method: GetPropertyPosition()
    /// ------------------------------------------------------
    /// Get the position of property on the board. Returns 
    /// position of the property i in the array.
    /// </summary>
    /// <param name="property"></param>
    /// <returns index = i></returns>
    public int GetPropertyPosition(Property property)
    {
        for (int i = 0; i < properties.Length; i++)
        {
            if(properties[i].name == property.name)
            {
                return i;
            }
        }

        Debug.LogError("Property not found!");
        return -1;
    }
    
    // TESTING

    public void addPlayer(Player player)
    {
        players.Enqueue(player);
    }

    public void addMutiplePlayer(Player[] newPlayers)
    {
        foreach (Player player in newPlayers)
        {
            players.Enqueue(player);
        }
    }

    public Property GetCurrentProperty()
    {
        return currentProperty;
    }

    public void SetCurrentProperty(Property property)
    {
        currentProperty = property;
    }

    public bool GetRaiseFunds()
    {
        return raiseFunds;
    }

    public void SetBankController(BankController bankController)
    {
        this.bankController = bankController;
    }

    /// <summary>
    /// Method: ShowGetOutOfJailFee()
    /// -------------------------------------------------------------------
    /// This method displays the get out of jail fee UI to the player. If player
    /// has enough money (which is 50) it display it otherwise it just ends turn.
    /// </summary>
    public void ShowGetOutOfJailFee()
    {
        if (bankController)
        {
            if (GetCurrentPlayer().IsInJail() && bankController.HasEnoughBalance(GetCurrentPlayer(), 50))
            {
                getOutOfJailFee.SetActive(true);
            }
            else
            {
                getOutOfJailFee.SetActive(false);
                FinishTurn();
            }
        }
        else
        {
            FinishTurn();
        }
    }

    /// <summary>
    /// Method: PayToLeaveJail()
    /// --------------------------------------------------------
    /// This method makes the current player pay the fee to leave jail
    /// and no longer be in jail. It then ends the turn.
    /// </summary>
    public void PayToLeaveJail()
    {
        if(bankController.HasEnoughBalance(GetCurrentPlayer(), 50))
        {
            bankController.AddFreeParking(GetCurrentPlayer(), 50);
            if (getOutOfJailFee)
            {
                getOutOfJailFee.SetActive(false);
            }
            GetCurrentPlayer().GetOutOfJail();
            FinishTurn();
        }
    }

    public void SetProperties(PropertyController[] properties)
    {
        this.properties = properties;
    }

    public int GetAIBidChance()
    {
        return AIBidChance;
    }

    public PlayerFunds GetPlayerFunds(Player player)
    {
        if (playerFundsParent)
        {
            foreach (PlayerFunds funds in playerFundsParent.GetComponentsInChildren<PlayerFunds>())
            {
                if (funds.GetPlayer() == player)
                {
                    return funds;
                }
            }
        }
        return null;
    }
}
