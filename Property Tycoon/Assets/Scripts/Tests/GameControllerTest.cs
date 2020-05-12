using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GameControllerTest
    {
        
        // A Test behaves as an ordinary method
        [Test]
        public void IntiallizationOfGameController()
        {
            var gameController = new GameObject().AddComponent<GameController>();
            Assert.NotNull(gameController);
        }

        [Test]
        public void TestMovePlayer()
        {
            GameObject player = new GameObject();
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            player.AddComponent<Player>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            gameController.GetComponent<GameController>().MovePlayer(5);
            Assert.NotNull(player.GetComponent<Player>());
            Assert.AreEqual(true, gameController.GetComponent<GameController>().GetTurnInProgress());
            Assert.AreEqual(5, gameController.GetComponent<GameController>().GetPlayers().Peek().getTargetPos());


        }

        [Test]
        public void TestTurnProgressStartingValue()
        {
            GameObject player = new GameObject();
            GameObject player2 = new GameObject();
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            player.AddComponent<Player>();
            player2.AddComponent<Player>();
            Player[] temp = new Player[2];
            temp[0] = player.GetComponent<Player>();
            temp[1] = player2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(temp);
            Assert.AreEqual(false, gameController.GetComponent<GameController>().GetTurnInProgress());
            
        }

        [Test]
        public void TestSendPlayerToJail()
        {
            GameObject player = new GameObject();
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            player.AddComponent<Player>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            gameController.GetComponent<GameController>().SendPlayerToJail();
            Assert.AreEqual(false, gameController.GetComponent<GameController>().GetTurnInProgress());
            Assert.AreEqual(true, player.GetComponent<Player>().IsInJail());

        }

        [Test]
        public void TestGoToJail()
        {
            GameObject player = new GameObject();
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            player.AddComponent<Player>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            gameController.GetComponent<GameController>().GoToJail();
            Assert.AreEqual(false, gameController.GetComponent<GameController>().GetTurnInProgress());
            Assert.AreEqual(true, player.GetComponent<Player>().IsInJail());
        }

        [Test]
        public void TestPayFineToGetOutOfJail()
        {
            GameObject player = new GameObject();
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            player.AddComponent<Player>();
            player.GetComponent<Player>().SetBalance(50);
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            gameController.GetComponent<GameController>().GoToJail();
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            gameController.GetComponent<GameController>().SetBankController(bankController.GetComponent<BankController>());
            gameController.GetComponent<GameController>().PayToLeaveJail();
            Assert.AreEqual(0, player.GetComponent<Player>().GetBalance());
            Assert.AreEqual(false, player.GetComponent<Player>().IsInJail());
        }

    }
}
