using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class: CardController
/// </summary>
public class CardController : Singleton<CardController>
{
    [SerializeField] private GameObject cardParent;
    [SerializeField] private Animator cardAnimator;
    [SerializeField] private TextMeshProUGUI cardTypeText;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;
    [SerializeField] private AudioClip cardDrawSound;

    private int potLuckPos = 0;
    private int opportunityKnocksPos = 0;
    private Player getOutOfJailCardHolderPL;
    private Player getOutOfJailCardHolderOK;

    private List<Card> potLuckCards = new List<Card>();
    private List<Card> opportunityKnocksCards = new List<Card>();
    private Card jailCardPL;
    private Card jailCardOK;

    private Card currentCard;

    private bool GOOJPL = false;
    private bool GOOJOK = false;

    private void Start()
    {
        HideCardParent();

        ImportController.Instance.ImportCards(out potLuckCards, out opportunityKnocksCards);

        if(potLuckCards.Count == 0)
        {
            potLuckCards.Add(new MoneyCard(100, "You inherit £100"));
            potLuckCards.Add(new MoneyCard(20, "You have won 2nd prize in a beauty contest, collect £20"));
            potLuckCards.Add(new MovePropertyCard(1, "Go back to Crapper Street", false));
            potLuckCards.Add(new MoneyCard(20, "Student loan refund. Collect £20"));
            potLuckCards.Add(new MoneyCard(200, "Bank error in your favour. Collect £200"));
            potLuckCards.Add(new MoneyCard(-100, "Pay bill for text books of £100"));
            potLuckCards.Add(new MoneyCard(-50, "Mega late night taxi bill pay £50"));
            potLuckCards.Add(new MovePropertyCard(0, "Advance to go"));
            potLuckCards.Add(new MoneyCard(50, "From sale of Bitcoin you get £50"));
            potLuckCards.Add(new PayFineOrOpportunityKnocksCard("Pay a £10 fine or take opportunity knocks", 10));
            potLuckCards.Add(new MoneyCard(-50,"Pay insurance fee of £50", MoneyCardType.ToFreeParking));
            potLuckCards.Add(new MoneyCard(100, "Savings bond matures, collect £100"));
            potLuckCards.Add(new GoToJailCard("Go to jail. Do not pass GO, do not collect £200"));
            potLuckCards.Add(new MoneyCard(25, "Received interest on shares of £25"));
            potLuckCards.Add(new MoneyCard(10, "It's your birthday. Collect £10 from each player", MoneyCardType.FromPlayers));
            jailCardPL = new GetOutOfJailFreeCard("Get out of jail free", true);
            potLuckCards.Add(jailCardPL);
        }

        if(opportunityKnocksCards.Count == 0)
        {
            opportunityKnocksCards.Add(new MoneyCard(50, "Bank pays you divided of £50"));
            opportunityKnocksCards.Add(new MoneyCard(100, "You have won a lip sync battle. Collect £100"));
            opportunityKnocksCards.Add(new MovePropertyCard(39, "Advance to Turing Heights"));
            opportunityKnocksCards.Add(new MovePropertyCard(24, "Advance to Han Xin Gardens. If you pass GO, collect £200"));
            opportunityKnocksCards.Add(new MoneyCard(-15, "Fined £15 for speeding"));
            opportunityKnocksCards.Add(new MoneyCard(-150, "Pay university fees of £150"));
            opportunityKnocksCards.Add(new MovePropertyCard(15, "Take a trip to Hove station. If you pass GO collect £200"));
            opportunityKnocksCards.Add(new MoneyCard(150, "Loan matures, collect £150"));
            opportunityKnocksCards.Add(new RepairAssessmentCard(40, 115, "You are assessed for repairs, £40/house, £115/hotel"));
            opportunityKnocksCards.Add(new MovePropertyCard(0, "Advance to GO"));
            opportunityKnocksCards.Add(new RepairAssessmentCard(25, 100, "You are assessed for repairs, £25/house, £100/hotel"));
            opportunityKnocksCards.Add(new MoveStepsCard(-3, "Go back 3 spaces"));
            opportunityKnocksCards.Add(new MovePropertyCard(11, "Advance to Skywalker Drive. If you pass GO collect £200"));
            opportunityKnocksCards.Add(new GoToJailCard("Go to jail. Do not pass GO, do not collect £200"));
            opportunityKnocksCards.Add(new MoneyCard(-20, "Drunk in charge of a skateboard. Fine £20", MoneyCardType.ToFreeParking));

            jailCardOK = new GetOutOfJailFreeCard("Get out of jail free", false);
            opportunityKnocksCards.Add(jailCardOK);
        }

        potLuckCards = ShuffleCards(potLuckCards);
        opportunityKnocksCards = ShuffleCards(opportunityKnocksCards);
    }

    /// <summary>
    /// Method: DrawPotLuck()
    /// ----------------------------------------
    /// Draws a card from the top of pot luck pile
    /// </summary>
    public void DrawPotLuck()
    {
        // Debug.LogError("Draw card");

        if (cardParent)
        {
            cardParent.SetActive(true);
        }

        if (cardDrawSound)
        {
            AudioController.Instance.PlaySound(cardDrawSound, 35);
        }
        currentCard = GetNextCard(potLuckCards);

        if (cardTypeText)
        {
            cardTypeText.text = "Pot Luck";
            cardDescriptionText.text = currentCard.GetDescription();
            cardAnimator.SetTrigger("Draw Pot Luck");
        }
    }

