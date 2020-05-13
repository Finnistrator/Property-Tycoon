using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class: PropertyCard
/// </summary>
public class PropertyCard : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cardDescriptionText;
    //  [SerializeField] private Renderer colourRenderer;

    /// <summary>
    /// Method: updateInfo()
    /// -----------------------------------------------
    /// Updates the infor for purchaseable property
    /// </summary>
    /// <param name="property"></param>
    public void updateInfo(PurchaseableProperty property)
    {
        cardName.text = property.name;
        cardDescriptionText.text = ("rent with 0 houses" + property.GetRent0Houses() + "  " + "rent with 1 house" +
                                    property.GetRent1House());
        
        transform.Find("ColourPanel").GetComponent<Image>().color = ColourController.Instance.GetGroupColour(property.GetGroup());
    }

//    public void moveToFront()
//    {
//        transform.SetAsLastSibling(); //move to front
//    }
}
