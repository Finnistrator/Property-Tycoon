using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class: ImportController
/// </summary>
public class ImportController : Singleton<ImportController>
{
    [Header("Excel Import")]
    [SerializeField] private TextAsset import;

    private List<string> propertyColours = new List<string>() { "Brown", "Blue", "Purple", "Orange", "Red", "Yellow", "Green", "Deep Blue" };

    private float[] houseCosts;
    private float[] hotelCosts;

    private void Start()
    {
        ImportProperties();
        ImportHouseAndHotelCosts();
    }

    /// <summary>
    /// Method: GetHouseCost()
    /// --------------------------------------------
    /// Gets house prices for the property from the data.
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    public float GetHouseCost(Group group)
    {
        if((int)group > (int)Group.DeepBlue)
        {
            return -1;
        }
        return houseCosts[(int)group];
    }

    /// <summary>
    /// Method: GetHouseCost()
    /// --------------------------------------------
    /// Gets house prices for the property from the data.
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    public float GetHotelCost(Group group)
    {
        return hotelCosts[(int)group];
    }

    /// <summary>
    /// Method: ImportCards()
    /// ----------------------------------------------
    /// Imports all card data to the game.
    /// </summary>
    /// <param name="potLuck"></param>
    /// <param name="opportunityKnocks"></param>
    public void ImportCards(out List<Card> potLuck, out List<Card> opportunityKnocks)
    {
        potLuck = new List<Card>();
        opportunityKnocks = new List<Card>();

        string[] cardStrings = import.text.Split('\n');

        for (int i = 50; i < cardStrings.Length; i++)
        {
            string[] cardAttributes = cardStrings[i].Split(',');

            if (cardAttributes.Length > 1 && cardAttributes[1] != "")
            {
                string cardType = cardAttributes[1];
                string cardDescription = cardAttributes[2];
                float cardValue = float.Parse(cardAttributes[3]);
                int cardMoneyOrigin = -1;

                bool moveForwards = !cardDescription.ToLower().Contains("backward");
                bool moveCollectGo = !cardDescription.ToLower().Contains("from go");

                if(cardAttributes[4] == "From The Bank")
                {
                    cardMoneyOrigin = (int)MoneyCardType.FromBank;
                }
                else if(cardAttributes[4] == "To Free Parking")
                {
                    cardMoneyOrigin = (int)MoneyCardType.ToFreeParking;
                }
                else if(cardAttributes[4] == "From Players")
                {
                    cardMoneyOrigin = (int)MoneyCardType.FromPlayers;
                }

                bool toPotLuck = cardAttributes[5] == "Pot Luck";

                try
                {
                    switch (cardType)
                    {
                        case "Move Steps":
                            if(toPotLuck)
                            {
                                potLuck.Add(new MoveStepsCard((int)cardValue, cardDescription));
                            }
                            else
                            {
                                opportunityKnocks.Add(new MoveStepsCard((int)cardValue, cardDescription));
                            }
                            break;
                        case "Move To Property Number":
                            if (toPotLuck)
                            {
                                potLuck.Add(new MovePropertyCard((int)cardValue, cardDescription, moveForwards, moveCollectGo));
                            }
                            else
                            {
                                opportunityKnocks.Add(new MovePropertyCard((int)cardValue, cardDescription, moveForwards, moveCollectGo));
                            }
                            break;
                        case "Pay Money":
                            if (toPotLuck)
                            {
                                potLuck.Add(new MoneyCard((int)cardValue, cardDescription, (MoneyCardType)cardMoneyOrigin));
                            }
                            else
                            {
                                opportunityKnocks.Add(new MoneyCard((int)cardValue, cardDescription, (MoneyCardType)cardMoneyOrigin));
                            }
                            break;
                        case "Money Per Property":
                            if (toPotLuck)
                            {
                                potLuck.Add(new RepairAssessmentCard((int)cardValue, (int)cardValue, cardDescription));
                            }
                            else
                            {
                                opportunityKnocks.Add(new RepairAssessmentCard((int)cardValue, (int)cardValue, cardDescription));
                            }
                            break;
                        case "Pay Fine Or Take Opportunity Knocks Card":
                            if (toPotLuck)
                            {
                                potLuck.Add(new PayFineOrOpportunityKnocksCard(cardDescription, cardValue));
                            }
                            else
                            {
                                opportunityKnocks.Add(new PayFineOrOpportunityKnocksCard(cardDescription, cardValue));
                            }
                            break;
                        case "Go To Jail":
                            if (toPotLuck)
                            {
                                potLuck.Add(new GoToJailCard(cardDescription));
                            }
                            else
                            {
                                opportunityKnocks.Add(new GoToJailCard(cardDescription));
                            }
                            break;
                        case "Get Out Of Jail Free":
                            if (toPotLuck && !CardController.Instance.GetGOOJPL())
                            {
                                GetOutOfJailFreeCard card = new GetOutOfJailFreeCard(cardDescription, true);
                                CardController.Instance.SetjailCardPL(card);
                                potLuck.Add(card);
                            }
                            else if(!toPotLuck && !CardController.Instance.GetGOOJOK())
                            {
                                GetOutOfJailFreeCard card = new GetOutOfJailFreeCard(cardDescription, false);
                                CardController.Instance.SetjailCardOK(card);
                                opportunityKnocks.Add(card);
                            }
                            break;
                    }
                }
                catch(System.Exception e)
                {
                    Debug.LogError("Invalid card import. Dismissing...");
                }
            }
        }
    }

