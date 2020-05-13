using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ColourControllerTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void IntializationOfColour()
        {
            GameObject colorController = new GameObject();
            colorController.AddComponent<ColourController>();
            Assert.NotNull(colorController.GetComponent<ColourController>());
        }

        [Test]
        public void TestGetColour()
        {
            GameObject colourController = new GameObject();
            colourController.AddComponent<ColourController>();
            Color GroupBrownColour = new Color(1, 1, 1, 1);
            Assert.NotNull(colourController.GetComponent<ColourController>());
            Assert.AreEqual(GroupBrownColour, colourController.GetComponent<ColourController>().GetGroupColour(Group.Brown));
        }


    }
}
