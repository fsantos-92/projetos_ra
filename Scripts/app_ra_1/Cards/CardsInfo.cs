using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace App.Cards
{

    public class CardsInfo : MonoBehaviour
    {
        [SerializeField]
        Button btn;

        void Awake()
        {
            this.btn.onClick.AddListener(BtnOnClick);
        }

        private void BtnOnClick()
        {
            this.gameObject.SetActive(false);
        }
    }

}