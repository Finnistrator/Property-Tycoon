using UnityEngine;

/// <summary>
/// Class: CameraScaler
/// </summary>
public class CameraScaler : MonoBehaviour
{
    [Header("Camera")]
    [SerializeField] private Camera cam = null;
    [Header("Screen Size")]
    [SerializeField] private int xSize = 1920;
    [SerializeField] private int ySize = 1080;
    [Header("Target Aspect Ratio")]
    [SerializeField] private float xRatio = 16;
    [SerializeField] private float yRatio = 9;

    void Start()
    {
        RescaleCamera();
    }

    void Update()
    {
        RescaleCamera();
    }

    private void RescaleCamera()
    {
        if (Screen.width == xSize && Screen.height == ySize) return;

        float targetaspect = xRatio / yRatio;
        float windowaspect = (float)Screen.width / (float)Screen.height;
        float scaleheight = windowaspect / targetaspect;

        if (scaleheight < 1.0f)
        {
            Rect rect = cam.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            cam.rect = rect;
        }
        else
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = cam.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            cam.rect = rect;
        }

        xSize = Screen.width;
        ySize = Screen.height;
    }
    
    void OnPreCull()
    {
        if (Application.isEditor) return;
        Rect wp = cam.rect;
        Rect nr = new Rect(0, 0, 1, 1);

        cam.rect = nr;
        GL.Clear(true, true, Color.black);

        cam.rect = wp;
    }
}