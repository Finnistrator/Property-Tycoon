using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerFunds : MonoBehaviour
{
    [Header("Player Funds")]
    [SerializeField] private Transform card;
    [SerializeField] private Image playerIcon;
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI balanceText;
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private float balanceChangeRate = 1;
    [SerializeField] private GameObject bidButton;
    [SerializeField] private GameObject outText;
    [SerializeField] private GameObject jail;
    [SerializeField] private TextMeshProUGUI blue;
    [SerializeField] private TextMeshProUGUI brown;
    [SerializeField] private TextMeshProUGUI deepBlue;
    [SerializeField] private TextMeshProUGUI green;
    [SerializeField] private TextMeshProUGUI orange;
    [SerializeField] private TextMeshProUGUI red;
    [SerializeField] private TextMeshProUGUI purple;
    [SerializeField] private TextMeshProUGUI yellow;

    private Player player;
    private float currentBalance = 0;
    private float originY;

    public void SetPlayer(Player player)
    {
        originY = card.position.x;
        this.player = player;
        playerIcon.sprite = player.GetIcon();

        playerNameText.text = player.GetPlayerName();

        currentBalance = player.GetBalance();

        //TODO set player icon based on token
    }

    private void FixedUpdate()
    {
        jail.SetActive(player.IsInJail());

        blue.text = NumberOfColour(Group.Blue);
        brown.text = NumberOfColour(Group.Brown);
        deepBlue.text = NumberOfColour(Group.DeepBlue);
        green.text = NumberOfColour(Group.Green);
        orange.text = NumberOfColour(Group.Orange);
        red.text = NumberOfColour(Group.Red);
        purple.text = NumberOfColour(Group.Purple);
        yellow.text = NumberOfColour(Group.Yellow);
        
        if ((!BankController.Instance.IsAuctioning() && GameController.Instance.GetCurrentPlayer() == player) || (BankController.Instance.IsAuctioning() && BankController.Instance.GetCurrentAuctioningPlayer() == player))
        {
            playerNameText.color = Color.green; //y z x
            card.position = Vector3.Lerp(card.position, new Vector3(originY + 16, card.position.y, card.position.z), 5 * Time.deltaTime);
        }
        else
        {
            playerNameText.color = Color.white;
            card.position = Vector3.Lerp(card.position, new Vector3(originY + 14, card.position.y, card.position.z), 5 * Time.deltaTime);
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

    private string NumberOfColour(Group group)
    {
        int count = 0;

        foreach (PurchaseableProperty property in player.GetOwnedProperties())
        {
            if(property.GetGroup() == group)
            {
                count++;
            }
        }

        return count.ToString();
    }

    public Player GetPlayer()
    {
        return player;
    }

    public void Bid()
    {
        BankController.Instance.BidInAuction();
    }
}
