using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Class: PlayerFunds
/// </summary>
public class PlayerFunds : MonoBehaviour
{
    [Header("Player Funds")]
    [SerializeField] private Image playerIcon;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private float balanceChangeRate = 1;
    [SerializeField] private GameObject bidButton;
    [SerializeField] private GameObject outText;
    [SerializeField] private GameObject jail;

    private Player player;
    private float currentBalance = 0;

    /// <summary>
    /// Method: SetPlayer()
    /// --------------------------------
    /// Set the player
    /// </summary>
    /// <param name="player"></param>
    public void SetPlayer(Player player)
    {
        this.player = player;
        playerIcon.sprite = player.GetIcon();

        playerNameText.text = player.GetPlayerName();
        
        currentBalance = 1500;

        //TODO set player icon based on token
    }

    private void FixedUpdate()
    {
        jail.SetActive(player.IsInJail());

        if((!BankController.Instance.IsAuctioning() && GameController.Instance.GetCurrentPlayer() == player) || (BankController.Instance.IsAuctioning() && BankController.Instance.GetCurrentAuctioningPlayer() == player))
        {
            playerNameText.color = Color.green;
        }
        else
        {
            playerNameText.color = Color.white;
        }

        if(currentBalance < player.GetBalance())
        {
            currentBalance += 0.01f * balanceChangeRate;

            if(currentBalance > player.GetBalance())
            {
                currentBalance = player.GetBalance();
            }
        }
        else if(currentBalance > player.GetBalance())
        {
            currentBalance -= 0.01f * balanceChangeRate;

            if (currentBalance < player.GetBalance())
            {
                currentBalance = player.GetBalance();
            }
        }

        balanceText.text = "£" + currentBalance.ToString("F2");

        if(BankController.Instance.IsAuctioning() && !player.IsAI())
        {
            if(BankController.Instance.GetCurrentAuctioningPlayer() == player)
            {
                if(BankController.Instance.GetCurrentBid() == player.GetBalance() && player.GetBalance() == BankController.Instance.GetNextBidAmount(player.GetBalance()) && BankController.Instance.GetHighestBid() != player)
                {
                    BankController.Instance.RemovePlayerFromAuction();
                    bidButton.SetActive(false);
                    outText.SetActive(true);
                    return;
                }

                if(player.GetBalance() >= BankController.Instance.GetNextBidAmount(player.GetBalance()))
                {
                    bidButton.SetActive(true);
                    bidButton.GetComponentInChildren<TextMeshProUGUI>().text = "BID £" + BankController.Instance.GetNextBidAmount(player.GetBalance());
                }
                else
                {
                    BankController.Instance.RemovePlayerFromAuction();
                    bidButton.SetActive(false);
                    outText.SetActive(true);
                }
            }
            else
            {
                bidButton.SetActive(false);
            }
        }
        else if(BankController.Instance.IsAuctioning() && !BankController.Instance.IsPlayerStillInAuction(player))
        {
            bidButton.SetActive(false);
            outText.SetActive(true);
        }
        else
        {
            bidButton.SetActive(false);
            outText.SetActive(false);
        }
    }

    public Player GetPlayer()
    {
        return player;
    }

    /// <summary>
    /// Method: Bid()
    /// ---------------------------------------------
    /// Bid in the auction
    /// </summary>
    public void Bid()
    {
        BankController.Instance.BidInAuction();
    }
}
