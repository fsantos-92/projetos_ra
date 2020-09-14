using UnityEngine;
using Vuforia;

namespace App.Vuforia
{
    public class CameraFocus : MonoBehaviour
    {
        void OnEnable()
        {
            CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
            
        }
    }
}
