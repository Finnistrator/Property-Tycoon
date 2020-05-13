using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This the BankController class
/// </summary>
public class BankController : Singleton<BankController>
{
    [SerializeField] private List<PurchaseableProperty> unsoldProperties = new List<PurchaseableProperty>();
    [SerializeField] private AudioClip purchaseSound;
    [SerializeField] private TextMeshPro freeParkingText;
    [SerializeField] private TextMeshProUGUI currentBidText;
    [SerializeField] private TextMeshProUGUI auctionTimer;
    [SerializeField] private GameObject notEnoughFundsWindow;
    [SerializeField] private TextMeshProUGUI paymentDueText;
    private List<PropertyImport> properties;

    private GameController gameController = GameController.Instance;
    private DiceController diceController = DiceController.Instance;
    private ImportController importController = ImportController.Instance;

    private float freeParking = 0;
    private float balance = 50000;
    private bool isAuctioning = false;
    private float balanceDue;
    private Player balanceDuePlayer;

    private void Start()
    {
        notEnoughFundsWindow.SetActive(false);

        foreach (PropertyController property in GameController.Instance.GetProperties())
        {
            if(property.GetProperty() is PurchaseableProperty)
            {
                unsoldProperties.Add((PurchaseableProperty)property.GetProperty());
            }
        }

        freeParkingText.text = "£" + freeParking;

        foreach (Player player in GameController.Instance.GetPlayers())
        {
            player.SetBalance(1500);
        }

        foreach (PurchaseableProperty property in unsoldProperties)
        {
            property.ResetProperty();
        }
        
        gameController = GameController.Instance;
    }

    /// <summary>
    /// Method: AddBalance()
    /// ------------------------------------------------------
    /// Adds an amount to a specific player
    /// </summary>
    /// <param name="player"></param>
    /// <param name="amount"></param>
    public void AddBalance(Player player, float amount)
    {
        player.AddBalance(amount);
    }

    /// <summary>
    /// Method: GoBankrupt
    /// --------------------------------------------------------
    /// Specific player loses the game.
    /// </summary>
    /// <param name="player"></param>
    public void GoBankrupt(Player player)
    {
        Debug.Log(player + " has gone bankrupt!");
        if (GameController.Instance)
        {
            GameController.Instance.RemovePlayer(player);
            notEnoughFundsWindow.SetActive(false);
        }
        else
        {
            player.GoBankrupt();
            gameController.RemovePlayer(player);
        }
    }

    /// <summary>
    /// Method: CantPayMoney
    /// ################################
    /// Balance due player goes bankrupt
    /// </summary>
    public void CantPayMoney()
    {
        GoBankrupt(balanceDuePlayer);
    }

    /// <summary>
    /// Method: GetTotalNetWorth()
    /// --------------------------------------------------------
    /// This method gets the total worth of a player. So it returns
    /// the sum of all the costs of the person's properties and balance
    /// </summary>
    /// <param name="player"></param>
    /// <returns total></returns>
    public float GetTotalNetWorth(Player player)
    {
        float total = player.GetBalance();
        
        foreach (PurchaseableProperty property in player.GetOwnedProperties())
        {
            total += property.GetCost();

            if(property.GetHouses() <= 4)
            {
                total += ImportController.Instance.GetHouseCost(property.GetGroup()) * property.GetHouses();
            }
            else
            {
                total += 4 * ImportController.Instance.GetHouseCost(property.GetGroup());
                total += ImportController.Instance.GetHotelCost(property.GetGroup());
            }
        }

        return total;
    }
    /// <summary>
    /// Method: SubtractBalance()
    /// ----------------------------------------------------------------------
    /// This methods returns true if okayerr has enough money to lose for a current amount.
    /// If the player is an AI and doesnt have enough it will go bankrupt and return true. If
    /// its not compulsory then it will return false
    /// </summary>
    /// <param name="player"></param>
    /// <param name="amount"></param>
    /// <param name="compulsory"></param>
    /// <returns bool></returns>
    public bool SubtractBalance(Player player, float amount, bool compulsory)
    {
        if(HasEnoughBalance(player, amount))
        {
            player.AddBalance(-amount);
            return true;
        }
        else
        {
            if(player.IsAI() && GetTotalNetWorth(player) >= amount)
            {
                int infinite = 0;

                while(player.GetBalance() < amount)
                {
                    List<PurchaseableProperty> ownedProperties = player.GetOwnedProperties();

                    if(ownedProperties.Count == 0)
                    {
                        GoBankrupt(player);
                    }

                    SellProperty(ownedProperties[Random.Range(0, ownedProperties.Count)], player);

                    infinite++;

                    if(infinite > 1000)
                    {
                        Debug.LogError("Infinite loop selling properties!");
                        break;
                    }
                }

                return true;
            }
            else if(player.IsAI())
            {
                GoBankrupt(player);
                return true;
            }
            else if(!compulsory)
            {
                return false;
            }
            else
            {
                paymentDueText.text = "PAY £" + amount;
                notEnoughFundsWindow.SetActive(true);
                balanceDuePlayer = player;
                balanceDue = amount;
                return false;
            }
        }
    }

