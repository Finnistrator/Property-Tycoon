using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DiceClassTest
    {

        // A Test behaves as an ordinary method
        [Test]
        public void DiceGameObjectCreated()
        {
            var dice = new GameObject().AddComponent<Dice>();
            Assert.NotNull(dice);
        }

        [Test]
        public void TestRollDice()
        {
            
            
            var gameController = new GameObject();
            var audioController = new GameObject();
            var diceController = new GameObject();
            var player = new GameObject();
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<BoxCollider>();
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<Dice>();
            audioController.AddComponent<AudioController>();
            player.AddComponent<Player>();
            gameController.AddComponent<GameController>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            Assert.NotNull(gameObject.GetComponent<Dice>());
            diceController.AddComponent<DiceController>();
            diceController.GetComponent<DiceController>().SetGameController(gameController.GetComponent<GameController>());
            diceController.GetComponent<DiceController>().SetAudioController(audioController.GetComponent<AudioController>());
            diceController.GetComponent<DiceController>().RollDice();
            Assert.AreEqual(true, diceController.GetComponent<DiceController>().GetRolled());
        }


        [Test]
        public void TestSetNumberRoll()
        {
            var gameController = new GameObject();
            var audioController = new GameObject();
            var diceController = new GameObject();
            var player = new GameObject();
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<BoxCollider>();
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<Dice>();
            audioController.AddComponent<AudioController>();
            player.AddComponent<Player>();
            gameController.AddComponent<GameController>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            Assert.NotNull(gameObject.GetComponent<Dice>());
            diceController.AddComponent<DiceController>();
            diceController.GetComponent<DiceController>().SetGameController(gameController.GetComponent<GameController>());
            diceController.GetComponent<DiceController>().SetAudioController(audioController.GetComponent<AudioController>());
            diceController.GetComponent<DiceController>().SetRolledNumber(6);
            Assert.AreEqual(6, diceController.GetComponent<DiceController>().GetRolledNumber());
        }

        [Test]
        public void TestSetNumberRollFail()
        {
            var gameController = new GameObject();
            var audioController = new GameObject();
            var diceController = new GameObject();
            var player = new GameObject();
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<BoxCollider>();
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<Dice>();
            audioController.AddComponent<AudioController>();
            player.AddComponent<Player>();
            gameController.AddComponent<GameController>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            Assert.NotNull(gameObject.GetComponent<Dice>());
            diceController.AddComponent<DiceController>();
            diceController.GetComponent<DiceController>().SetGameController(gameController.GetComponent<GameController>());
            diceController.GetComponent<DiceController>().SetAudioController(audioController.GetComponent<AudioController>());
            diceController.GetComponent<DiceController>().SetRolledNumber(20);
            Assert.AreEqual(0, diceController.GetComponent<DiceController>().GetRolledNumber());
        }

        [Test]
        public void TestAIRolled()
        {
            var gameController = new GameObject();
            var audioController = new GameObject();
            var diceController = new GameObject();
            var player = new GameObject();
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<BoxCollider>();
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<Dice>();
            audioController.AddComponent<AudioController>();
            player.AddComponent<Player>();
            player.GetComponent<Player>().SetAI(true);
            gameController.AddComponent<GameController>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            Assert.NotNull(gameObject.GetComponent<Dice>());
            diceController.AddComponent<DiceController>();
            diceController.GetComponent<DiceController>().SetGameController(gameController.GetComponent<GameController>());
            diceController.GetComponent<DiceController>().SetAudioController(audioController.GetComponent<AudioController>());
            diceController.GetComponent<DiceController>().RollDice();
            Assert.AreEqual(true, diceController.GetComponent<DiceController>().GetRolled());
            Assert.AreEqual(true, player.GetComponent<Player>().GetAI());
        }

        [Test]
        public void TestRollDouble()
        {
            var gameController = new GameObject();
            var audioController = new GameObject();
            var diceController = new GameObject();
            var player = new GameObject();
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<BoxCollider>();
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<Dice>();
            audioController.AddComponent<AudioController>();
            player.AddComponent<Player>();
            player.GetComponent<Player>().SetAI(true);
            gameController.AddComponent<GameController>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            Assert.NotNull(gameObject.GetComponent<Dice>());
            diceController.AddComponent<DiceController>();
            diceController.GetComponent<DiceController>().SetGameController(gameController.GetComponent<GameController>());
            diceController.GetComponent<DiceController>().SetAudioController(audioController.GetComponent<AudioController>());
            Assert.AreEqual(0, player.GetComponent<Player>().GetAmountOfRolledDoubles());
            diceController.GetComponent<DiceController>().SetRolledNumber(2);
            diceController.GetComponent<DiceController>().SetRolledNumber(2);
            Assert.AreEqual(1, player.GetComponent<Player>().GetAmountOfRolledDoubles());
        }

        [Test]
        public void TestRollDoubleGoToJail()
        {
            var gameController = new GameObject();
            var audioController = new GameObject();
            var diceController = new GameObject();
            var player = new GameObject();
            GameObject gameObject = new GameObject();
            gameObject.AddComponent<MeshFilter>();
            gameObject.AddComponent<BoxCollider>();
            gameObject.AddComponent<Rigidbody>();
            gameObject.AddComponent<Dice>();
            audioController.AddComponent<AudioController>();
            player.AddComponent<Player>();
            gameController.AddComponent<GameController>();
            gameController.GetComponent<GameController>().addPlayer(player.GetComponent<Player>());
            Assert.NotNull(gameObject.GetComponent<Dice>());
            diceController.AddComponent<DiceController>();
            diceController.GetComponent<DiceController>().SetGameController(gameController.GetComponent<GameController>());
            diceController.GetComponent<DiceController>().SetAudioController(audioController.GetComponent<AudioController>());
            player.GetComponent<Player>().SetAmountOfRolledDoubles(2);
            Assert.AreEqual(2, player.GetComponent<Player>().GetAmountOfRolledDoubles());
            diceController.GetComponent<DiceController>().SetRolledNumber(1);
            diceController.GetComponent<DiceController>().SetRolledNumber(1);
            Assert.AreEqual(3, player.GetComponent<Player>().GetAmountOfRolledDoubles());
            Assert.AreEqual(true, player.GetComponent<Player>().IsInJail());
        }
    }
}
