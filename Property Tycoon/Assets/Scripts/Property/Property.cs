using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class: Property
/// </summary>
[CreateAssetMenu]
public class Property : ScriptableObject
{
    [SerializeField] protected Group Group;

    /// <summary>
    /// Contructor for class
    /// </summary>
    /// <param name="name"></param>
    /// <param name="group"></param>
    public Property(string name, Group group)
    {
        this.name = name;
        Group = group;
    }

    public Group GetGroup()
    {
        return Group;
    }
}
