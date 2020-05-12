using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class: CameraController
/// </summary>
public class CameraController : Singleton<CameraController>
{
    [SerializeField] private float rotationSpeed = 1;

    private Quaternion targetRotation;

    private void FixedUpdate()
    {
        //transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed);
    }

    /// <summary>
    /// Method: ChangeCameraRotation
    /// ----------------------------------------------------
    /// Changes the camera rotation depending on the position 
    /// on the board.
    /// </summary>
    /// <param name="position"></param>
    public void ChangeCameraRotation(int position)
    {
        if(position >= 0 && position <= 10)
        {
            targetRotation = Quaternion.Euler(90, 0, 90);
        }
        else if(position > 10 && position <= 20)
        {
            targetRotation = Quaternion.Euler(90, 0, 0);
        }
        else if(position > 20 && position <= 30)
        {
            targetRotation = Quaternion.Euler(90, 0, -90);
        }
        else
        {
            targetRotation = Quaternion.Euler(90, 0, -180);
        }
    }
    
    public Quaternion GetTargetRotation()
    {
        return targetRotation;
    }
}