    private void ImportHouseAndHotelCosts()
    {
        houseCosts = new float[8];
        hotelCosts = new float[8];

        string[] costStrings = import.text.Split('\n');

        for (int i = 0; i < houseCosts.Length; i++)
        {
            houseCosts[i] = float.Parse(costStrings[40 + i].Split(',')[2].Remove(0, 1));
            hotelCosts[i] = float.Parse(costStrings[40 + i].Split(',')[3].Remove(0, 1));
        }
    }

    /// <summary>
    /// Method: ImportProperties()
    /// -----------------------------------------
    /// Returns a list of all properties for the game
    /// from the data.
    /// </summary>
    /// <returns></returns>
    public List<Property> ImportProperties()
    {
        List<Property> properties = new List<Property>();

        string[] propertyStrings = import.text.Split('\n');

        for (int i = 2; i < 38; i++)
        {
            string[] propertyAttributes = propertyStrings[i].Split(',');

            string propertyName = propertyAttributes[2];
            string propertyType = propertyAttributes[3];
            float propertyCost = GetFloatFromString(propertyAttributes[4]);
            float propertyBaseRent = GetFloatFromString(propertyAttributes[5]);
            float property1House = GetFloatFromString(propertyAttributes[6]);
            float property2House = GetFloatFromString(propertyAttributes[7]);
            float property3House = GetFloatFromString(propertyAttributes[8]);
            float property4House = GetFloatFromString(propertyAttributes[9]);
            float propertyHotel = GetFloatFromString(propertyAttributes[10]);

            try
            {
                if (propertyColours.Contains(propertyType))
                {
                    properties.Add(new PurchaseableProperty(propertyName, GetGroupFromString(propertyType), propertyCost, propertyBaseRent, property1House, property2House, property3House, property4House, propertyHotel));
                }
                else if (propertyType == "Station")
                {
                    properties.Add(new PurchaseableProperty(propertyName, Group.Station, propertyCost));
                }
                else if (propertyType == "Utilities")
                {
                    properties.Add(new PurchaseableProperty(propertyName, Group.Utilities, propertyCost));
                }
                else if (propertyType == "Pot Luck")
                {
                    properties.Add(new Property(propertyName, Group.PotLuck));
                }
                else if (propertyType == "Opportunity Knocks")
                {
                    properties.Add(new Property(propertyName, Group.OpportunityKnocks));
                }
                else if (propertyType == "Income Tax")
                {
                    properties.Add(new PurchaseableProperty(propertyName, Group.IncomeTax, propertyCost));
                }
                else if (propertyType == "Super Tax")
                {
                    properties.Add(new PurchaseableProperty(propertyName, Group.SuperTax, propertyCost));
                }
            }
            catch(System.Exception e)
            {
                Debug.LogError("Invalid property import. Dismissing...");
            }
        }

        return properties;
    }

    private float GetFloatFromString(string number)
    {
        if(number != "")
        {
            return float.Parse(number.Remove(0, 1));
        }

        return 0;
    }

    private Group GetGroupFromString(string group)
    {
        if (group == propertyColours[0]) return Group.Brown;
        else if (group == propertyColours[1]) return Group.Blue;
        else if (group == propertyColours[2]) return Group.Purple;
        else if (group == propertyColours[3]) return Group.Orange;
        else if (group == propertyColours[4]) return Group.Red;
        else if (group == propertyColours[5]) return Group.Yellow;
        else if (group == propertyColours[6]) return Group.Green;
        else return Group.DeepBlue;
    }
}
