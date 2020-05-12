using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class: Player
/// </summary>
public class Player : MonoBehaviour
{
    [Header("Player Info")]
    [SerializeField] private string playerName;
    [SerializeField] private float speed;
    [SerializeField] private bool AI;

    [Header("Bank Info")]
    [SerializeField] private float bankBalance;
    [SerializeField] private List<PurchaseableProperty> ownedProperties = new List<PurchaseableProperty>();

    [Header("Sound Effects")]
    [SerializeField] private AudioClip moveSound;

    private Sprite icon;
    private int currentPos = 0;
    private int targetPos = 0;
    private int lastPos = 0;

    private PropertyController[] boardPositions;

    private bool moving = false;
    private bool inJail = false;
	private int turnsInJail = 0;
	private bool isBankrupt = false;
	private int amountOfRolledDoubles = 0;
    private GameController gc = GameController.Instance;

    private bool oneLapCompleted = false;

    private Vector3 targetVector;

    private void Start()
    {
        boardPositions = GameController.Instance.GetProperties();
        targetVector = boardPositions[currentPos].ArriveAtProperty(this);
        transform.position = targetVector;
    }

    /// <summary>
    /// Method: StartTurn()
    /// ---------------------------------------------
    /// Starts the turn for player.
    /// </summary>
    public void StartTurn()
    {
        if(AI)
        {
            DiceController.Instance.RollDice();
        }
    }

    private void FixedUpdate()
    {
        Quaternion targetRotation;

        if (currentPos >= 0 && currentPos <= 10)
        {
            targetRotation = Quaternion.Euler(0, 90, 0);
        }
        else if (currentPos > 10 && currentPos <= 20)
        {
            targetRotation = Quaternion.Euler(0, 180, 0);
        }
        else if (currentPos > 20 && currentPos <= 30)
        {
            targetRotation = Quaternion.Euler(0, 270, 0);
        }
        else
        {
            targetRotation = Quaternion.Euler(0, 360, 0);
        }

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, 5 * Time.deltaTime);

