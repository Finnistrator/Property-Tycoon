using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlayerClassEditModeTest
    {
        Player player;

        [Test]
        public void OnePlayerCreated()
        {
            GameObject player1 = new GameObject("Player (1)");
            player1.AddComponent<Player>();
            Assert.NotNull(player1);
        }

        [Test]
        public void MultiplePlayerCreated()
        {
            GameObject player2 = new GameObject("Player (2)");
            player2.AddComponent<Player>();
            GameObject player3 = new GameObject("Player (3)");
            player3.AddComponent<Player>();
            GameObject player4 = new GameObject("Player (4)");
            player4.AddComponent<Player>();
            GameObject player5 = new GameObject("Player (5)");
            player5.AddComponent<Player>();
            GameObject player6 = new GameObject("Player (6)");
            player6.AddComponent<Player>();
            Assert.NotNull("Player (2)");
            Assert.NotNull("Player (3)");
            Assert.NotNull("Player (4)");
            Assert.NotNull("Player (5)");
            Assert.NotNull("Player (6)");
        }

        [Test]
        public void TestIntialPlayerName()
        {
            var gamePlayer = new GameObject("Ben").AddComponent<Player>();
            Assert.AreEqual(null, gamePlayer.GetPlayerName());
        }

        [Test]
        public void TestIntialCurrentPos()
        {
            var gamePlayer = new GameObject("Ben").AddComponent<Player>();
            Assert.AreEqual(0, gamePlayer.GetCurrentPos());
        }

        [Test]
        public void TestMove()
        {
            var gamePlayer = new GameObject();
            gamePlayer.AddComponent<Player>();
            gamePlayer.GetComponent<Player>().Move(10);
            Assert.AreEqual(10, gamePlayer.GetComponent<Player>().getTargetPos());

        }

        [Test]
        public void TestPlayerProperties()
        {
            var gamePlayer = new GameObject();
            gamePlayer.AddComponent<Player>();
            gamePlayer.GetComponent<Player>().AddProperty(new PurchaseableProperty("Test Property", Group.Blue, 50, 2, 10, 100, 200, 300, 500));
            Assert.AreEqual(1, gamePlayer.GetComponent<Player>().GetOwnedProperties().Count);
        }

        [Test]
        public void TestSetBalance()
        {
            var gamePlayer = new GameObject();
            gamePlayer.AddComponent<Player>();
            gamePlayer.GetComponent<Player>().SetBalance(1500);
            Assert.AreEqual(1500, gamePlayer.GetComponent<Player>().GetBalance());
        }

        [Test]
        public void TestTurnInjail()
        {
            var gamePlayer = new GameObject();
            gamePlayer.AddComponent<Player>();
            gamePlayer.GetComponent<Player>().GoToJail();
            Assert.AreEqual(2, gamePlayer.GetComponent<Player>().GetTurnsInJail());
            Assert.AreEqual(true, gamePlayer.GetComponent<Player>().IsInJail());
        }

        [Test]
        public void TestTurnInjailDecrease()
        {
            var gamePlayer = new GameObject();
            gamePlayer.AddComponent<Player>();
            gamePlayer.GetComponent<Player>().GoToJail();
            Assert.AreEqual(2, gamePlayer.GetComponent<Player>().GetTurnsInJail());
            Assert.AreEqual(true, gamePlayer.GetComponent<Player>().IsInJail());
            gamePlayer.GetComponent<Player>().ReduceTurnsInJail();
            Assert.AreEqual(1, gamePlayer.GetComponent<Player>().GetTurnsInJail());
            Assert.AreEqual(true, gamePlayer.GetComponent<Player>().IsInJail());

        }

        [Test]
        public void TestTurnInjailPlayerisFree()
        {
            var gamePlayer = new GameObject();
            gamePlayer.AddComponent<Player>();
            gamePlayer.GetComponent<Player>().GoToJail();
            Assert.AreEqual(2, gamePlayer.GetComponent<Player>().GetTurnsInJail());
            Assert.AreEqual(true, gamePlayer.GetComponent<Player>().IsInJail());
            gamePlayer.GetComponent<Player>().ReduceTurnsInJail();
            Assert.AreEqual(1, gamePlayer.GetComponent<Player>().GetTurnsInJail());
            Assert.AreEqual(true, gamePlayer.GetComponent<Player>().IsInJail());
            gamePlayer.GetComponent<Player>().ReduceTurnsInJail();
            Assert.AreEqual(0, gamePlayer.GetComponent<Player>().GetTurnsInJail());
            Assert.AreEqual(true, gamePlayer.GetComponent<Player>().IsInJail());
            gamePlayer.GetComponent<Player>().ReduceTurnsInJail();
            Assert.AreEqual(0, gamePlayer.GetComponent<Player>().GetTurnsInJail());
            Assert.AreEqual(false, gamePlayer.GetComponent<Player>().IsInJail());
        }

    }
}
