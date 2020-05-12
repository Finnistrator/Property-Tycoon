using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class: PropertyDisplay
/// </summary>
public class PropertyDisplay : MonoBehaviour
{
    [Header("Property Display")]
    [SerializeField] private TextMeshProUGUI propertyNameText;
    [SerializeField] private Image propertyGroupColour;

    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI mortgageText;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private TextMeshProUGUI buyHouseText;
    [SerializeField] private TextMeshProUGUI housesText;
    [SerializeField] private GameObject buttonsParent;

    [Header("Buttons")]
    [SerializeField] private GameObject mortgageButton;
    [SerializeField] private GameObject buyHouseButton;

    private PurchaseableProperty property;
    private BankController bankController = BankController.Instance;
    private GameController gameController = GameController.Instance;
    private Player owner;

    /// <summary>
    /// Method: SetProperty()
    /// ---------------------------------------------------------
    /// Set the property
    /// </summary>
    /// <param name="property"></param>
    /// <param name="owner"></param>
    public void SetProperty(PurchaseableProperty property, Player owner)
    {
        this.property = property;
        this.owner = owner;
        propertyNameText.text = property.name;
        propertyGroupColour.color = ColourController.Instance.GetGroupColour(property.GetGroup());
        UpdateButtons();
    }

    /// <summary>
    /// Method: PurchaseHouse()
    /// ---------------------------------------------
    /// Buys a house for property.
    /// </summary>
    public void PurchaseHouse()
    {
        if(!bankController.DoesPlayerOwnAllSameColour(property, owner) || !bankController.Within1House(property))
        {
            return;
        }

        if(property.GetHouses() < 5)
        {
            if(bankController.PurchaseHouse(property))
            {
                UpdateButtons();
            }
        }
    }

    private void UpdateButtons()
    {
        if (housesText)
        {
            housesText.text = property.GetHouses().ToString();

            buttonsParent.SetActive(!owner.IsAI());

            mortgageButton.SetActive(property.GetHouses() == 0);
            buyHouseButton.SetActive(bankController.Within1House(property));

            if (property.GetHouses() == 0)
            {
                if (property.IsMortgaged())
                {
                    mortgageText.text = "Unmortgage (<color=red>-£" + property.GetCost().ToString("F2") + "</color>)";
                }
                else
                {
                    mortgageText.text = "Mortgage (<color=green>+£" + (property.GetCost() / 2f).ToString("F2") + "</color>)";
                }
            }

            if (property.GetHouses() > 0)
            {
                if (property.GetHouses() >= 5)
                {
                    sellText.text = "Sell Hotel (<color=green>+£" + ImportController.Instance.GetHotelCost(property.GetGroup()) + "</color>)";
                    buyHouseButton.SetActive(false);
                }
                else
                {
                    if (property.GetHouses() == 4)
                    {
                        buyHouseText.text = "Buy Hotel (<color=red>-£" + ImportController.Instance.GetHotelCost(property.GetGroup()) + "</color>)";
                    }
                    else
                    {
                        buyHouseText.text = "Buy House (<color=red>-£" + ImportController.Instance.GetHouseCost(property.GetGroup()) + "</color>)";
                    }
                    sellText.text = "Sell House (<color=green>+£" + ImportController.Instance.GetHouseCost(property.GetGroup()) + "</color>)";
                }
            }
            else
            {
                buyHouseText.text = "Buy House (<color=red>-£" + ImportController.Instance.GetHouseCost(property.GetGroup()) + "</color>)";
                sellText.text = "Sell Property (<color=green>+£" + property.GetCost().ToString("F2") + "</color>)";
            }

            if (!bankController.DoesPlayerOwnAllSameColour(property, owner))
            {
                buyHouseButton.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Method: MortgageProperty()
    /// ----------------------------------------------------
    /// Mortgage the property
    /// </summary>
    public void MortgageProperty()
    {
        if(property.GetHouses() == 0)
        {
            if (property.IsMortgaged()) //unmortgage
            {
                if (bankController.HasEnoughBalance(owner, property.GetCost() / 2f))
                {
                    property.SetMortgaged(false);
                    bankController.PayBankFromPlayer(owner, property.GetCost() / 2f);
                    UpdateButtons();
                }
            }
            else //mortgage
            {
                property.SetMortgaged(true);
                bankController.PayPlayerFromBank(owner, property.GetCost());
                UpdateButtons();
            }
        }
    }

    /// <summary>
    /// Method: SellProperty()
    /// ---------------------------------------------------
    /// Sells houses, hotels or the property
    /// </summary>
    public void SellProperty()
    {
        if (property.GetHouses() > 0) //sell houses
        {
            bankController.SellHouse(property);
            UpdateButtons();
        }
        else //sell property
        {
            bankController.PayPlayerFromBank(owner, property.GetCost());
            bankController.AddProperty(property);
            owner.RemoveProperty(property);

            property.ResetProperty();
            gameController.TogglePropertiesList(true);
        }
    }

    /// <summary>
    /// Method: ToggleRaiseFunds()
    /// --------------------------------------------
    /// Toggle the raise funds.
    /// </summary>
    /// <param name="state"></param>
    public void ToggleRaiseFunds(bool state)
    {
        //addHouseParent.SetActive(!state);
        //mortgageParent.SetActive(state);
        //sellPropertyParent.SetActive(state);
    }

    public void SetGameController(GameController gameController)
    {
        this.gameController = gameController;
    }

    public void SetBankController(BankController bankController)
    {
        this.bankController = bankController;
    }

    public void SetProperty(PurchaseableProperty property)
    {
        this.property = property;
    }

    public void SetOwner(Player owner)
    {
        this.owner = owner;
    }
}
