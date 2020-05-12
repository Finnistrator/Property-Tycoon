using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PropertyDisplayTestScript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void TestMortgageProperty()
        {
            GameObject player = new GameObject();
            player.AddComponent<Player>();
            player.GetComponent<Player>().SetBalance(1000);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.SetCost(100);
            GameObject propertyDisplay = new GameObject();
            propertyDisplay.AddComponent<PropertyDisplay>();
            propertyDisplay.GetComponent<PropertyDisplay>().SetBankController(bankController.GetComponent<BankController>());
            propertyDisplay.GetComponent<PropertyDisplay>().SetGameController(gameController.GetComponent<GameController>());
            propertyDisplay.GetComponent<PropertyDisplay>().SetProperty(propertyClass);
            propertyDisplay.GetComponent<PropertyDisplay>().SetOwner(player.GetComponent<Player>());
            propertyDisplay.GetComponent<PropertyDisplay>().MortgageProperty();

            Assert.AreEqual(true, propertyClass.IsMortgaged());
            Assert.AreEqual(1050, player.GetComponent<Player>().GetBalance());

        }

        [Test]
        public void TestMortgagePropertyFail()
        {
            GameObject player = new GameObject();
            player.AddComponent<Player>();
            player.GetComponent<Player>().SetBalance(1000);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.SetCost(100);
            propertyClass.AddHouse();
            GameObject propertyDisplay = new GameObject();
            propertyDisplay.AddComponent<PropertyDisplay>();
            propertyDisplay.GetComponent<PropertyDisplay>().SetBankController(bankController.GetComponent<BankController>());
            propertyDisplay.GetComponent<PropertyDisplay>().SetGameController(gameController.GetComponent<GameController>());
            propertyDisplay.GetComponent<PropertyDisplay>().SetProperty(propertyClass);
            propertyDisplay.GetComponent<PropertyDisplay>().MortgageProperty();
            Assert.AreEqual(false, propertyClass.IsMortgaged());
            Assert.AreEqual(1000, player.GetComponent<Player>().GetBalance());

        }

        [Test]
        public void TestSellProperty()
        {
            GameObject player = new GameObject();
            player.AddComponent<Player>();
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.SetCost(100);
            propertyClass.SetGroup(Group.Brown);
            propertyClass.SetMortgaged(false);

            player.GetComponent<Player>().SetBalance(1000);
            player.GetComponent<Player>().AddProperty(propertyClass);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject propertyDisplay = new GameObject();
            propertyDisplay.AddComponent<PropertyDisplay>();
            propertyDisplay.GetComponent<PropertyDisplay>().SetBankController(bankController.GetComponent<BankController>());
            propertyDisplay.GetComponent<PropertyDisplay>().SetGameController(gameController.GetComponent<GameController>());
            propertyDisplay.GetComponent<PropertyDisplay>().SetProperty(propertyClass);
            propertyDisplay.GetComponent<PropertyDisplay>().SetOwner(player.GetComponent<Player>());
            propertyDisplay.GetComponent<PropertyDisplay>().SellProperty();
            Assert.AreEqual(1100, player.GetComponent<Player>().GetBalance());
            Assert.AreEqual(0, player.GetComponent<Player>().GetOwnedProperties().Count);

        }

        [Test]
        public void TestSellPropertyWithHouse()
        {
            GameObject player = new GameObject();
            player.AddComponent<Player>();
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.SetCost(100);
            propertyClass.SetGroup(Group.Brown);
            propertyClass.SetMortgaged(false);
            propertyClass.AddHouse();
            PropertyController[] listOfProp = new PropertyController[1];
            GameObject PropCon = new GameObject();
            PropCon.AddComponent<PropertyController>();
            PropCon.GetComponent<PropertyController>().SetProperty(propertyClass);
            listOfProp[0] = PropCon.GetComponent<PropertyController>();
            player.GetComponent<Player>().SetBalance(1000);
            player.GetComponent<Player>().AddProperty(propertyClass);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            gameController.GetComponent<GameController>().SetProperties(listOfProp);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject propertyDisplay = new GameObject();
            propertyDisplay.AddComponent<PropertyDisplay>();
            bankController.GetComponent<BankController>().SetGameController(gameController.GetComponent<GameController>());
            propertyDisplay.GetComponent<PropertyDisplay>().SetBankController(bankController.GetComponent<BankController>());
            // propertyDisplay.GetComponent<PropertyDisplay>().SetGameController(gameController.GetComponent<GameController>());
            propertyDisplay.GetComponent<PropertyDisplay>().SetProperty(propertyClass);
            propertyDisplay.GetComponent<PropertyDisplay>().SetOwner(player.GetComponent<Player>());
            propertyDisplay.GetComponent<PropertyDisplay>().SellProperty();
            Assert.AreEqual(1110, player.GetComponent<Player>().GetBalance());
            Assert.AreEqual(1, player.GetComponent<Player>().GetOwnedProperties().Count);

        }

        [Test]
        public void TestSellPropertyMortgage()
        {
            GameObject player = new GameObject();
            player.AddComponent<Player>();
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.SetCost(100);
            propertyClass.SetGroup(Group.Brown);
            propertyClass.SetMortgaged(true);

            player.GetComponent<Player>().SetBalance(1000);
            player.GetComponent<Player>().AddProperty(propertyClass);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject propertyDisplay = new GameObject();
            Assert.NotNull(gameController.GetComponent<GameController>().GetCurrentPlayer());
            propertyDisplay.AddComponent<PropertyDisplay>();
            propertyDisplay.GetComponent<PropertyDisplay>().SetBankController(bankController.GetComponent<BankController>());
            propertyDisplay.GetComponent<PropertyDisplay>().SetGameController(gameController.GetComponent<GameController>());
            propertyDisplay.GetComponent<PropertyDisplay>().SetProperty(propertyClass);
            propertyDisplay.GetComponent<PropertyDisplay>().SetOwner(player.GetComponent<Player>());
            propertyDisplay.GetComponent<PropertyDisplay>().SellProperty();
            Assert.AreEqual(1050, player.GetComponent<Player>().GetBalance());
            Assert.AreEqual(0, player.GetComponent<Player>().GetOwnedProperties().Count);

        }

    }
}
