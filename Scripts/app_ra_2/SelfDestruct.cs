using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace App.AnimatedItems
{
    public class SelfDestruct : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(this.gameObject);
        }
    }
}
