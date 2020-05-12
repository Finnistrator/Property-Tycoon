using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class: TurnAnimator
/// </summary>
public class TurnAnimator : MonoBehaviour
{
    /// <summary>
    /// Method: StartTurn()
    /// -------------------------------------------
    /// Start the animation for start turn
    /// </summary>
    public void StartTurn()
    {
        GameController.Instance.StartTurn();
    }
}
