using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class: PurchaseableProperty
/// </summary>
[CreateAssetMenu]
public class PurchaseableProperty : Property
{
    [SerializeField] private float cost;
    [SerializeField] private float rent0Houses;
    [SerializeField] private float rent1House;
    [SerializeField] private float rent2House;
    [SerializeField] private float rent3House;
    [SerializeField] private float rent4House;
    [SerializeField] private float rentHotel;

    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="name"></param>
    /// <param name="group"></param>
    /// <param name="cost"></param>
    public PurchaseableProperty(string name, Group group, float cost) : base(name, group)
    {
        this.cost = cost;
    }

    /// <summary>
    /// Constructor for class
    /// </summary>
    /// <param name="name"></param>
    /// <param name="group"></param>
    /// <param name="cost"></param>
    public PurchaseableProperty(string name, Group group, float cost, float rent0, float rent1, float rent2, float rent3, float rent4, float rentHotel) : base(name, group)
    {
        this.cost = cost;
        rent0Houses = rent0;
        rent1House = rent1;
        rent2House = rent2;
        rent3House = rent3;
        rent4House = rent4;
        this.rentHotel = rentHotel;
    }

    private int houses;
    private bool mortgaged;

    public void ResetProperty()
    {
        houses = 0;
        mortgaged = false;
    }

    public void AddHouse()
    {
        houses++;
    }

    public void RemoveHouse()
    {
        houses--;
    }

    public float GetCost()
    {
        return mortgaged ? cost / 2f : cost;
    }

    public float GetRent0Houses()
    {
        return rent0Houses;
    }

    public float GetRent1House()
    {
        return rent1House;
    }

    public float GetRent2House()
    {
        return rent2House;
    }

    public float GetRent3House()
    {
        return rent3House;
    }

    public float GetRent4House()
    {
        return rent4House;
    }

    public float GetRentHotel()
    {
        return rentHotel;
    }

    public void SetCost(float costs)
    {
        cost = costs;
    }

    public void SetRent0Houses(float houseRent)
    {
        rent0Houses = houseRent;
    }

    public void SetRent1House(float houseRent)
    {
        rent1House = houseRent;
    }

    public void SetRent2House(float houseRent)
    {
        rent2House = houseRent;
    }

    public void SetRent3House(float houseRent)
    {
        rent3House = houseRent;
    }

    public void SetRent4House(float houseRent)
    {
        rent4House = houseRent;
    }

    public void SetRentHotel(float houseRent)
    {
        rentHotel = houseRent;
    }

    public int GetHouses()
    {
        return houses;
    }

    public bool HasHotel()
    {
        return houses == 5;
    }

    public void SetMortgaged(bool state)
    {
        mortgaged = state;
    }

    public bool IsMortgaged()
    {
        return mortgaged;
    }

    public void SetGroup(Group group)
    {
        Group = group;
    }
}
