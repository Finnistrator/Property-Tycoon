using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract Class: Card
/// </summary>
public abstract class Card
{
    protected string description;

    /// <summary>
    /// Method: Interact()
    /// --------------------------------------------------
    /// Is an event for the card
    /// </summary>
    public abstract void Interact();

    public string GetDescription()
    {
        return description;
    }
}

/// <summary>
/// Class: MoneyCard
/// </summary>
public class MoneyCard : Card
{
    private int amount;
    private MoneyCardType type;
    private BankController bankController = BankController.Instance;
    private GameController gameController = GameController.Instance;

    /// <summary>
    /// Constructor for class
    /// ------------------------------------------
    /// Sets up the class
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="description"></param>
    /// <param name="type"></param>
    public MoneyCard(int amount, string description, MoneyCardType type = MoneyCardType.FromBank)
    {
        this.amount = amount;
        this.description = description;
        this.type = type;
    }

    /// <summary>
    /// Method: Interact()
    /// --------------------------------------------------
    /// Is an event for the card.
    /// </summary>
    public override void Interact()
    {
		int totalFromPlayers = 0;
        if (type != MoneyCardType.FromPlayers)
        {
            if(type != MoneyCardType.ToFreeParking)
            {
                bankController.AddBalance(gameController.GetCurrentPlayer(), amount);
            }
            else
            {
                bankController.AddFreeParking(gameController.GetCurrentPlayer(), -amount);
            }
        }
        else
        {
			foreach(Player player in gameController.GetPlayers())
			{
				if(player != gameController.GetCurrentPlayer())
				{
					totalFromPlayers += amount;
                    bankController.SubtractBalance(player, amount, true);
				}
				
			}
            bankController.AddBalance(gameController.GetCurrentPlayer(), totalFromPlayers);
        }
    }

    public void SetBankController(BankController bankController)
    {
        this.bankController = bankController;
    }

    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }
}

/// <summary>
/// Class: RepairAssessmentCard
/// </summary>
public class RepairAssessmentCard : Card
{
    private int houseAmount;
    private int hotelAmount;
	private BankController bankController = BankController.Instance;
	private GameController gameController = GameController.Instance;

    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="houseAmount"></param>
    /// <param name="hotelAmount"></param>
    /// <param name="description"></param>
    public RepairAssessmentCard(int houseAmount, int hotelAmount, string description)
    {
        this.houseAmount = houseAmount;
        this.hotelAmount = hotelAmount;
        this.description = description;
    }

    /// <summary>
    /// Method: Interact()
    /// --------------------------------------------------
    /// Is an event for the card
    /// </summary>
    public override void Interact()
    {
		int total = 0;
		foreach(PurchaseableProperty property in gameController.GetCurrentPlayer().GetOwnedProperties())
		{
			if(!property.HasHotel())
			{
				for(int i = 0; i < property.GetHouses(); i++)
				{
					total += houseAmount;
				}
			}
			else
			{
				total += hotelAmount;
			}
		}
		bankController.SubtractBalance(gameController.GetCurrentPlayer(), total, true);
    }
}

/// <summary>
/// Class: MoveStepsCard
/// </summary>
public class MoveStepsCard : Card
{
    private int steps;

    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="steps"></param>
    /// <param name="description"></param>
    public MoveStepsCard(int steps, string description)
    {
        this.steps = steps;
        this.description = description;
    }

    /// <summary>
    /// Method: Interact()
    /// --------------------------------------------------
    /// Is an event for the card
    /// </summary>
    public override void Interact()
    {
        GameController.Instance.GetCurrentPlayer().Move(steps);
    }

    public int GetSteps()
    {
        return steps;
    }
}

/// <summary>
/// Class: MovePropertyCard
/// </summary>
public class MovePropertyCard : Card
{
    private int propertyPos;
    private bool forward;
    private bool collectFromGo;
    private GameController gameController = GameController.Instance;

