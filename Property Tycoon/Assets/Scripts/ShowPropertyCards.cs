using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class: ShowPropertyCards
/// </summary>
public class ShowPropertyCards : MonoBehaviour
{
    public GameObject cardsUI;

    private bool showingCards = false;
    
    // Start is called before the first frame update
    void Start()
    {
      //  cardsUI.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //----------

    /// <summary>
    /// Method: clicked()
    /// ---------------------------------
    /// Hides and shows card depending state of card.
    /// </summary>
    public void clicked()
    {
        if (showingCards)
        {
            hidingCards();
        }
        else
        {
            showCards();
        }
    }

    /// <summary>
    /// Method: showCards()
    /// -------------------------------------------
    /// Shows the card
    /// </summary>
    void showCards()
    {
        showingCards = true;
        PropertyCardController.Instance.setUpCards(GameController.Instance.GetCurrentPlayer());
        cardsUI.active = true;
    }

    /// <summary>
    /// Method: hidingCards()
    /// --------------------------------------------
    /// Hides the card.
    /// </summary>
    void hidingCards()
    {
        showingCards = false;
        PropertyCardController.Instance.destoryCards();
        cardsUI.active = false;
    }
    
    
    //------
    
    
}
