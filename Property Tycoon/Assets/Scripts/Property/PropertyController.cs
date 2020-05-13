    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class: PropertyController
/// </summary>
public class PropertyController : MonoBehaviour
{
    [SerializeField] private Property property;
    [Space]
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private TextMeshPro unpurchaseableText;
    [SerializeField] private Renderer colourRenderer;
    [SerializeField] private TextMeshPro costText;
    [SerializeField] private Transform landingPositionsParent;
    [SerializeField] private GameObject[] houses;
    [SerializeField] private GameObject hotel;
    [SerializeField] private SpriteRenderer icon;
    [SerializeField] private bool permanent = false;

    private Player[] playersAtProperty;
    private List<Transform> movePositions = new List<Transform>();

    private void Awake()
    {
        foreach (Transform trans in landingPositionsParent.GetComponentsInChildren<Transform>())
        {
            if (trans != landingPositionsParent)
            {
                movePositions.Add(trans);
            }
        }
        playersAtProperty = new Player[movePositions.Count];
    }

    private void Start()
    {
        if(!property)
        {
            return;
        }

        name = property.name;

        if (property is PurchaseableProperty && property.GetGroup() != Group.IncomeTax && property.GetGroup() != Group.SuperTax && property.GetGroup() != Group.Station && property.GetGroup() != Group.Utilities)
        {
            ((PurchaseableProperty)property).ResetProperty();
            nameText.text = property.name;
            colourRenderer.material.color = ColourController.Instance.GetGroupColour(property.GetGroup());
            costText.text = "£" + ((PurchaseableProperty)property).GetCost();
        }
        else if(!permanent)
        {
            unpurchaseableText.text = property.name;
            icon.sprite = ColourController.Instance.GetPropertyIcon(property.GetGroup());
            icon.color = ColourController.Instance.GetGroupColour(property.GetGroup());
        }

        UpdateProperty();
    }
    
    public Property GetProperty()
    {
        return property;
    }

    public void SetProperty(Property prop)
    {
        property = prop;
    }

    /// <summary>
    /// Method: ArriveAtProperty
    /// ------------------------------------------------------
    /// Returns the positon of the player at property on the card
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public Vector3 ArriveAtProperty(Player player)
    {
        for (int i = 0; i < playersAtProperty.Length; i++)
        {
            if (playersAtProperty[i] == null)
            {
                playersAtProperty[i] = player;
                return movePositions[i].position;
            }
        }

        Debug.LogError("Not enough room at this property!");
        return Vector3.zero;
    }

    /// <summary>
    /// Method: LeaveProperty()
    /// ---------------------------------------------------------
    /// Removes player postion on property and shift all other players
    /// other.
    /// </summary>
    /// <param name="player"></param>
    public void LeaveProperty(Player player)
    {
        for (int i = 0; i < playersAtProperty.Length; i++)
        {
            if (playersAtProperty[i] == player)
            {
                playersAtProperty[i] = null;
                return;
            }
        }

        Debug.LogError("Player isn't at property to begin with!");
    }

    /// <summary>
    /// Method: UpdateProperty()
    /// -------------------------------------------------------------------
    /// Updates the property
    /// </summary>
    public void UpdateProperty()
    {
        if (colourRenderer)
        {
            if (property is PurchaseableProperty && property.GetGroup() != Group.Station && property.GetGroup() != Group.Utilities)
            {
                if (((PurchaseableProperty)property).GetHouses() < 5)
                {
                    for (int i = 0; i < houses.Length; i++)
                    {
                        if (i < ((PurchaseableProperty)property).GetHouses())
                        {
                            houses[i].SetActive(true);
                        }
                        else
                        {
                            houses[i].SetActive(false);
                        }
                    }
                    hotel.SetActive(false);
                }
                else
                {
                    for (int i = 0; i < houses.Length; i++)
                    {
                        houses[i].SetActive(false);
                    }

                    hotel.SetActive(true);
                }
            }
            else if(hotel)
            {
                for (int i = 0; i < houses.Length; i++)
                {
                    houses[i].SetActive(false);
                }

                hotel.SetActive(false);
            }
        }
    }

    public bool IsPermanent()
    {
        return permanent;
    }
}