    /// <summary>
    /// Contructor for class
    /// </summary>
    /// <param name="propertyPos"></param>
    /// <param name="description"></param>
    /// <param name="forward"></param>
    /// <param name="collectFromGo"></param>
    public MovePropertyCard(int propertyPos, string description, bool forward = true, bool collectFromGo = true)
    {
        this.propertyPos = propertyPos;
        this.description = description;
        this.forward = forward;
        this.collectFromGo = collectFromGo;
        
    }

    /// <summary>
    /// Method: Interact()
    /// --------------------------------------------------
    /// Is an event for the card
    /// </summary>
    public override void Interact()
    {
        int totalPropertyNo = 40;
        int playerPos = gameController.GetCurrentPlayer().GetCurrentPos();

        if (forward)
        {
            if (propertyPos > playerPos)
            {
                MovePlayer(propertyPos - playerPos);
            }
            else
            {
                MovePlayer(totalPropertyNo - playerPos + propertyPos);
            }
        }
        else
        {
            if (propertyPos < playerPos)
            {
                MovePlayer(propertyPos - playerPos);
            }
            else
            {
                MovePlayer(-(playerPos + totalPropertyNo - propertyPos));
            }
        }
    }

    public void MovePlayer(int steps)
    {
        gameController.GetCurrentPlayer().Move(steps);
    }

    public int GetProperty()
    {
        return propertyPos;
    }

    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

}

/// <summary>
/// Class: PayFineOrOpportunityKnocksCard
/// </summary>
public class PayFineOrOpportunityKnocksCard : Card
{
    private float fine;
    /// <summary>
    /// Contructor for card
    /// </summary>
    /// <param name="description"></param>
    /// <param name="fine"></param>
    public PayFineOrOpportunityKnocksCard(string description, float fine)
    {
        this.description = description;
        this.fine = fine;
    }

    /// <summary>
    /// Method: Interact()
    /// --------------------------------------------------
    /// Is an event for the card
    /// </summary>
    public override void Interact()
    {
        GameController.Instance.HideFinishTurnButton();

        if(!GameController.Instance.GetCurrentPlayer().IsAI())
        {
            GameController.Instance.ShowOptionCardWindow();
        }
        else
        {
            if(Random.Range(0, 100) < 50)
            {
                GameController.Instance.PayChoiceFine(fine);
            }
            else
            {
                CardController.Instance.DrawOpportunityKnocks();
            }
        }
    }
}

/// <summary>
/// Class: GoToJailCard
/// </summary>
public class GoToJailCard : Card
{
    private GameController gameController = GameController.Instance;

    /// <summary>
    /// Contructor for class
    /// </summary>
    /// <param name="description"></param>
    public GoToJailCard(string description)
    {
        this.description = description;
    }

    /// <summary>
    /// Method: Interact()
    /// --------------------------------------------------
    /// Is an event for the card
    /// </summary>
    public override void Interact()
    {
        gameController.GoToJail();
    }

    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }
}

/// <summary>
/// Class GetOUtOfJailFreeCard
/// </summary>
public class GetOutOfJailFreeCard : Card
{
    private bool isFromPotLuck;
    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="description"></param>
    /// <param name="isFromPotLuck"></param>
    public GetOutOfJailFreeCard(string description, bool isFromPotLuck)
    {
        this.description = description;
        this.isFromPotLuck = isFromPotLuck;
    }

    /// <summary>
    /// Method: Interact()
    /// --------------------------------------------------
    /// Is an event for the card
    /// </summary>
    public override void Interact()
    {
        if (isFromPotLuck)
        {
            CardController.Instance.SetOutOfJailCardHolder(GameController.Instance.GetCurrentPlayer(), true);
        }
        else
        {
            CardController.Instance.SetOutOfJailCardHolder(GameController.Instance.GetCurrentPlayer(), false);
        }

        //TODO increment gooj card counter
    }

    public bool getIsFromPotLuck()
    {
        return isFromPotLuck;
    }
}

/// <summary>
/// Enum for MoneyCardType
/// </summary>
public enum MoneyCardType
{
    FromBank,
    ToFreeParking,
    FromPlayers,
}