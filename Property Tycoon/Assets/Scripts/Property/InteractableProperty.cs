using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableProperty : Property
{
    public void LandOnProperty()
    {
        switch (Group)
        {
            case Group.Go:
                break;
            case Group.JustVisiting:
                break;
            case Group.FreeParking:
                break;
            case Group.GoToJail:
                break;
            case Group.OpportunityKnocks:
                break;
            case Group.PotLuck:
                break;
            case Group.IncomeTax:
                break;
        }
    }

    public InteractableProperty(string name, Group group) : base(name, group)
    {
       
    }

}
