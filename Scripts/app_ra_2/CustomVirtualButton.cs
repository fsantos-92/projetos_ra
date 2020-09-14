using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;

namespace App.Core
{

    public class CustomVirtualButton : MonoBehaviour, IVirtualButtonEventHandler
    {
        [SerializeField]
        GameObject VtButton;
        // Start is called before the first frame update
        void Start()
        {
            VtButton.GetComponent<VirtualButtonBehaviour>().RegisterEventHandler(this);
        }

        public void OnButtonPressed(VirtualButtonBehaviour vb)
        {
            Debug.Log("Pressed");
        }
        public void OnButtonReleased(VirtualButtonBehaviour vb)
        {
            Debug.Log("Released");
        }
    }
}
