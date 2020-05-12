using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class: CardHiderDebug
/// </summary>
public class CardHiderDebug : MonoBehaviour
{
    [SerializeField] private GameObject card;
    [SerializeField] private GameObject textHider;
    [SerializeField] private GameObject okayButton;

    private void Start()
    {
        HideCard();
    }

    /// <summary>
    /// Method: ShowTextHider()
    /// ----------------------------------------------
    /// Shows the text hider
    /// </summary>
    public void ShowTextHider()
    {
        textHider.SetActive(true);
    }

    /// <summary>
    /// Method: HideTextHider()
    /// ----------------------------------------------
    /// Hides the text hider
    /// </summary>
    public void HideTextHider()
    {
        textHider.SetActive(false);
    }

    /// <summary>
    /// Method: HideCard()
    /// --------------------------------------------------------
    /// Hides the card
    /// </summary>
    public void HideCard()
    {
        card.SetActive(false);
    }

    /// <summary>
    /// Method: ShowCard()
    /// ----------------------------------------------------------
    /// Shows the card 
    /// </summary>
    public void ShowCard()
    {
        okayButton.SetActive(!GameController.Instance.GetCurrentPlayer().IsAI());
        card.SetActive(true);
    }

    /// <summary>
    /// Method: AIInteract()
    /// --------------------------------------------------------------
    /// Checks the interaction of the AI for the card
    /// </summary>
    public void AIInteract()
    {
        if(GameController.Instance.GetCurrentPlayer().IsAI())
        {
            CardInteract();
            Card card = CardController.Instance.GetCurrentCard();

            if(!(card is MovePropertyCard || card is MoveStepsCard || card is GoToJailCard))
            {
                GameController.Instance.FinishTurn();
            }

            CardController.Instance.HideCardParent();
        }
    }

    /// <summary>
    /// Method: CardInteract()
    /// ----------------------------------------------------------------
    /// Checks the card interaction and hides card.
    /// </summary>
    public void CardInteract()
    {
        CardController.Instance.CardInteract();
        HideCard();
    }
}
