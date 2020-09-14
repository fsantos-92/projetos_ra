using UnityEngine;
using UnityEngine.UI;

namespace App.Tools
{
    public class BottomMenu : MonoBehaviour
    {
        [SerializeField]
        Button[] BtnColors;
        [SerializeField]
        GameObject Camera3DObj;
        void Awake()
        {
            foreach (Button element in BtnColors)
            {
                element.onClick.AddListener(delegate { BtnClick(element); });
            }
        }

        void BtnClick(Button btn)
        {
            this.Camera3DObj.GetComponent<Camera>().backgroundColor = btn.GetComponent<Image>().color;
        }
    }
}