    /// <summary>
    /// Method: SellProperty()
    /// ---------------------------------------------------
    /// Sells houses, hotels or the property
    /// </summary>
    private void SellProperty(PurchaseableProperty property, Player owner)
    {
        if (property.GetHouses() > 0) //sell houses
        {
            SellHouse(property);
            GameController.Instance.TogglePropertiesList(true);
        }
        else //sell property
        {
            PayPlayerFromBank(owner, property.GetCost());
            AddProperty(property);
            owner.RemoveProperty(property);

            property.ResetProperty();
            gameController.TogglePropertiesList(true);
        }
    }

    private void AISellAsset(Player player)
    {
        foreach (PurchaseableProperty property in player.GetOwnedProperties())
        {
            if(property.GetHouses() > 0)
            {
                property.RemoveHouse();
                return;
            }
        }
    }

    /// <summary>
    /// Method: PayBalanceDue()
    /// ----------------------------------------------------------------
    /// This method pays the balance due for current player
    /// </summary>
    public void PayBalanceDue()
    {
        if(SubtractBalance(balanceDuePlayer, balanceDue, true))
        {
            notEnoughFundsWindow.SetActive(false);
        }
    }

    /// <summary>
    /// Method: HasEnoughBalance()
    /// ----------------------------------------------------------------
    /// This methods returns boolean if a player has enough money to pay. It
    /// returns true if player has enough false otherwise.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool HasEnoughBalance(Player player, float amount)
    {
        if (player.GetBalance() - amount >= 0)
        {
            return true;
        }

        return false;
    }

    public void AddProperty(Property property)
    {
        unsoldProperties.Add((PurchaseableProperty)property);
    }

    public void RemoveProperty(Property property)
    {
        unsoldProperties.Remove((PurchaseableProperty)property);
    }

    public bool PropertyAvailable(PurchaseableProperty property)
    {
        return unsoldProperties.Contains(property);
    }

