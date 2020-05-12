using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class UIControllerTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void IntializationTest()
        {
            GameObject uiController = new GameObject();
            uiController.AddComponent<UIController>();
            Assert.NotNull(uiController.GetComponent<UIController>());
        }

        [Test]
        public void TogglePurchasePropertyWindowTest()
        {
            GameObject uiController = new GameObject();
            uiController.AddComponent<UIController>();
            Assert.NotNull(uiController.GetComponent<UIController>());
            GameObject toggleObj = new GameObject();
            uiController.GetComponent<UIController>().SetTogglePurchasePropertyWindow(toggleObj);
            uiController.GetComponent<UIController>().GetTogglePurchasePropertyWindow().SetActive(true);
            uiController.GetComponent<UIController>().HidePurchasePropertyWindow();
            Assert.AreEqual(true, uiController.GetComponent<UIController>().GetTogglePurchasePropertyWindow().activeInHierarchy);
        }

        [Test]
        public void TogglePurchasePropertyWindowTestWithProperty()
        {
            GameObject uiController = new GameObject();
            uiController.AddComponent<UIController>();
            Assert.NotNull(uiController.GetComponent<UIController>());
            GameObject toggleObj = new GameObject();
            PurchaseableProperty property = new PurchaseableProperty("Test Property", Group.Blue, 50, 2, 10, 100, 200, 300, 500);
            uiController.GetComponent<UIController>().SetTogglePurchasePropertyWindow(toggleObj);
            uiController.GetComponent<UIController>().GetTogglePurchasePropertyWindow().SetActive(true);
            uiController.GetComponent<UIController>().ShowPurchasePropertyWindow(property);
            Assert.AreEqual(true, uiController.GetComponent<UIController>().GetTogglePurchasePropertyWindow().activeInHierarchy);
        }
    }
}
