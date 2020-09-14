using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using App.Game;
using UnityEngine.EventSystems;
public class RaycastCamera : MonoBehaviour
{
    public GameObject GameScreen;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "ARModel")
                {
                    hit.transform.gameObject.GetComponent<ModelBehaviour>().PlayAnim();
                }
            }
        }
    }
}
