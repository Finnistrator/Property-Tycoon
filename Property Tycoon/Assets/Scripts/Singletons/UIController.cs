using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class: UIController
/// </summary>
public class UIController : Singleton<UIController>
{
    [SerializeField] private GameObject purchasePropertyParent;
	[SerializeField] private GameObject getOutOfJailWindow;
	[SerializeField] private GameObject optionCardWindow;
	[SerializeField] private GameObject raiseFundsWindow;
	[SerializeField] private GameObject auctionWindow;
    [SerializeField] private TextMeshProUGUI buyButtonText;
    [SerializeField] private GameObject purchasePropertyButton;


    [Header("Purchase Property Window - Normal Property")]
    [SerializeField] private GameObject purchasePropertyWindow;
    [Space]
    [SerializeField] private Image propertyGroupColour;
    [SerializeField] private TextMeshProUGUI propertyNameText;
    [SerializeField] private TextMeshProUGUI propertyPriceText;
    [SerializeField] private TextMeshProUGUI propertyMortgageText;
    [SerializeField] private TextMeshProUGUI propertyRentText;
    [SerializeField] private TextMeshProUGUI propertyRentWithColourText;
    [SerializeField] private TextMeshProUGUI propertyRent1HouseText;
    [SerializeField] private TextMeshProUGUI propertyRent2HouseText;
    [SerializeField] private TextMeshProUGUI propertyRent3HouseText;
    [SerializeField] private TextMeshProUGUI propertyRent4HouseText;
    [SerializeField] private TextMeshProUGUI propertyRentHotelText;
    [SerializeField] private TextMeshProUGUI propertyHouseCostText;
    [SerializeField] private TextMeshProUGUI propertyHotelCostText;

    [Header("Purchase Property Window - Station")]
    [SerializeField] private GameObject purchaseStationWindow;
    [Space]
    [SerializeField] private TextMeshProUGUI stationNameText;
    [SerializeField] private TextMeshProUGUI stationPriceText;
    [SerializeField] private TextMeshProUGUI stationMortgageText;

    [Header("Purchase Property Window - Station")]
    [SerializeField] private GameObject purchaseUtilityWindow;
    [Space]
    [SerializeField] private TextMeshProUGUI utilityNameText;
    [SerializeField] private TextMeshProUGUI utilityPriceText;
    [SerializeField] private TextMeshProUGUI utilityMortgageText;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private void Start()
    {
        HidePurchasePropertyWindow();
		ToggleGetOutOfJailWindow(false);
    }

    /// <summary>
    /// Method: HidePurchasePropertyWindow()
    /// --------------------------------------------------
    /// Hides the property windows UI
    /// </summary>
    public void HidePurchasePropertyWindow()
    {
        if (animator)
        {
            animator.SetBool("Show Purchase Property", false);
            Invoke(nameof(DisablePropertyWindows), 1.5f);
        }
    }

    private void DisablePropertyWindows()
    {
        purchasePropertyParent.SetActive(false);
        purchasePropertyWindow.SetActive(false);
        purchaseStationWindow.SetActive(false);
        purchaseUtilityWindow.SetActive(false);
    }

    /// <summary>
    /// Method: ShowPurchasePropertyWindow()
    /// ----------------------------------------------------
    /// Shows the property window for a certain purchaseable
    /// property.
    /// </summary>
    /// <param name="property"></param>
    public void ShowPurchasePropertyWindow(PurchaseableProperty property)
    {
        if (buyButtonText)
        {
            purchasePropertyParent.SetActive(true);
            buyButtonText.text = "Buy £" + property.GetCost();
            purchasePropertyButton.SetActive(BankController.Instance.HasEnoughBalance(GameController.Instance.GetCurrentPlayer(), property.GetCost()));

            if (property.GetGroup() == Group.Station)
            {
                purchaseStationWindow.SetActive(true);
                stationNameText.text = property.name;
                stationPriceText.text = "£" + property.GetCost();
                stationMortgageText.text = "£" + (property.GetCost() / 2f);
            }
            else if (property.GetGroup() == Group.Utilities)
            {
                purchaseUtilityWindow.SetActive(true);
                utilityNameText.text = property.name;
                utilityPriceText.text = "£" + (property.GetCost());
                utilityMortgageText.text = "£" + (property.GetCost() / 2f);
            }
            else
            {
                purchasePropertyWindow.SetActive(true);
                propertyGroupColour.color = ColourController.Instance.GetGroupColour(property.GetGroup());
                propertyNameText.text = property.name;
                propertyPriceText.text = "£" + property.GetCost();
                propertyMortgageText.text = "£" + (property.GetCost() / 2f);
                propertyRentText.text = "£" + property.GetRent0Houses();
                propertyRentWithColourText.text = "£" + (property.GetRent0Houses() * 2);
                propertyRent1HouseText.text = "£" + property.GetRent1House();
                propertyRent2HouseText.text = "£" + property.GetRent2House();
                propertyRent3HouseText.text = "£" + property.GetRent3House();
                propertyRent4HouseText.text = "£" + property.GetRent4House();
                propertyRentHotelText.text = "£" + property.GetRentHotel();
                propertyHouseCostText.text = "£" + ImportController.Instance.GetHouseCost(property.GetGroup());
                propertyHotelCostText.text = "£" + ImportController.Instance.GetHotelCost(property.GetGroup()) + " (+4H)";
            }

            animator.SetBool("Show Purchase Property", true);
        }
    }

    /// <summary>
    /// Method: ToggleGetOutOfJailWindow()
    /// -------------------------------------------------------
    /// Displays the get out of jail window or disable it depending
    /// on the state.
    /// </summary>
    /// <param name="state"></param>
    public void ToggleGetOutOfJailWindow(bool state)
    {
        getOutOfJailWindow.SetActive(state);
    }

    /// <summary>
    /// Method: ToggleOptionCardWindow()
    /// ---------------------------------------------------------
    /// Displays the option card window or disable it depending
    /// on the state.
    /// </summary>
    /// <param name="state"></param>
	public void ToggleOptionCardWindow(bool state)
    {
        optionCardWindow.SetActive(state);
    }

    public void SetTogglePurchasePropertyWindow(GameObject obj)
    {
        purchasePropertyWindow = obj;
    }

    public GameObject GetTogglePurchasePropertyWindow()
    {
        return purchasePropertyWindow;
    }

    public void ToggleRaiseFundsWindow(bool state)
    {
        raiseFundsWindow.SetActive(state);
    }

    /// <summary>
    /// Method: Show Auction
    /// -----------------------------------------------
    /// Display the Auction window.
    /// </summary>
    /// <param name="state"></param>
    public void ShowAuction(bool state)
    {
        auctionWindow.SetActive(state);
        animator.SetBool("Auction", state);
        if(!state)
        {
            HidePurchasePropertyWindow();
        }
    }
}
