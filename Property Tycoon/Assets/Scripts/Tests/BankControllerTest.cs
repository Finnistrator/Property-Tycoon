using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class BankControllerTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void BankControllerNotNull()
        {
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            Assert.NotNull(bankController);
            Assert.NotNull(bankController.GetComponent<BankController>());
        }

        [Test]
        public void AddBalanceTest()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(100);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            bankController.GetComponent<BankController>().AddBalance(playerObj.GetComponent<Player>(), 100);
            Assert.AreEqual(200, playerObj.GetComponent<Player>().GetBalance());
        }

        [Test]
        public void RemoveBalanceSuccessTest()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(100);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            Assert.AreEqual(true,bankController.GetComponent<BankController>().SubtractBalance(playerObj.GetComponent<Player>(), 100, false));
            Assert.AreEqual(0, playerObj.GetComponent<Player>().GetBalance());
        }

        [Test]
        public void RemoveBalanceFailTest()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(0);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            Assert.AreEqual(false, bankController.GetComponent<BankController>().SubtractBalance(playerObj.GetComponent<Player>(), 100, false));
            Assert.AreEqual(0, playerObj.GetComponent<Player>().GetBalance());
        }

        [Test]
        public void PayRentTest()
        {
            GameObject playerObj1 = new GameObject();
            playerObj1.AddComponent<Player>();
            playerObj1.GetComponent<Player>().SetBalance(1000);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1000);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test";



            propertyClass.SetRent0Houses(5);
            //propertyClass.Rent1House = 0;
            //propertyClass.Rent2House = 0;
            //propertyClass.Rent3House = 0;
            //propertyClass.Rent4House = 0;
            //propertyClass.RentHotel = 0;



            var propertyClass2 = ScriptableObject.CreateInstance<PurchaseableProperty>(); ;
            GameObject property = new GameObject();
            property.AddComponent<PropertyController>().SetProperty(propertyClass);
            Assert.NotNull(propertyClass);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj1.GetComponent<Player>();
            players[1] = playerObj2.GetComponent<Player>();
            playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            playerObj2.GetComponent<Player>().AddProperty(propertyClass2);
            gameContoller.GetComponent<GameController>().addMutiplePlayer(players);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());

            
            bankController.GetComponent<BankController>().PayRent(playerObj2.GetComponent<Player>(), propertyClass);
            Assert.AreEqual(1005, playerObj1.GetComponent<Player>().GetBalance());
            Assert.AreEqual(995, playerObj2.GetComponent<Player>().GetBalance());
        }

        [Test]
        public void TestHasEnoughBalance()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(0);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            Assert.AreEqual(false, bankController.GetComponent<BankController>().HasEnoughBalance(playerObj.GetComponent<Player>(), 100));
            Assert.AreEqual(0, playerObj.GetComponent<Player>().GetBalance());
        }

        [Test]
        public void TestPurchaseProperty()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(1000);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test";
            propertyClass.SetCost(100);
            List<PurchaseableProperty> listOfProperties = new List<PurchaseableProperty>();
            
            GameObject property = new GameObject();
            property.AddComponent<PropertyController>().SetProperty(propertyClass);
            listOfProperties.Add(propertyClass);
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();   
            gameContoller.GetComponent<GameController>().addPlayer(playerObj.GetComponent<Player>());
            gameContoller.GetComponent<GameController>().SetCurrentProperty(propertyClass);
            bankController.GetComponent<BankController>().SetUnsoldProperties(listOfProperties);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());
            Assert.AreEqual(1, bankController.GetComponent<BankController>().GetUnsoldProperties().Count);
            bankController.GetComponent<BankController>().CurrentPlayerPurchaseProperty();
            Assert.AreEqual(0, bankController.GetComponent<BankController>().GetUnsoldProperties().Count);
            Assert.AreEqual(900, playerObj.GetComponent<Player>().GetBalance());
        }

        [Test]
        public void TestDoesPlayerOwnAllSameColourTrue()
        {
            GameObject playerObj1 = new GameObject();
            playerObj1.AddComponent<Player>();
            playerObj1.GetComponent<Player>().SetBalance(1000);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1000);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);



            propertyClass.SetRent0Houses(5);
            propertyClass.SetRent1House(10);
            //propertyClass.Rent2House = 0;
            //propertyClass.Rent3House = 0;
            //propertyClass.Rent4House = 0;
            //propertyClass.RentHotel = 0;

            var propertyClass2 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass2.name = "Test2";
            propertyClass2.SetGroup(Group.Blue);

            var propertyClass3 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass3.name = "Test2";
            propertyClass3.SetGroup(Group.Blue);

            Assert.NotNull(propertyClass);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj1.GetComponent<Player>();
            playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass2);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass3);
            gameContoller.GetComponent<GameController>().addMutiplePlayer(players);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());

            Assert.AreEqual(true, bankController.GetComponent<BankController>().DoesPlayerOwnAllSameColour(propertyClass2, playerObj1.GetComponent<Player>()));
        }

        [Test]
        public void TestDoesPlayerOwnAllSameColourFalse()
        {
            GameObject playerObj1 = new GameObject();
            playerObj1.AddComponent<Player>();
            playerObj1.GetComponent<Player>().SetBalance(1000);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1000);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);



            propertyClass.SetRent0Houses(5);
            propertyClass.SetRent1House(10);
            //propertyClass.Rent2House = 0;
            //propertyClass.Rent3House = 0;
            //propertyClass.Rent4House = 0;
            //propertyClass.RentHotel = 0;

            var propertyClass2 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass2.name = "Test2";
            propertyClass2.SetGroup(Group.Blue);

            var propertyClass3 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass3.name = "Test2";
            propertyClass3.SetGroup(Group.Blue);

            Assert.NotNull(propertyClass);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj1.GetComponent<Player>();
            playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass2);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass3);
            gameContoller.GetComponent<GameController>().addMutiplePlayer(players);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());

            Assert.AreEqual(false, bankController.GetComponent<BankController>().DoesPlayerOwnAllSameColour(propertyClass2, playerObj1.GetComponent<Player>()));
        }

        [Test]
        public void TestPurchaseHouse()
        {
            GameObject playerObj1 = new GameObject();
            playerObj1.AddComponent<Player>();
            playerObj1.GetComponent<Player>().SetBalance(1000);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1000);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);



            propertyClass.SetRent0Houses(5);
            propertyClass.SetRent1House(10);
            //propertyClass.Rent2House = 0;
            //propertyClass.Rent3House = 0;
            //propertyClass.Rent4House = 0;
            //propertyClass.RentHotel = 0;

            var propertyClass2 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass2.name = "Test2";
            propertyClass2.SetGroup(Group.Blue);

            var propertyClass3 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass3.name = "Test2";
            propertyClass3.SetGroup(Group.Blue);

            Assert.NotNull(propertyClass);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            GameObject propertyController = new GameObject();
            propertyController.AddComponent<PropertyController>();
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();
            PropertyController[] properties = new PropertyController[1];
            properties[0] = propertyController.GetComponent<PropertyController>();
            gameContoller.GetComponent<GameController>().SetProperties(properties);
            Player[] players = new Player[2];
            players[0] = playerObj1.GetComponent<Player>();
            playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass2);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass3);
            gameContoller.GetComponent<GameController>().addMutiplePlayer(players);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());

            Assert.AreEqual(true, bankController.GetComponent<BankController>().DoesPlayerOwnAllSameColour(propertyClass2, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(1, propertyClass.GetHouses());
        }

        [Test]
        public void TestPurchaseHotel()
        {
            GameObject playerObj1 = new GameObject();
            playerObj1.AddComponent<Player>();
            playerObj1.GetComponent<Player>().SetBalance(1000);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1000);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);



            propertyClass.SetRent0Houses(5);
            propertyClass.SetRent1House(10);
            //propertyClass.Rent2House = 0;
            //propertyClass.Rent3House = 0;
            //propertyClass.Rent4House = 0;
            //propertyClass.RentHotel = 0;

            var propertyClass2 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass2.name = "Test2";
            propertyClass2.SetGroup(Group.Blue);

            var propertyClass3 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass3.name = "Test2";
            propertyClass3.SetGroup(Group.Blue);

            Assert.NotNull(propertyClass);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            GameObject propertyController = new GameObject();
            propertyController.AddComponent<PropertyController>();
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();
            PropertyController[] properties = new PropertyController[1];
            properties[0] = propertyController.GetComponent<PropertyController>();
            gameContoller.GetComponent<GameController>().SetProperties(properties);
            Player[] players = new Player[2];
            players[0] = playerObj1.GetComponent<Player>();
            playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass2);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass3);
            gameContoller.GetComponent<GameController>().addMutiplePlayer(players);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());

            Assert.AreEqual(true, bankController.GetComponent<BankController>().DoesPlayerOwnAllSameColour(propertyClass2, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(5, propertyClass.GetHouses());
        }

        [Test]
        public void TestGetRentForProperty()
        {
            GameObject playerObj1 = new GameObject();
            playerObj1.AddComponent<Player>();
            playerObj1.GetComponent<Player>().SetBalance(1000);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1000);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);

            GameObject importController = new GameObject();
            importController.AddComponent<ImportController>();

            propertyClass.SetRent0Houses(5);
            propertyClass.SetRent1House(10);
            //propertyClass.Rent2House = 0;
            //propertyClass.Rent3House = 0;
            //propertyClass.Rent4House = 0;
            //propertyClass.RentHotel = 0;

            var propertyClass2 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass2.name = "Test2";
            propertyClass2.SetGroup(Group.Blue);

            var propertyClass3 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass3.name = "Test2";
            propertyClass3.SetGroup(Group.Blue);

            Assert.NotNull(propertyClass);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            GameObject propertyController = new GameObject();
            propertyController.AddComponent<PropertyController>();
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();
            PropertyController[] properties = new PropertyController[1];
            properties[0] = propertyController.GetComponent<PropertyController>();
            gameContoller.GetComponent<GameController>().SetProperties(properties);
            Player[] players = new Player[2];
            players[0] = playerObj1.GetComponent<Player>();
            playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass2);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass3);
            gameContoller.GetComponent<GameController>().addMutiplePlayer(players);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());

            Assert.AreEqual(true, bankController.GetComponent<BankController>().DoesPlayerOwnAllSameColour(propertyClass2, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(10, bankController.GetComponent<BankController>().GetRent(propertyClass, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(5, propertyClass.GetHouses());
        }

        [Test]
        public void TestGetRentForPropertyWithHotel()
        {
            GameObject playerObj1 = new GameObject();
            playerObj1.AddComponent<Player>();
            playerObj1.GetComponent<Player>().SetBalance(1000);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1000);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);



            propertyClass.SetRent0Houses(5);
            propertyClass.SetRent1House(10);
            propertyClass.SetRentHotel(100);
            //propertyClass.Rent2House = 0;
            //propertyClass.Rent3House = 0;
            //propertyClass.Rent4House = 0;
            //propertyClass.RentHotel = 0;

            var propertyClass2 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass2.name = "Test2";
            propertyClass2.SetGroup(Group.Blue);

            var propertyClass3 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass3.name = "Test2";
            propertyClass3.SetGroup(Group.Blue);

            Assert.NotNull(propertyClass);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            GameObject propertyController = new GameObject();
            propertyController.AddComponent<PropertyController>();
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();
            PropertyController[] properties = new PropertyController[1];
            properties[0] = propertyController.GetComponent<PropertyController>();
            gameContoller.GetComponent<GameController>().SetProperties(properties);
            Player[] players = new Player[2];
            players[0] = playerObj1.GetComponent<Player>();
            playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass2);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass3);
            gameContoller.GetComponent<GameController>().addMutiplePlayer(players);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());

            Assert.AreEqual(true, bankController.GetComponent<BankController>().DoesPlayerOwnAllSameColour(propertyClass2, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(10, bankController.GetComponent<BankController>().GetRent(propertyClass, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(true, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(100, bankController.GetComponent<BankController>().GetRent(propertyClass, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(5, propertyClass.GetHouses());
        }

        [Test]
        public void TestGetRentForStation()
        {
            GameObject playerObj1 = new GameObject();
            playerObj1.AddComponent<Player>();
            playerObj1.GetComponent<Player>().SetBalance(1000);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1000);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Station);

            var propertyClass2 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass2.name = "Test2";
            propertyClass2.SetGroup(Group.Blue);

            var propertyClass3 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass3.name = "Test2";
            propertyClass3.SetGroup(Group.Blue);

            Assert.NotNull(propertyClass);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            GameObject propertyController = new GameObject();
            propertyController.AddComponent<PropertyController>();
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();
            PropertyController[] properties = new PropertyController[1];
            properties[0] = propertyController.GetComponent<PropertyController>();
            gameContoller.GetComponent<GameController>().SetProperties(properties);
            Player[] players = new Player[2];
            players[0] = playerObj1.GetComponent<Player>();
            playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass2);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass3);
            gameContoller.GetComponent<GameController>().addMutiplePlayer(players);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());

            Assert.AreEqual(false, bankController.GetComponent<BankController>().DoesPlayerOwnAllSameColour(propertyClass2, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(25, bankController.GetComponent<BankController>().GetRent(propertyClass, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(0, propertyClass.GetHouses());
        }

        [Test]
        public void TestGetRentForMultpleStations()
        {
            GameObject playerObj1 = new GameObject();
            playerObj1.AddComponent<Player>();
            playerObj1.GetComponent<Player>().SetBalance(1000);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1000);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Station);

            var propertyClass2 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass2.name = "Test2";
            propertyClass2.SetGroup(Group.Station);

            var propertyClass3 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass3.name = "Test2";
            propertyClass3.SetGroup(Group.Blue);

            Assert.NotNull(propertyClass);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            GameObject propertyController = new GameObject();
            propertyController.AddComponent<PropertyController>();
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();
            PropertyController[] properties = new PropertyController[1];
            properties[0] = propertyController.GetComponent<PropertyController>();
            gameContoller.GetComponent<GameController>().SetProperties(properties);
            Player[] players = new Player[2];
            players[0] = playerObj1.GetComponent<Player>();
            playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass2);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass3);
            gameContoller.GetComponent<GameController>().addMutiplePlayer(players);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());

            Assert.AreEqual(false, bankController.GetComponent<BankController>().DoesPlayerOwnAllSameColour(propertyClass2, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(50, bankController.GetComponent<BankController>().GetRent(propertyClass, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(0, propertyClass.GetHouses());
        }

        [Test]
        public void TestGetRentForutilities()
        {
            GameObject playerObj1 = new GameObject();
            playerObj1.AddComponent<Player>();
            playerObj1.GetComponent<Player>().SetBalance(1000);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1000);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Utilities);

            var propertyClass2 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass2.name = "Test2";
            propertyClass2.SetGroup(Group.Station);

            var propertyClass3 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass3.name = "Test2";
            propertyClass3.SetGroup(Group.Blue);

            Assert.NotNull(propertyClass);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            GameObject propertyController = new GameObject();
            propertyController.AddComponent<PropertyController>();
            GameObject diceController = new GameObject();
            diceController.AddComponent<DiceController>();
            diceController.GetComponent<DiceController>().SetRolledNumber(6);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();
            PropertyController[] properties = new PropertyController[1];
            properties[0] = propertyController.GetComponent<PropertyController>();
            gameContoller.GetComponent<GameController>().SetProperties(properties);
            Player[] players = new Player[2];
            players[0] = playerObj1.GetComponent<Player>();
            playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass2);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass3);
            gameContoller.GetComponent<GameController>().addMutiplePlayer(players);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());
            bankController.GetComponent<BankController>().SetDiceController(diceController.GetComponent<DiceController>());
            Assert.AreEqual(false, bankController.GetComponent<BankController>().DoesPlayerOwnAllSameColour(propertyClass2, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(24, bankController.GetComponent<BankController>().GetRent(propertyClass, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(0, propertyClass.GetHouses());
        }

        [Test]
        public void TestGetRentForPropertyWithNohouseFullColourSet()
        {
            GameObject playerObj1 = new GameObject();
            playerObj1.AddComponent<Player>();
            playerObj1.GetComponent<Player>().SetBalance(1000);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1000);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);



            propertyClass.SetRent0Houses(5);
            propertyClass.SetRent1House(10);
            propertyClass.SetRentHotel(100);
            //propertyClass.Rent2House = 0;
            //propertyClass.Rent3House = 0;
            //propertyClass.Rent4House = 0;
            //propertyClass.RentHotel = 0;

            var propertyClass2 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass2.name = "Test2";
            propertyClass2.SetGroup(Group.Blue);

            var propertyClass3 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass3.name = "Test2";
            propertyClass3.SetGroup(Group.Blue);

            Assert.NotNull(propertyClass);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            GameObject propertyController = new GameObject();
            propertyController.AddComponent<PropertyController>();
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();
            PropertyController[] properties = new PropertyController[1];
            properties[0] = propertyController.GetComponent<PropertyController>();
            gameContoller.GetComponent<GameController>().SetProperties(properties);
            Player[] players = new Player[2];
            players[0] = playerObj1.GetComponent<Player>();
            playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass2);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass3);
            gameContoller.GetComponent<GameController>().addMutiplePlayer(players);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());

            Assert.AreEqual(true, bankController.GetComponent<BankController>().DoesPlayerOwnAllSameColour(propertyClass2, playerObj1.GetComponent<Player>()));
            Assert.AreEqual(10, bankController.GetComponent<BankController>().GetRent(propertyClass, playerObj1.GetComponent<Player>()));
        }

        [Test]
        public void TestStartAuction()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj.GetComponent<Player>();
            players[1] = playerObj2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(players);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);
            gameController.GetComponent<GameController>().SetCurrentProperty(propertyClass);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            bankController.GetComponent<BankController>().SetGameController(gameController.GetComponent<GameController>());
            bankController.GetComponent<BankController>().StartAuction();
            Assert.AreEqual(true, bankController.GetComponent<BankController>().GetIsAuctioning());
        }

        [Test]
        public void TestBidInAuction()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(1500);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1500);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj.GetComponent<Player>();
            players[1] = playerObj2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(players);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);
            propertyClass.SetCost(100);
            gameController.GetComponent<GameController>().SetCurrentProperty(propertyClass);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            bankController.GetComponent<BankController>().SetGameController(gameController.GetComponent<GameController>());
            bankController.GetComponent<BankController>().StartAuction();
            Assert.AreEqual(true, bankController.GetComponent<BankController>().GetIsAuctioning());
            bankController.GetComponent<BankController>().BidInAuction();
            Assert.AreEqual(10, bankController.GetComponent<BankController>().GetCurrentBid());
        }

        [Test]
        public void TestNextPlayerInAuction()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(1500);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1500);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj.GetComponent<Player>();
            players[1] = playerObj2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(players);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);
            propertyClass.SetCost(100);
            gameController.GetComponent<GameController>().SetCurrentProperty(propertyClass);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            bankController.GetComponent<BankController>().SetGameController(gameController.GetComponent<GameController>());
            bankController.GetComponent<BankController>().StartAuction();
            Assert.AreEqual(true, bankController.GetComponent<BankController>().GetIsAuctioning());
            bankController.GetComponent<BankController>().BidInAuction();
            Assert.AreEqual(10, bankController.GetComponent<BankController>().GetCurrentBid());
            Assert.AreEqual(playerObj2.GetComponent<Player>(), bankController.GetComponent<BankController>().GetCurrentAuctioningPlayer());
        }

        [Test]
        public void TestRemovePlayerFromAuction()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(1500);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1500);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj.GetComponent<Player>();
            players[1] = playerObj2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(players);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);
            propertyClass.SetCost(100);
            gameController.GetComponent<GameController>().SetCurrentProperty(propertyClass);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            bankController.GetComponent<BankController>().SetGameController(gameController.GetComponent<GameController>());
            bankController.GetComponent<BankController>().StartAuction();
            Assert.AreEqual(true, bankController.GetComponent<BankController>().GetIsAuctioning());
            bankController.GetComponent<BankController>().RemovePlayerFromAuction();
            Assert.AreEqual(false, bankController.GetComponent<BankController>().IsPlayerStillInAuction(playerObj.GetComponent<Player>()));
        }

        [Test]
        public void TestGetNextBidAmount()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(1500);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1500);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj.GetComponent<Player>();
            players[1] = playerObj2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(players);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);
            propertyClass.SetCost(100);
            gameController.GetComponent<GameController>().SetCurrentProperty(propertyClass);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            bankController.GetComponent<BankController>().SetGameController(gameController.GetComponent<GameController>());
            bankController.GetComponent<BankController>().StartAuction();
            Assert.AreEqual(true, bankController.GetComponent<BankController>().GetIsAuctioning());
            Assert.AreEqual(10, bankController.GetComponent<BankController>().GetNextBidAmount(80));
        }

        public void TestIsPlayerInuction()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(1500);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1500);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj.GetComponent<Player>();
            players[1] = playerObj2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(players);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);
            propertyClass.SetCost(100);
            gameController.GetComponent<GameController>().SetCurrentProperty(propertyClass);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            bankController.GetComponent<BankController>().SetGameController(gameController.GetComponent<GameController>());
            bankController.GetComponent<BankController>().StartAuction();
            Assert.AreEqual(true, bankController.GetComponent<BankController>().GetIsAuctioning());
            Assert.AreEqual(true, bankController.GetComponent<BankController>().IsPlayerStillInAuction(playerObj.GetComponent<Player>()));
        }

        [Test]
        public void TestEmptyFreeParking()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(1500);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1500);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj.GetComponent<Player>();
            players[1] = playerObj2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(players);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);
            propertyClass.SetCost(100);
            gameController.GetComponent<GameController>().SetCurrentProperty(propertyClass);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            bankController.GetComponent<BankController>().SetGameController(gameController.GetComponent<GameController>());
            Assert.AreEqual(0, bankController.GetComponent<BankController>().EmptyFreeParking());
            bankController.GetComponent<BankController>().SetFreeParking(100);
            Assert.AreEqual(100, bankController.GetComponent<BankController>().EmptyFreeParking());
        }

        [Test]
        public void AddFreeParking()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(1500);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj.GetComponent<Player>();
            players[1] = playerObj2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(players);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Blue);
            propertyClass.SetCost(100);
            gameController.GetComponent<GameController>().SetCurrentProperty(propertyClass);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            bankController.GetComponent<BankController>().SetGameController(gameController.GetComponent<GameController>());
            bankController.GetComponent<BankController>().AddFreeParking(playerObj.GetComponent<Player>(),100);
            Assert.AreEqual(100, bankController.GetComponent<BankController>().EmptyFreeParking());
            bankController.GetComponent<BankController>().SetFreeParking(10);
        }

        [Test]
        public void PlayerGoBankrupt()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(1500);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1500);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj.GetComponent<Player>();
            players[1] = playerObj2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(players);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            bankController.GetComponent<BankController>().SetGameController(gameController.GetComponent<GameController>());
            bankController.GetComponent<BankController>().GoBankrupt(playerObj.GetComponent<Player>());
            Assert.AreEqual(1, gameController.GetComponent<GameController>().GetPlayers().Count);
            Assert.AreEqual(true, playerObj.GetComponent<Player>().GetIsBankrupt());
        }

        [Test]
        public void PlayerCantPayMoneyGoBankrupt()
        {
            GameObject playerObj = new GameObject();
            playerObj.AddComponent<Player>();
            playerObj.GetComponent<Player>().SetBalance(1500);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1500);
            GameObject gameController = new GameObject();
            gameController.AddComponent<GameController>();
            Player[] players = new Player[2];
            players[0] = playerObj.GetComponent<Player>();
            players[1] = playerObj2.GetComponent<Player>();
            gameController.GetComponent<GameController>().addMutiplePlayer(players);
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            bankController.GetComponent<BankController>().SetGameController(gameController.GetComponent<GameController>());
            bankController.GetComponent<BankController>().SetPlayerBalanceDue(playerObj.GetComponent<Player>());
            bankController.GetComponent<BankController>().CantPayMoney();
            Assert.AreEqual(1, gameController.GetComponent<GameController>().GetPlayers().Count);
            Assert.AreEqual(true, playerObj.GetComponent<Player>().GetIsBankrupt());
        }
        [Test]
        public void CantPurchaseHouse()
        {
            GameObject playerObj1 = new GameObject();
            playerObj1.AddComponent<Player>();
            playerObj1.GetComponent<Player>().SetBalance(1000);
            GameObject playerObj2 = new GameObject();
            playerObj2.AddComponent<Player>();
            playerObj2.GetComponent<Player>().SetBalance(1000);
            var propertyClass = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass.name = "Test1";
            propertyClass.SetGroup(Group.Station);



            propertyClass.SetRent0Houses(5);
            propertyClass.SetRent1House(10);
            //propertyClass.Rent2House = 0;
            //propertyClass.Rent3House = 0;
            //propertyClass.Rent4House = 0;
            //propertyClass.RentHotel = 0;

            var propertyClass2 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass2.name = "Test2";
            propertyClass2.SetGroup(Group.Station);

            var propertyClass3 = ScriptableObject.CreateInstance<PurchaseableProperty>();
            propertyClass3.name = "Test2";
            propertyClass3.SetGroup(Group.Station);

            Assert.NotNull(propertyClass);
            //playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            GameObject propertyController = new GameObject();
            propertyController.AddComponent<PropertyController>();
            GameObject bankController = new GameObject();
            bankController.AddComponent<BankController>();
            GameObject gameContoller = new GameObject();
            gameContoller.AddComponent<GameController>();
            PropertyController[] properties = new PropertyController[1];
            properties[0] = propertyController.GetComponent<PropertyController>();
            gameContoller.GetComponent<GameController>().SetProperties(properties);
            Player[] players = new Player[2];
            players[0] = playerObj1.GetComponent<Player>();
            playerObj1.GetComponent<Player>().AddProperty(propertyClass);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass2);
            playerObj1.GetComponent<Player>().AddProperty(propertyClass3);
            gameContoller.GetComponent<GameController>().addMutiplePlayer(players);
            bankController.GetComponent<BankController>().SetGameController(gameContoller.GetComponent<GameController>());

            Assert.AreEqual(false, bankController.GetComponent<BankController>().PurchaseHouse(propertyClass));
            Assert.AreEqual(0, propertyClass.GetHouses());
        }

    }

}
