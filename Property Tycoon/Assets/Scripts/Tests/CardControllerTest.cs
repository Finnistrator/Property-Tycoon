using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CardControllerTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void DrawPotLuckCard()
        {
            GameObject cardController = new GameObject();
            cardController.AddComponent<CardController>();
            List<Card> cards = new List<Card>();
            cards.Add(new MoneyCard(100, "You inherit £100"));
            cards.Add(new MoneyCard(20, "You have won 2nd prize in a beauty contest, collect £20"));
            cardController.GetComponent<CardController>().SetPotLuckCards(cards);
            var testCard = cardController.GetComponent<CardController>().GetPotLuckCards()[0];
            var testCard2 = cardController.GetComponent<CardController>().GetPotLuckCards()[1];
            cardController.GetComponent<CardController>().DrawPotLuck();
            Assert.AreEqual(testCard, cardController.GetComponent<CardController>().GetCurrentCard());
            cardController.GetComponent<CardController>().DrawPotLuck();
            Assert.AreEqual(testCard2, cardController.GetComponent<CardController>().GetCurrentCard());
        }

        [Test]
        public void DrawOpportunityKnocksCards()
        {
            GameObject cardController = new GameObject();
            cardController.AddComponent<CardController>();
            List<Card> cards = new List<Card>();
            cards.Add(new MoneyCard(100, "You inherit £100"));
            cards.Add(new MoneyCard(20, "You have won 2nd prize in a beauty contest, collect £20"));
            cardController.GetComponent<CardController>().SetOpportunityKnocksCards(cards);
            var testCard = cardController.GetComponent<CardController>().GetOpportunityKnocksCards()[0];
            var testCard2 = cardController.GetComponent<CardController>().GetOpportunityKnocksCards()[1];
            cardController.GetComponent<CardController>().DrawOpportunityKnocks();
            Assert.AreEqual(testCard, cardController.GetComponent<CardController>().GetCurrentCard());
            cardController.GetComponent<CardController>().DrawOpportunityKnocks();
            Assert.AreEqual(testCard2, cardController.GetComponent<CardController>().GetCurrentCard());
        }

        [Test]
        public void DrawPotLuckCardAndDrawOpportunityKnocksCards()
        {
            GameObject cardController = new GameObject();
            cardController.AddComponent<CardController>();
            List<Card> cards = new List<Card>();
            cards.Add(new MoneyCard(100, "You inherit £100"));
            cards.Add(new MoneyCard(20, "You have won 2nd prize in a beauty contest, collect £20"));
            List<Card> cards2 = new List<Card>();
            cards2.Add(new GoToJailCard("Go to jail. Do not pass GO, do not collect £200"));
            cards2.Add(new MoneyCard(25, "Received interest on shares of £25"));
            cardController.GetComponent<CardController>().SetOpportunityKnocksCards(cards);
            cardController.GetComponent<CardController>().SetPotLuckCards(cards2);
            var testCard = cardController.GetComponent<CardController>().GetOpportunityKnocksCards()[0];
            var testCard2 = cardController.GetComponent<CardController>().GetPotLuckCards()[0];
            cardController.GetComponent<CardController>().DrawOpportunityKnocks();
            Assert.AreEqual(testCard, cardController.GetComponent<CardController>().GetCurrentCard());
            cardController.GetComponent<CardController>().DrawPotLuck();
            Assert.AreEqual(testCard2, cardController.GetComponent<CardController>().GetCurrentCard());
        }

        [Test]
        public void GetJailCardDeckForPotLuck()
        {
            GameObject cardController = new GameObject();
            cardController.AddComponent<CardController>();
            GameObject playerController = new GameObject();
            playerController.AddComponent<Player>();
            cardController.GetComponent<CardController>().SetOutOfJailCardHolder(playerController.GetComponent<Player>(), true);
            Assert.AreEqual("Pot Luck", cardController.GetComponent<CardController>().GetJailCardDeck(playerController.GetComponent<Player>()));

        }

        [Test]
        public void GetJailCardDeckForOK()
        {
            GameObject cardController = new GameObject();
            cardController.AddComponent<CardController>();
            GameObject playerController = new GameObject();
            playerController.AddComponent<Player>();
            cardController.GetComponent<CardController>().SetOutOfJailCardHolder(playerController.GetComponent<Player>(), false);
            Assert.AreEqual("Opportunity Knocks", cardController.GetComponent<CardController>().GetJailCardDeck(playerController.GetComponent<Player>()));

        }

        [Test]
        public void GetJailCardDeckForNone()
        {
            GameObject cardController = new GameObject();
            cardController.AddComponent<CardController>();
            GameObject playerController = new GameObject();
            playerController.AddComponent<Player>();
            Assert.AreEqual("None", cardController.GetComponent<CardController>().GetJailCardDeck(playerController.GetComponent<Player>()));

        }

        [Test]
        public void RemoveGetOutOfJailCardForPL()
        {
            GameObject cardController = new GameObject();
            cardController.AddComponent<CardController>();
            List<Card> cards = new List<Card>();
            cards.Add(new MoneyCard(100, "You inherit £100"));
            cards.Add(new MoneyCard(20, "You have won 2nd prize in a beauty contest, collect £20"));
            Card jailCardPL = new GetOutOfJailFreeCard("Get out of jail free", false);
            cards.Add(jailCardPL);
            cardController.GetComponent<CardController>().SetPotLuckCards(cards);
            cardController.GetComponent<CardController>().SetjailCardPL(jailCardPL);
            Assert.AreEqual(true, cardController.GetComponent<CardController>().GetPotLuckCards().Contains(jailCardPL));
            cardController.GetComponent<CardController>().RemoveGetOutOfJailCard(true);
            Assert.AreEqual(false, cardController.GetComponent<CardController>().GetPotLuckCards().Contains(jailCardPL));

        }

        [Test]
        public void RemoveGetOutOfJailCardForOK()
        {
            GameObject cardController = new GameObject();
            cardController.AddComponent<CardController>();
            List<Card> cards = new List<Card>();
            cards.Add(new MoneyCard(100, "You inherit £100"));
            cards.Add(new MoneyCard(20, "You have won 2nd prize in a beauty contest, collect £20"));
            Card jailCardOK = new GetOutOfJailFreeCard("Get out of jail free", false);
            cards.Add(jailCardOK);
            cardController.GetComponent<CardController>().SetOpportunityKnocksCards(cards);
            cardController.GetComponent<CardController>().SetjailCardOK(jailCardOK);
            Assert.AreEqual(true, cardController.GetComponent<CardController>().GetOpportunityKnocksCards().Contains(jailCardOK));
            cardController.GetComponent<CardController>().RemoveGetOutOfJailCard(false);
            Assert.AreEqual(false, cardController.GetComponent<CardController>().GetOpportunityKnocksCards().Contains(jailCardOK));

        }

        [Test]
        public void CheckingOnlyOneJailCardatMostCanBeAdded()
        {
            GameObject cardController = new GameObject();
            cardController.AddComponent<CardController>();
            List<Card> cards = new List<Card>();
            Card jailCardOK = new GetOutOfJailFreeCard("Get out of jail free", false);
            cards.Add(jailCardOK);
            cardController.GetComponent<CardController>().SetOpportunityKnocksCards(cards);
            cardController.GetComponent<CardController>().SetPotLuckCards(cards);
            cardController.GetComponent<CardController>().SetjailCardPL(jailCardOK);
            cardController.GetComponent<CardController>().SetjailCardOK(jailCardOK);
            cardController.GetComponent<CardController>().AddGetOutOfJailCard(true);
            cardController.GetComponent<CardController>().AddGetOutOfJailCard(false);
            int check = 0;
            foreach (var value in cardController.GetComponent<CardController>().GetOpportunityKnocksCards())
            {
                if (value.Equals(jailCardOK))
                {
                    check++;
                }
            }
            Assert.AreEqual(1, check);
            check = 0;
            foreach (var value in cardController.GetComponent<CardController>().GetPotLuckCards())
            {
                if (value.Equals(jailCardOK))
                {
                    check++;
                }
            }
            Assert.AreEqual(1, check);
        }

    }
}