        if (moving)
        {
            transform.position = Vector3.Lerp(transform.position, targetVector, speed * Time.deltaTime);

            //If halfway across the board, mark one lap completed as true
            if(currentPos == 39)
            {
                oneLapCompleted = true;
            }

            //If destination is in front of the player
            if (currentPos < targetPos && AtTargetPosition())
            {
                lastPos = currentPos;
                boardPositions[currentPos].LeaveProperty(this);
                currentPos++;

                //Wrap around
                if (currentPos >= boardPositions.Length)
                {
                    currentPos -= boardPositions.Length;
                    targetPos -= boardPositions.Length;
                }

                targetVector = boardPositions[currentPos].ArriveAtProperty(this);
                if (currentPos == 0)
                {
                    BankController.Instance.PayPlayerFromBank(this, 200);
                }
            }
            //If destination is behind player
            else if (currentPos > targetPos && AtTargetPosition())
            {
                lastPos = currentPos;
                boardPositions[currentPos].LeaveProperty(this);
                currentPos--;

                //Wrap around
                if (currentPos < 0)
                {
                    currentPos += boardPositions.Length;
                    targetPos += boardPositions.Length;
                }

                targetVector = boardPositions[currentPos].ArriveAtProperty(this);
            }

            //If player passes go, collect £200
            

            //If at target position and player has stopped moving, arrive at property
            if (currentPos == targetPos && AtTargetPosition())
            {
                moving = false;
                GameController.Instance.OnArriveAtProperty(boardPositions[targetPos].GetProperty());
                return;
            }

            //Play movement sound
            if(currentPos != lastPos)
            {
                lastPos = currentPos;
                AudioController.Instance.PlaySound(moveSound, 15, 0.05f);
            }

            //Rotate the camera based on position on the board
            CameraController.Instance.ChangeCameraRotation(currentPos);
        }
    }

    /// <summary>
    /// Method: AtTargetPosition()
    /// ---------------------------------------------------
    /// Checks if player at target position.
    /// </summary>
    /// <returns></returns>
    private bool AtTargetPosition()
    {
        return (Mathf.Abs(transform.position.x - targetVector.x) < 0.1f) && (Mathf.Abs(transform.position.z - targetVector.z) < 0.1f);
    }

    /// <summary>
    /// Method: Move()
    /// -------------------------------------------------------------------
    /// Moves the player a number of steps
    /// </summary>
    /// <param name="steps"></param>
    public void Move(int steps)
    {
        moving = true;
        targetPos += steps;
    }

    /// <summary>
    /// Method: GetNumberOfSameColourProperties
    /// -----------------------------------------------------------
    /// Get the number of same colour properties
    /// </summary>
    /// <param name="targetProperty"></param>
    /// <returns></returns>
    public int GetNumberOfSameColourProperties(PurchaseableProperty targetProperty)
    {
        int count = 0;

        foreach (PurchaseableProperty property in ownedProperties)
        {
            if(property.GetGroup() == targetProperty.GetGroup())
            {
                count++;
            }
        }

        return count;
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

    public int GetCurrentPos()
    {
        return currentPos;
    }

    public void AddBalance(float amount)
    {
        bankBalance += amount;
    }

    public void SetBalance(float amount)
    {
        bankBalance = amount;
        if (GameController.Instance) {
            GameController.Instance.GetPlayerFunds(this).SetBalance(bankBalance);
        }
    }

    public float GetBalance()
    {
        return bankBalance;
    }

    public void AddProperty(PurchaseableProperty property)
    {
        ownedProperties.Add(property);
    }

    public void RemoveProperty(PurchaseableProperty property)
    {
        ownedProperties.Remove(property);
    }

    public List<PurchaseableProperty> GetOwnedProperties()
    {
        return ownedProperties;
    }

    public bool IsAI()
    {
        return AI;
    }

    /// <summary>
    /// Method: GoToJail()
    /// -----------------------------------------------------
    /// Set player in jail
    /// </summary>
    public void GoToJail()
    {
        currentPos = 10;
        targetPos = 10;
        inJail = true;
		turnsInJail = 2;
    }

    public bool IsInJail()
    {
        return inJail;
    }

    public int getTargetPos()
    {
        return targetPos;
    }

    public bool HasCompletedOneLap()
    {
        return oneLapCompleted;
    }

    public void SetAI(bool boolean)
    {
        AI = boolean;
    }

    public bool GetAI()
    {
        return AI;
    }

    /// <summary>
    /// Method: GetIsBankrupt()
    /// -----------------------------------
    /// Check if player is bankrupt.
    /// </summary>
    /// <returns></returns>
	public bool GetIsBankrupt()
	{
		return isBankrupt;
	}

    /// <summary>
    /// Method: ReduceTurnsInJail()
    /// -----------------------------------------------
    /// Reduces the turn in jail.
    /// </summary>
	public void ReduceTurnsInJail()
	{
		if(turnsInJail < 1)
		{
            GetOutOfJail();
        }
		else{
			turnsInJail--;
		}
	}
    
    /// <summary>
    /// Method: GetOutOfJail()
    /// --------------------------------------
    /// Gets player out of jail
    /// </summary>
    public void GetOutOfJail()
    {
        turnsInJail = 0;
        inJail = false;
        transform.position = targetVector;
    }

	public int GetTurnsInJail()
	{
		return turnsInJail;
	}

    public void SetCurrentPos(int pos)
    {
        currentPos = pos;
        targetPos = pos;
        targetVector = boardPositions[currentPos].ArriveAtProperty(this);
    }

	public void SetAmountOfRolledDoubles(int amount)
	{
		amountOfRolledDoubles = amount;
	}

	public int GetAmountOfRolledDoubles()
	{
		return amountOfRolledDoubles;
	}

    public void SetIcon(Sprite sprite)
    {
        icon = sprite;
    }

    public void SetModel(IconChoice choice)
    {
        Instantiate(ColourController.Instance.GetModel(choice), transform.position, Quaternion.identity, transform);
    }

    public Sprite GetIcon()
    {
        return icon;
    }

    public void GoBankrupt()
    {
        isBankrupt = true;
    }
}
