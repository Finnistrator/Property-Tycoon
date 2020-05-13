using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class AudioControllerTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void IntializationOfAudtioController()
        {
            GameObject audioController = new GameObject();
            audioController.AddComponent<AudioController>();
            Assert.NotNull(audioController.GetComponent<AudioController>());
        }

        [Test]
        public void PlaySoundTest()
        {
            GameObject audioController = new GameObject();
            audioController.AddComponent<AudioController>();
            Assert.NotNull(audioController.GetComponent<AudioController>());
            audioController.GetComponent<AudioController>().PlaySound(null);
            Assert.AreEqual(true, audioController.GetComponent<AudioController>().GetPlayingTheSong());
        }

        [Test]
        public void PlaySoundTestSecondMethod()
        {
            GameObject audioController = new GameObject();
            audioController.AddComponent<AudioController>();
            Assert.NotNull(audioController.GetComponent<AudioController>());
            audioController.GetComponent<AudioController>().PlaySound(null,100);
            Assert.AreEqual(true, audioController.GetComponent<AudioController>().GetPlayingTheSong());
        }
    }
}
