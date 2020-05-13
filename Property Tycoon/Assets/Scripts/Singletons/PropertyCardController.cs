using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class: PropertyCardController
/// </summary>
public class PropertyCardController : Singleton<PropertyCardController>
{
    // Start is called before the first frame update
    
    [Header("PropertyCard Prefab")]
    [SerializeField] private PropertyCard propertyCardPrefab;

    [SerializeField] private GameObject PropertyCardContainer;
    

    private PropertyCard Instance;
    private float posX = 0;
    private float posY = 0;
    

    private void Start()
    {

    }

    /// <summary>
    /// Method: setUpCards()
    /// --------------------------------------------------------
    /// Sets up card for the player with the properties they control.
    /// </summary>
    /// <param name="currentPlayer"></param>
    public void setUpCards(Player currentPlayer)
    {
        Debug.Log("setting up cards");
        posX = (float) (100);
        posY = (float) (100);
        currentPlayer = GameController.Instance.GetCurrentPlayer();
        foreach (PurchaseableProperty property in currentPlayer.GetOwnedProperties())
            if (propertyCardPrefab)
            {
                Instance = Instantiate(propertyCardPrefab, new Vector3(posX, posY), Quaternion.identity);
                Instance.GetComponent<PropertyCard>().updateInfo(property);
                Instance.transform.SetParent(PropertyCardContainer.transform);
                Debug.Log("instantiated a card " + property.name);
                posX += 20;
            }

    }

    /// <summary>
    /// Method: destoryCards()
    /// -----------------------------------------------
    /// Removes cards from UI
    /// </summary>
    public void destoryCards()
    {
        foreach (Transform child in PropertyCardContainer.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
