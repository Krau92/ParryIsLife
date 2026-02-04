using UnityEngine;

[RequireComponent(typeof(Camera))]
public class AspectRatioEnforcer : MonoBehaviour
{

    public float targetAspect = 3.0f / 4.0f; // Tu ratio 3:4
    
    private Camera cam;
    private int lastWidth = 0;
    private int lastHeight = 0;

    void Start()
    {
        cam = GetComponent<Camera>();
        
        //Setting camera aspect ratio at start
        UpdateCameraRect();
    }

    void Update()
    {
        //Update only if the screen size has changed
        if (Screen.width != lastWidth || Screen.height != lastHeight)
        {
            UpdateCameraRect();
        }
    }

    void UpdateCameraRect()
    {
        lastWidth = Screen.width;
        lastHeight = Screen.height;

        // Calculate the scale for the current aspect ratio compared to the target aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;
        float scaleHeight = windowAspect / targetAspect;

        if (scaleHeight < 1.0f) // Pillarbox
        {
            Rect rect = cam.rect;
            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;
            cam.rect = rect;
        }
        else // Letterbox
        {
            float scaleWidth = 1.0f / scaleHeight;
            Rect rect = cam.rect;
            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;
            cam.rect = rect;
        }
    }
}
