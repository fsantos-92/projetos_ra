/*
 From: https://answers.unity.com/questions/773464/webcamtexture-correct-resolution-and-ratio.html
 */

using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections;

public class FrontCameraController : MonoBehaviour
{
    public RawImage rawImage;
    public RectTransform rawImageRT;
    public AspectRatioFitter rawImageARF;

    // Device cameras
    WebCamDevice frontCameraDevice;

    WebCamTexture frontCameraTexture;

    void Awake()
    {
        // Check for device cameras
        if (WebCamTexture.devices.Length == 0)
        {
            Debug.Log("No devices cameras found");
            return;
        }

        // Get the device's cameras and create WebCamTextures with them
        frontCameraDevice = WebCamTexture.devices.Length > 0 ? WebCamTexture.devices.Last() : WebCamTexture.devices.First();

        gameObject.SetActive(false);

    }

    public void OnEnable()
    {
        frontCameraTexture = new WebCamTexture(frontCameraDevice.name);

        // Set camera filter modes for a smoother looking image
        frontCameraTexture.filterMode = FilterMode.Trilinear;

        rawImage.texture = frontCameraTexture;

        frontCameraTexture.Play();
    }

    public void OnDisable()
    {
        if(frontCameraTexture)
            frontCameraTexture.Stop();

        frontCameraTexture = null;

    }

    // Make adjustments to image every frame to be safe, since Unity isn't 
    // guaranteed to report correct data as soon as device camera is started
    void Update()
    {
        if (!frontCameraTexture)
            return;

        // Skip making adjustment for incorrect camera data
        if (frontCameraTexture.width < 100)
        {
            Debug.Log("Still waiting another frame for correct info...");
            return;
        }

        int cwNeeded = frontCameraTexture.videoRotationAngle;
        // Unity helpfully returns the _clockwise_ twist needed
        // guess nobody at Unity noticed their product works in counterclockwise:
        int ccwNeeded = -cwNeeded;

        // IF the image needs to be mirrored, it seems that it
        // ALSO needs to be spun. Strange: but true.
        if (frontCameraTexture.videoVerticallyMirrored) ccwNeeded += 180;

        // you'll be using a UI RawImage, so simply spin the RectTransform
        rawImageRT.localEulerAngles = new Vector3(0f, 0f, ccwNeeded);

        float videoRatio = (float)frontCameraTexture.width / (float)frontCameraTexture.height;

        rawImageARF.aspectRatio = videoRatio;

        if (Application.platform == RuntimePlatform.Android)
            rawImage.transform.localScale = new Vector3(1, -1, 1);
        else
            if (Application.platform == RuntimePlatform.IPhonePlayer)
            rawImage.transform.localScale = new Vector3(-1, -1, 1);
        else
            rawImage.transform.localScale = new Vector3(-1, 1, 1);


    }
}