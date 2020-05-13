using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CardClassScriptTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void IntializationTest()
        {
            Card test = new MoneyCard(100, "You inherit £100");
            Assert.NotNull(test);
        }

        [Test]
        public void TestMoveCard()
        {
            MoveStepsCard test = new MoveStepsCard(4, "Move 4 places forward.");
            Assert.NotNull(test);
            Assert.AreEqual(4, test.GetSteps());
            Assert.AreEqual("Move 4 places forward.", test.GetDescription());
        }

        [Test]
        public void TestMovePropertyCard()
        {
            MovePropertyCard test = new MovePropertyCard(4, "Move to testSpace");
            Assert.NotNull(test);
            Assert.AreEqual(4, test.GetProperty());
            Assert.AreEqual("Move to testSpace", test.GetDescription());
        }

        [Test]
        public void TestPayFineOrOpportunityKnocksCard()
        {
            PayFineOrOpportunityKnocksCard test = new PayFineOrOpportunityKnocksCard("Pay $100 for school fees.", 100);
            Assert.NotNull(test);
            Assert.AreEqual("Pay $100 for school fees.", test.GetDescription());
        }

        [Test]
        public void TestGoToJailCard()
        {
            GoToJailCard test = new GoToJailCard("Go to jail");
            Assert.NotNull(test);
            Assert.AreEqual("Go to jail", test.GetDescription());
        }

        [Test]
        public void TestGetOutOfJailFreeCardPL()
        {
            GetOutOfJailFreeCard test = new GetOutOfJailFreeCard("Get out of jail free", true);
            Assert.NotNull(test);
            Assert.AreEqual("Get out of jail free", test.GetDescription());
            Assert.AreEqual(true, test.getIsFromPotLuck());
        }

        [Test]
        public void TestGetOutOfJailFreeCardOK()
        {
            GetOutOfJailFreeCard test = new GetOutOfJailFreeCard("Get out of jail free", false);
            Assert.NotNull(test);
            Assert.AreEqual("Get out of jail free", test.GetDescription());
            Assert.AreEqual(false, test.getIsFromPotLuck());
        }

        [Test]
        public void TestInteractionWinMoney()
        {
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject player = new GameObject();
            player.AddComponent<Player>();
            GameObject moneyCard = new GameObject();
            MoneyCard card = new MoneyCard(40,"you win 40 from videogame");
            player.GetComponent<Player>().SetBalance(1500);
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            card.SetBankController(bankController.GetComponent<BankController>());
            card.SetGameController(gameController.GetComponent<GameController>());
            card.Interact();
            Assert.AreEqual(1540, gameController.GetComponent<GameController>().GetCurrentPlayer().GetBalance());
        }

        [Test]
        public void TestInteractionPayFreeParking()
        {
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject player = new GameObject();
            player.AddComponent<Player>();
            GameObject moneyCard = new GameObject();
            MoneyCard card = new MoneyCard(40, "you win 40 from videogame", MoneyCardType.ToFreeParking);
            player.GetComponent<Player>().SetBalance(1500);
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            card.SetBankController(bankController.GetComponent<BankController>());
            card.SetGameController(gameController.GetComponent<GameController>());
            card.Interact();
            Assert.AreEqual(1460, gameController.GetComponent<GameController>().GetCurrentPlayer().GetBalance());
        }

        [Test]
        public void TestInteractionPayingPlayers()
        {
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject player = new GameObject();
            player.AddComponent<Player>();
            GameObject player2 = new GameObject();
            player2.AddComponent<Player>();
            player2.GetComponent<Player>().SetBalance(1500);
            GameObject moneyCard = new GameObject();
            MoneyCard card = new MoneyCard(40, "you win 40 from videogame", MoneyCardType.FromPlayers);
            player.GetComponent<Player>().SetBalance(1500);
            Player[] players = new Player[2];
            players[0] = player.GetComponent<Player>();
            players[1] = player2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(players);
            card.SetBankController(bankController.GetComponent<BankController>());
            card.SetGameController(gameController.GetComponent<GameController>());
            card.Interact();
            Assert.AreEqual(1540, gameController.GetComponent<GameController>().GetCurrentPlayer().GetBalance());
            Assert.AreEqual(1460, player2.GetComponent<Player>().GetBalance());

        }

        [Test]
        public void TestForMovePropertyCard()
        {
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            GameObject player = new GameObject();
            player.AddComponent<Player>();
            GameObject player2 = new GameObject();
            player2.AddComponent<Player>();
            player2.GetComponent<Player>().SetBalance(1500);
            GameObject moneyCard = new GameObject();
            MovePropertyCard card = new MovePropertyCard(1, "Move to x property");
            Player[] players = new Player[2];
            players[0] = player.GetComponent<Player>();
            players[1] = player2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(players);
            card.SetGameController(gameController.GetComponent<GameController>());
            card.Interact();
            Assert.AreEqual(1, gameController.GetComponent<GameController>().GetCurrentPlayer().getTargetPos());

        }
    
        [Test]
        public void TestGoToJailInteraction()
        {
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            GameObject bankController = new GameObject();
            GameObject player = new GameObject();
            player.AddComponent<Player>();
            player.GetComponent<Player>().SetBalance(1500);
            GameObject moneyCard = new GameObject();
            GoToJailCard card = new GoToJailCard("Go to Jail");
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            card.SetGameController(gameController.GetComponent<GameController>());
            card.Interact();
            Assert.AreEqual(true, player.GetComponent<Player>().IsInJail());


        }

    }
}