    /// <summary>
    /// Method: DrawOpportunityKnocks()
    /// -----------------------------------------------
    /// Draws a card from top of the opportunity knocks pile
    /// </summary>
    public void DrawOpportunityKnocks()
    {
        //Debug.LogError("Draw card");

        if (cardParent)
        {
            cardParent.SetActive(true);
        }

        if (cardDrawSound)
        {
            AudioController.Instance.PlaySound(cardDrawSound, 35);
        }
        currentCard = GetNextCard(opportunityKnocksCards);
        if (cardTypeText)
        {
            cardTypeText.text = "Opportunity Knocks";
            cardDescriptionText.text = currentCard.GetDescription();
            cardAnimator.SetTrigger("Draw Opportunity Knock");
        }
    }

    /// <summary>
    /// Method: HideCardParent()
    /// -----------------------------------------
    /// Hides the parent card.
    /// </summary>
    public void HideCardParent()
    {
        cardParent.SetActive(false);
    }

    /// <summary>
    /// Method: CardInteract()
    /// -----------------------------------------
    /// Activates the trigger of the current card.
    /// </summary>
    public void CardInteract()
    {
        cardAnimator.SetTrigger("Stop");
        currentCard.Interact();
    }

    private Card GetNextCard(List<Card> cards)
    {
        Card card = cards[0];
        cards.RemoveAt(0);
        cards.Add(card);

        return card;
    }

    private List<Card> ShuffleCards(List<Card> cards)
    {
        List<Card> shuffledCards = new List<Card>();

        while(cards.Count > 0)
        {
            int randomPos = Random.Range(0, cards.Count);
            shuffledCards.Add(cards[randomPos]);
            cards.RemoveAt(randomPos);
        }
        
        return shuffledCards;
    }

    /// <summary>
    /// Method: SetOutOfJailCardHolder()
    /// -----------------------------------------------
    /// Sets a player as holder to the get out of jail free
    /// card and checks its from pot luck or opportunity knocks.
    /// </summary>
    /// <param name="player"></param>
    /// <param name="isPotLuck"></param>
    public void SetOutOfJailCardHolder(Player player, bool isPotLuck)
    {
        if (isPotLuck)
        {
            getOutOfJailCardHolderPL = player;
        }
        else
        {
            getOutOfJailCardHolderOK = player;
        }
    }

    /// <summary>
    /// Method: RemoveGetOutOfJailCard()
    /// --------------------------------------------------
    /// Removes get out of jail free card from either pot luck
    /// or opportunity knocks.
    /// </summary>
    /// <param name="isPotLuck"></param>
    public void RemoveGetOutOfJailCard(bool isPotLuck)
    {
        if (isPotLuck)
        {
            potLuckCards.Remove(jailCardPL);
        }
        else
        {
            opportunityKnocksCards.Remove(jailCardOK);
        }
    }

    /// <summary>
    /// Method: AddGetOutOfJailCard()
    /// -----------------------------------------------
    /// Adds back get out of jail free card to pot luck or
    /// opportunity knocks.
    /// </summary>
    /// <param name="isPotLuck"></param>
    public void AddGetOutOfJailCard(bool isPotLuck)
    {
        if (isPotLuck && !(potLuckCards.Contains(jailCardPL)))
        {
            potLuckCards.Add(jailCardPL);
            getOutOfJailCardHolderPL = null;

        }
        else if (!isPotLuck && !(opportunityKnocksCards.Contains(jailCardOK)))
        {
            opportunityKnocksCards.Add(jailCardOK);
            getOutOfJailCardHolderOK = null;
        }
    }

    /// <summary>
    /// Method: GetJailCardDeck()
    /// ---------------------------------------------
    /// This method returns a string which is the string 
    /// of where the get out of jail card is from by the a 
    /// player.
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public string GetJailCardDeck(Player player)
    {
        if(getOutOfJailCardHolderPL != null && getOutOfJailCardHolderPL == player)
        {
            return "Pot Luck";
        }
        else if (getOutOfJailCardHolderOK != null && getOutOfJailCardHolderOK == player)
        {
            return "Opportunity Knocks";
        }
        return "None";
    }

    /// <summary>
    /// Method: RemovePlayersGetOutOfJailCard()
    /// --------------------------------------------------
    /// Removes get out of jail card from the player who had 
    /// the get out of jail card.
    /// </summary>
	public void RemovePlayersGetOutOfJailCard()
	{
		if(GetJailCardDeck(GameController.Instance.GetCurrentPlayer()) == "Pot Luck")
		{
			SetOutOfJailCardHolder(null, true);
		}
		else if(GetJailCardDeck(GameController.Instance.GetCurrentPlayer()) == "Opportunity Knocks")
		{
			SetOutOfJailCardHolder(null, false);
		}
	}

    public void SetPotLuckCards(List<Card> cards)
    {
        potLuckCards = cards;
    }

    public void SetOpportunityKnocksCards(List<Card> cards)
    {
        opportunityKnocksCards = cards;
    }

    public void SetCurrentCard(Card card)
    {
        currentCard = card;
    }

    public void SetjailCardPL(Card card)
    {
        jailCardPL = card;
        GOOJPL = true;
    }

    public void SetjailCardOK(Card card)
    {
        jailCardOK = card;
        GOOJOK = true;
    }

    public List<Card> GetPotLuckCards()
    {
        return potLuckCards;
    }

    public List<Card> GetOpportunityKnocksCards()
    {
        return opportunityKnocksCards;
    }

    public Card GetCurrentCard()
    {
        return currentCard;
    }

    public Card GetJailCardPL()
    {
        return jailCardPL;
    }

    public bool GetGOOJOK()
    {
        return GOOJOK;
    }

    public bool GetGOOJPL()
    {
        return GOOJPL;
    }
 }
