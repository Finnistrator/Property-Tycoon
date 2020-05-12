using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class: Dice
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    [Header("DICE")]
    [SerializeField] private Transform[] positions;

    private Rigidbody rb;
    private bool numberFound = false;
    private float rollTimer = 1;
    
    //Finds the required components
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        transform.rotation = Quaternion.Euler(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360));
    }

    /// <summary>
    /// Method: RollDice()
    /// -------------------------------------------------------------
    /// Rolls the dice, parsing in the dice throw velocity
    /// </summary>
    /// <param name="throwSpeed"></param>
    /// <param name="spinSpeed"></param>
    public void RollDice(float throwSpeed, float spinSpeed)
    {
        rb.AddForce(new Vector3(0, throwSpeed, 0));
        rb.AddTorque(new Vector3(Random.Range(0, spinSpeed), Random.Range(0, spinSpeed), Random.Range(0, spinSpeed)));
    }

    //Checks every frame to see if the dice has stopped rolling, waits 1 second before checking as velocity is zero to begin with
    private void Update()
    {
        rollTimer -= Time.deltaTime;

        if(rollTimer <= 0 && !numberFound && rb.velocity == Vector3.zero)
        {
            Transform highestNumberPos = null;
            float highestPoint = 0;
            foreach (Transform pos in positions)
            {
                if (pos.transform.position.y > highestPoint)
                {
                    highestNumberPos = pos;
                    highestPoint = pos.transform.position.y;
                }
            }

            numberFound = true;
            DiceController.Instance.SetRolledNumber(int.Parse(highestNumberPos.name));
            Destroy(gameObject, 1);
        }
    }
}