    /// <summary>
    /// Method: PayRent()
    /// ----------------------------------------------------------------
    /// This method makes the payingPlayer pay the rent to the player who
    /// owns the property
    /// </summary>
    /// <param name="payingPlayer"></param>
    /// <param name="property"></param>
    public void PayRent(Player payingPlayer, PurchaseableProperty property)
    {
        foreach (Player player in gameController.GetPlayers())
        {
            foreach (PurchaseableProperty ownedProperty in player.GetOwnedProperties())
            {
                if(ownedProperty == property)
                {
                    SubtractBalance(payingPlayer, GetRent(ownedProperty, player), true);
                    AddBalance(player, GetRent(ownedProperty, player));
                    if (GameController.Instance)
                    {
                        GameController.Instance.ShowFinishTurnButton();
                    }
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Method: GetRent()
    /// -------------------------------------------------
    /// Returns the rent of the property that the currently player owns
    /// </summary>
    /// <param name="property"></param>
    /// <param name="owner"></param>
    /// <returns></returns>
    public float GetRent(PurchaseableProperty property, Player owner)
    {
        if(property.GetGroup() == Group.Station)
        {
            switch (GetPlayerPropertyTypeCount(Group.Station, owner))
            {
                case 0: return 0;
                case 1: return 25;
                case 2: return 50;
                case 3: return 100;
                case 4: return 200;
            }
        }
        else if(property.GetGroup() == Group.Utilities)
        {
            switch (GetPlayerPropertyTypeCount(Group.Utilities, owner))
            {
                case 0: return 0;
                case 1: return diceController.GetRolledNumber() * 4;
                case 2: return diceController.GetRolledNumber() * 10;
            }
        }
        
        if(DoesPlayerOwnAllSameColour(property, owner) && !DoesGroupContainHouses(property.GetGroup(), owner))
        {
            return property.GetRent0Houses() * 2;
        }
        else
        {
            switch (property.GetHouses())
            {
                case 1: return property.GetRent1House();
                case 2: return property.GetRent2House();
                case 3: return property.GetRent3House();
                case 4: return property.GetRent4House();
                case 5: return property.GetRentHotel();
            }
        }

        return property.GetRent0Houses();
    }

    private bool DoesGroupContainHouses(Group group, Player player)
    {
        foreach (PurchaseableProperty property in player.GetOwnedProperties())
        {
            if(property.GetGroup() == group && (property.GetHouses() > 0 || property.HasHotel()))
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Method: DoesPlayerOwnAllSameColour()
    /// ---------------------------------------------------------------------------------
    /// This method returns true if player owns all the property groups, false otherwise
    /// </summary>
    /// <param name="property"></param>
    /// <param name="owner"></param>
    /// <returns boolean></returns>
    public bool DoesPlayerOwnAllSameColour(PurchaseableProperty property, Player owner)
    {
        int sameColourCount = owner.GetNumberOfSameColourProperties(property);

        switch (property.GetGroup())
        {
            case Group.Brown: if (sameColourCount == 2) { return true; } break;
            case Group.Blue: if (sameColourCount == 3) { return true; } break;
            case Group.Purple: if (sameColourCount == 3) { return true; } break;
            case Group.Orange: if (sameColourCount == 3) { return true; } break;
            case Group.Red: if (sameColourCount == 3) { return true; } break;
            case Group.Yellow: if (sameColourCount == 3) { return true; } break;
            case Group.Green: if (sameColourCount == 3) { return true; } break;
            case Group.DeepBlue: if (sameColourCount == 2) { return true; } break;
        }

        return false;
    }

    /// <summary>
    /// Method: Within1House()
    /// -----------------------------------------------------------------
    /// Returns true if property has house difference of no more than one compared
    /// to the others houses in the same group
    /// </summary>
    /// <param name="property"></param>
    /// <returns boolean ></returns>
    public bool Within1House(PurchaseableProperty property)
    {
        foreach (PropertyController prop in gameController.GetProperties())
        {
            if(prop.GetProperty() is PurchaseableProperty && ((PurchaseableProperty)(prop.GetProperty())).GetGroup() == property.GetGroup() && Mathf.Abs(property.GetHouses() - ((PurchaseableProperty)(prop.GetProperty())).GetHouses()) > 1)
            {
                return false;
            }
        }

        return true;
    }

    private int GetPlayerPropertyTypeCount(Group group, Player player)
    {
        int count = 0;

        foreach (Property ownedProperty in player.GetOwnedProperties())
        {
            if (ownedProperty.GetGroup() == group)
            {
                count++;
            }
        }

        return count;
    }

    /// <summary>
    /// Method: CurrentPlayerPurchaseProperty()
    /// ---------------------------------------------------------------------------------
    /// This method checks if the current player wants to but the property. If he doesn't 
    /// it goes to auction.
    /// </summary>
    public void CurrentPlayerPurchaseProperty()
    {
        if (SubtractBalance(gameController.GetCurrentPlayer(), ((PurchaseableProperty)gameController.GetCurrentProperty()).GetCost(), false))
        {
            if (AudioController.Instance)
            {
                AudioController.Instance.PlaySound(purchaseSound, 35, 0.05f);
            }
            gameController.GetCurrentPlayer().AddProperty((PurchaseableProperty)gameController.GetCurrentProperty());
            RemoveProperty(gameController.GetCurrentProperty());
            gameController.SetCurrentProperty(null);
            if (UIController.Instance)
            {
                UIController.Instance.HidePurchasePropertyWindow();
                gameController.TogglePropertiesList(true);
            }

            gameController.ShowFinishTurnButton();
        }
        else
        {
            StartAuction();
        }
    }

    public void SetGameController(GameController gc)
    {
        gameController = gc;
    }

    /// <summary>
    /// Method: AddFreeParking()
    /// ---------------------------------------------------------------
    /// Adds an amount of money from player to the free parking.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="amount"></param>
    public void AddFreeParking(Player player, float amount)
    {
        SubtractBalance(player, amount, true);
        StartCoroutine(AddFreeParking(amount));
    }

    private IEnumerator AddFreeParking(float amount)
    {
        float current = freeParking;
        freeParking += amount;
        if (freeParkingText)
        {
            while (Mathf.Abs(current - freeParking) > 0.1f)
            {
                if (current > freeParking)
                {
                    current -= Time.deltaTime * 200;

                    if(current <= freeParking)
                    {
                        break;
                    }
                }
                else
                {
                    current += Time.deltaTime * 200;

                    if (current >= freeParking)
                    {
                        break;
                    }
                }
                freeParkingText.text = "£" + current.ToString("F2");
                yield return new WaitForEndOfFrame();
            }

            freeParkingText.text = "£" + freeParking.ToString("F2");
        }
    }

    /// <summary>
    /// Method: EmptyFreeParking()
    /// ---------------------------------------------------------------------
    /// Empties the free parking and returns the amount that was in it.
    /// </summary>
    /// <returns></returns>
    public float EmptyFreeParking()
    {
        float currentFreeParking = freeParking;
        StartCoroutine(AddFreeParking(-freeParking));
        freeParking = 0;

        return currentFreeParking;
    }

    /// <summary>
    /// Method: PayPlayerFromBank()
    /// ----------------------------------------------
    /// Bank pays player an amount
    /// </summary>
    /// <param name="player"></param>
    /// <param name="amount"></param>
    public void PayPlayerFromBank(Player player, float amount)
    {
        if(balance - amount >= 0)
        {
            AddBalance(player, amount);
            balance -= amount;
        }
        else
        {
            Debug.LogError("Bank ran out of money!");
        }
    }

    /// <summary>
    /// Method: PayBankFromPlayer()
    /// -------------------------------------------------------------------
    /// Players pays bank an amount
    /// </summary>
    /// <param name="player"></param>
    /// <param name="amount"></param>
    public void PayBankFromPlayer(Player player, float amount)
    {
        SubtractBalance(player, amount, true);
        balance += amount;
    }

    /// <summary>
    /// Method: PurchaseHouse()
    /// --------------------------------------------------------------------
    /// If player has enough money, current player adds house to property or hotel if has
    /// 4 houses prior and returns true. If player doesnt have enough money then returns false.
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public bool PurchaseHouse(PurchaseableProperty property)
    {
        if (ImportController.Instance)
        {
            if (property.GetHouses() < 5 && SubtractBalance(gameController.GetCurrentPlayer(), ImportController.Instance.GetHouseCost(property.GetGroup()), false))
            {
                property.AddHouse();

                foreach (PropertyController controller in gameController.GetProperties())
                {
                    controller.UpdateProperty();
                }

                return true;
            }
            return false;
        }
        else{
            if (property.GetHouses() < 5 && property.GetGroup() != Group.Station)
            {
                property.AddHouse();

                foreach (PropertyController controller in gameController.GetProperties())
                {
                    controller.UpdateProperty();
                }
                return true;
            }
            return false;
        }

    }

    /// <summary>
    /// Method: SellHouse()
    /// --------------------------------------------------------------------
    /// Sells a house on a property
    /// </summary>
    /// <param name="property"></param>
    public void SellHouse(PurchaseableProperty property)
    {
        if (ImportController.Instance)
        {
            if (property.GetHouses() < 5)
            {
                AddBalance(gameController.GetCurrentPlayer(), ImportController.Instance.GetHouseCost(property.GetGroup()));
            }
            else
            {
                AddBalance(gameController.GetCurrentPlayer(), ImportController.Instance.GetHotelCost(property.GetGroup()));
            }
        }
        else
        {
            if (property.GetHouses() < 5)
            {
                AddBalance(gameController.GetCurrentPlayer(), 110);
            }
            else
            {
                AddBalance(gameController.GetCurrentPlayer(), 110); 
            }
        }

        property.RemoveHouse();


        foreach (PropertyController controller in gameController.GetProperties())
        {
            controller.UpdateProperty();
        }
    }

    private Queue<Player> auctioningPlayers;
    private float currentBid = 0;
    private Player currentHighestBid;
    private float currentTimer = 0;
    private float maxTimer = 5;

    /// <summary>
    /// Method: StartAuction()
    /// ---------------------------------------------------
    /// Starts auction for current property.
    /// </summary>
    public void StartAuction()
    {
        Debug.Log("START AUCTION");

        auctioningPlayers = new Queue<Player>();

        currentHighestBid = null;

        //currentBid = ((PurchaseableProperty)gameController.GetCurrentProperty()).GetCost();
        currentBid = 0;

        foreach (Player player in gameController.GetPlayers())
        {
            if(player.HasCompletedOneLap() && !player.IsInJail())
            {
                auctioningPlayers.Enqueue(player);
            }
        }


        if(auctioningPlayers.Count > 0)
        {
            Debug.Log("ENOUGH PLAYERS");

            if (currentBidText)
            {
                currentBidText.text = "Current Bid: £" + currentBid;
                currentTimer = maxTimer;

                UIController.Instance.ShowAuction(true);
            }
            isAuctioning = true;
        }
        else if(gameController.GetCurrentPlayer().IsAI())
        {
            Debug.Log("NOT ENOUGH PLAYERS, ENDING AI TURN");
            gameController.FinishTurn();
        }
        else
        {
            Debug.Log("NOT ENOUGH PLAYERS, SHOWING FINISH TURN BUTTON");
            gameController.ShowFinishTurnButton();
        }
    }

    private void FixedUpdate()
    {
        gameController = GameController.Instance;
        diceController = DiceController.Instance;
        importController = ImportController.Instance;

        if (isAuctioning)
        {
            if(currentTimer < 3 && GetCurrentAuctioningPlayer().IsAI() && GetCurrentAuctioningPlayer().GetBalance() > GetNextBidAmount(GetCurrentAuctioningPlayer().GetBalance()) && Random.Range(0, 100) < GameController.Instance.GetAIBidChance())
            {
                BidInAuction();
            }
            else if(currentTimer < 3 && GetCurrentAuctioningPlayer().IsAI())
            {
                RemovePlayerFromAuction();
                return;
            }

            currentTimer -= Time.deltaTime;
            auctionTimer.text = Mathf.Ceil(currentTimer).ToString("F0");

            if(currentTimer <= 0)
            {
                RemovePlayerFromAuction();
                currentTimer = maxTimer;
            }
        }
    }

    /// <summary>
    /// Method: StopAuction()
    /// ------------------------------------------------------------
    /// Stops the auction, property goes to highest bidder. If there
    /// is no highest bidder, then property is remained unsold.
    /// </summary>
    public void StopAuction()
    {
        if(currentHighestBid)
        {
            currentHighestBid.AddProperty((PurchaseableProperty)GameController.Instance.GetCurrentProperty());
            SubtractBalance(currentHighestBid, currentBid, false);
            RemoveProperty(gameController.GetCurrentProperty());

            AudioController.Instance.PlaySound(purchaseSound, 35, 0.05f);
            gameController.SetCurrentProperty(null);
        }

        isAuctioning = false;

        currentHighestBid = null;
        auctioningPlayers = new Queue<Player>();
        UIController.Instance.ShowAuction(false);

        GameController.Instance.TogglePropertiesList(true);

        if(GameController.Instance.GetCurrentPlayer().IsAI())
        {
            GameController.Instance.FinishTurn();
        }
        else
        {
            GameController.Instance.ShowFinishTurnButton();
        }
    }

    /// <summary>
    /// Method: BidInAuction()
    /// -----------------------------------------------------
    /// Stops auction if one player in auction left and is that current player with highest bid.
    /// Otherwise current places bid and it goes to the next player.
    /// </summary>
    public void BidInAuction()
    {
        if(currentHighestBid == GetCurrentAuctioningPlayer() && auctioningPlayers.Count == 1)
        {
            StopAuction();
            return;
        }

        currentHighestBid = GetCurrentAuctioningPlayer();

        currentBid = GetNextBidAmount(GetCurrentAuctioningPlayer().GetBalance());

        if (currentBidText)
        {
            currentBidText.text = "Current Bid: £" + currentBid;
        }
        NextPlayerInAuction();
    }

    private void NextPlayerInAuction()
    {
        Player currentPlayer = auctioningPlayers.Dequeue();
        auctioningPlayers.Enqueue(currentPlayer);
        currentTimer = maxTimer;
    }

    /// <summary>
    /// Method: RemovePlayerFromAuction()
    /// -----------------------------------------------------------
    /// Removes current player from auction. If currnt player is highest
    /// bid and last person then it stop the auction, if there is no one
    /// in auction then stops auction.
    /// </summary>
    public void RemovePlayerFromAuction()
    {
        auctioningPlayers.Dequeue();

        if(auctioningPlayers.Count == 1 && auctioningPlayers.Peek() == currentHighestBid)
        {
            Debug.Log(auctioningPlayers.Peek() + " has won the bid!");
            StopAuction();
        }
        else if(auctioningPlayers.Count == 0)
        {
            StopAuction();
        }

        currentTimer = maxTimer;
    }

    public bool IsAuctioning()
    {
        return isAuctioning;
    }

    /// <summary>
    /// Method: GetCurrentAuctioningPlayer()
    /// ----------------------------------------------------
    /// Gets current player auctioning
    /// </summary>
    /// <returns></returns>
    public Player GetCurrentAuctioningPlayer()
    {
        if(auctioningPlayers.Count > 0)
        {
            return auctioningPlayers.Peek();
        }
        else
        {
            return null;
        }
    }

    public float GetCurrentBid()
    {
        return currentBid;
    }

    /// <summary>
    /// Method: GetNextBidAmount()
    /// ----------------------------------------------------------------------
    /// Get's the next value for the bid
    /// </summary>
    /// <param name="currentAmount"></param>
    /// <returns></returns>
    public float GetNextBidAmount(float currentAmount)
    {
        if(currentBid < 100 && currentAmount - (currentBid + 10) >= 0)
        {
            return currentBid + 10;
        }
        else if(currentBid < 1000 && currentAmount - (currentBid + 100) >= 0)
        {
            return currentBid + 100;
        }
        else if(currentBid < 10000 && currentAmount - (currentBid + 1000) >= 0)
        {
            return currentBid + 1000;
        }
        else
        {
            return currentAmount;
        }
    }

    public Player GetHighestBid()
    {
        return currentHighestBid;
    }

    /// <summary>
    /// Method: IsPlayerStillInAuction
    /// ----------------------------------------------------
    /// Checks if player is still in the auction. Return true
    /// if player is, false otherwise.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public bool IsPlayerStillInAuction(Player player)
    {
        if(auctioningPlayers != null)
        {
            return auctioningPlayers.Contains(player);
        }
        return false;
    }

    public List<PurchaseableProperty> GetUnsoldProperties()
    {
        return unsoldProperties;
    }

    public void SetUnsoldProperties(List<PurchaseableProperty> listOfProperties)
    {
        unsoldProperties = listOfProperties;
    }

    public void SetDiceController(DiceController diceController)
    {
        this.diceController = diceController;
    }

    public bool GetIsAuctioning()
    {
        return isAuctioning;
    }

    public void SetFreeParking(float freeparking)
    {
        this.freeParking = freeparking;
    }

    /// <summary>
    /// Method: GetOwner
    /// -------------------------------------------
    /// Returns the owner of the property
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    public Player GetOwner(PurchaseableProperty property)
    {
        foreach (Player player in GameController.Instance.GetPlayers())
        {
            foreach (PurchaseableProperty prop in player.GetOwnedProperties())
            {
                if(prop == property)
                {
                    return player;
                }
            }
        }

        return null;
    }

    public void LoadProperties(string path)
    {
        string myloadMyProperty = JsonFileReader.LoadJsonAsResource(path);
        PropertyImport property = JsonUtility.FromJson<PropertyImport>(myloadMyProperty);
        properties.Add(property);
    }

    public void SetPlayerBalanceDue(Player player)
    {
        balanceDuePlayer = player;
    }
}
