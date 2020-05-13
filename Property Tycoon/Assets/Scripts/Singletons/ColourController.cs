using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class: ColourController
/// </summary>
public class ColourController : Singleton<ColourController>
{
    [Header("Groups")]
    [SerializeField] private Color GroupBrownColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color GroupBlueColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color GroupPurpleColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color GroupOrangeColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color GroupRedColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color GroupYellowColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color GroupGreenColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color GroupDeepBlueColour = new Color(1, 1, 1, 1);
    [Space]
    [SerializeField] private Color GoColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color JustVisitingColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color FreeParkingColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color GoToJailColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color StationColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color UtilitiesColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color OpportunityKnocksColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color PotLuckColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color IncomeTaxColour = new Color(1, 1, 1, 1);
    [SerializeField] private Color SuperTaxColour = new Color(1, 1, 1, 1);

    [Header("UI")]
    [SerializeField] private Color enabledColour;
    [SerializeField] private Color disabledColour;
    [SerializeField] private Color redColour;

    [Header("Icons")]
    [SerializeField] private Sprite station;
    [SerializeField] private Sprite utility;
    [SerializeField] private Sprite potLuck;
    [SerializeField] private Sprite opportunityKnocks;
    [SerializeField] private Sprite incomeTax;
    [SerializeField] private Sprite superTax;
    [SerializeField] private Sprite freeParking;
    [SerializeField] private Sprite goToJail;

    [Header("Models")]
    [SerializeField] private GameObject goblet;
    [SerializeField] private GameObject hatstand;
    [SerializeField] private GameObject cat;
    [SerializeField] private GameObject phone;
    [SerializeField] private GameObject spoon;
    [SerializeField] private GameObject boot;

    public GameObject GetModel(IconChoice choice)
    {
        switch (choice)
        {
            case IconChoice.Boot: return boot;
            case IconChoice.Cat: return cat;
            case IconChoice.Goblet: return goblet;
            case IconChoice.HatStand: return hatstand;
            case IconChoice.Smartphone: return phone;
            case IconChoice.Spoon: return spoon;
        }

        return null;
    }

    /// <summary>
    /// Method: GetPropertyIcon
    /// -------------------------------------------
    /// Returns the icon for right group
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    public Sprite GetPropertyIcon(Group group)
    {
        switch (group)
        {
            case Group.Station: return station;
            case Group.Utilities: return utility;
            case Group.OpportunityKnocks: return opportunityKnocks;
            case Group.PotLuck: return potLuck;
            case Group.IncomeTax: return incomeTax;
            case Group.SuperTax: return superTax;
            case Group.FreeParking: return freeParking;
            case Group.GoToJail: return goToJail;
        }

        return null;
    }

    /// <summary>
    /// Method: GetGroupColour()
    /// -----------------------------------------------
    /// Returns Color for corresoding group.
    /// </summary>
    /// <param name="group"></param>
    /// <returns></returns>
    public Color GetGroupColour(Group group)
    {
        switch (group)
        {
            case Group.Brown: return GroupBrownColour;
            case Group.Blue: return GroupBlueColour;
            case Group.Purple: return GroupPurpleColour;
            case Group.Orange: return GroupOrangeColour;
            case Group.Red: return GroupRedColour;
            case Group.Yellow: return GroupYellowColour;
            case Group.Green: return GroupGreenColour;
            case Group.DeepBlue: return GroupDeepBlueColour;
            case Group.Go: return GoColour;
            case Group.JustVisiting: return JustVisitingColour;
            case Group.FreeParking: return FreeParkingColour;
            case Group.GoToJail: return GoToJailColour;
            case Group.Station: return StationColour;
            case Group.Utilities: return UtilitiesColour;
            case Group.OpportunityKnocks: return OpportunityKnocksColour;
            case Group.PotLuck: return PotLuckColour;
            case Group.IncomeTax: return IncomeTaxColour;
            case Group.SuperTax: return IncomeTaxColour;
        }

        return Color.black;
    }

    public Color GetEnabledColour()
    {
        return enabledColour;
    }

    public Color GetDisabledColour()
    {
        return disabledColour;
    }

    public Color GetRedColour()
    {
        return redColour;
    }
}

/// <summary>
/// Enum: Group
/// </summary>
public enum Group
{
    Brown,
    Blue,
    Purple,
    Orange,
    Red,
    Yellow,
    Green,
    DeepBlue,

    Go,
    JustVisiting,
    FreeParking,
    GoToJail,
    Station,
    Utilities,
    OpportunityKnocks,
    PotLuck,
    IncomeTax,
    SuperTax
};
