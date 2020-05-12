using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CameraControllerTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void CameraControllerTestSimplePasses()
        {
            GameObject cameraController = new GameObject();
            cameraController.AddComponent<CameraController>();
            Assert.NotNull(cameraController.GetComponent<CameraController>());
        }

        [Test]
        public void ChangeCameraRotationTest()
        {
            GameObject cameraController = new GameObject();
            cameraController.AddComponent<CameraController>();
            Assert.NotNull(cameraController.GetComponent<CameraController>());
            cameraController.GetComponent<CameraController>().ChangeCameraRotation(5);
            Assert.AreEqual(Quaternion.Euler(90, 0, 90), cameraController.GetComponent<CameraController>().GetTargetRotation());
            cameraController.GetComponent<CameraController>().ChangeCameraRotation(15);
            Assert.AreEqual(Quaternion.Euler(90, 0, 0), cameraController.GetComponent<CameraController>().GetTargetRotation());
            cameraController.GetComponent<CameraController>().ChangeCameraRotation(25);
            Assert.AreEqual(Quaternion.Euler(90, 0, -90), cameraController.GetComponent<CameraController>().GetTargetRotation());
            cameraController.GetComponent<CameraController>().ChangeCameraRotation(35);
            Assert.AreEqual(Quaternion.Euler(90, 0, -180), cameraController.GetComponent<CameraController>().GetTargetRotation());
        }

    }
}
