using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Vuforia;
using App.AnimatedItems;
using App.Interactions;

namespace App.Utils
{
    public class RaycastTest : MonoBehaviour
    {
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                OnScreenTouched(Input.mousePosition);
#elif UNITY_ANDROID || UNITY_IOS
            if (((Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)) && (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)))
                OnScreenTouched(Input.GetTouch(0).position);
            
#endif
        }

        void OnScreenTouched(Vector3 pos)
        {
            Ray ray = Camera.main.ScreenPointToRay(pos);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject.tag == "Interaction")
                {
                    hit.transform.gameObject.GetComponent<BasicInteraction>().PlayAnim();
                }
                // CIE ----------------------------------------------------------------------
                else if (hit.transform.gameObject.tag == "Cie01_interaction")
                {
                    GameObject.Find("Cie01_manager").GetComponent<Cie01_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Cie02_interaction")
                {
                    GameObject.Find("Cie02_manager").GetComponent<Cie02_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Cie03_interaction")
                {
                    GameObject.Find("Cie03_manager").GetComponent<Cie03_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Cie04_interaction")
                {
                    GameObject.Find("Cie04_manager").GetComponent<Cie04_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Cie05_interaction")
                {
                    GameObject.Find("Cie05_manager").GetComponent<Cie05_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                // MAT ----------------------------------------------------------------------
                else if (hit.transform.gameObject.tag == "Mat01_interaction")
                {
                    GameObject.Find("Mat01_manager").GetComponent<Mat01_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Mat02_interaction")
                {
                    GameObject.Find("Mat02_manager").GetComponent<Mat02_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Mat03_interaction")
                {
                    GameObject.Find("Mat03_manager").GetComponent<Mat03_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Mat04_interaction")
                {
                    GameObject.Find("Mat04_manager").GetComponent<Mat04_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Mat05_interaction")
                {
                    GameObject.Find("Mat05_manager").GetComponent<Mat05_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                // GEO ----------------------------------------------------------------------
                else if (hit.transform.gameObject.tag == "Geo01_interaction")
                {
                    GameObject.Find("Geo01_manager").GetComponent<Geo01_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Geo02_interaction")
                {
                    GameObject.Find("Geo02_manager").GetComponent<Geo02_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Geo03_interaction")
                {
                    GameObject.Find("Geo03_manager").GetComponent<Geo03_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Geo04_interaction")
                {
                    GameObject.Find("Geo04_manager").GetComponent<Geo04_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Geo05_interaction")
                {
                    GameObject.Find("Geo05_manager").GetComponent<Geo05_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                // HIS ----------------------------------------------------------------------
                else if (hit.transform.gameObject.tag == "His01_interaction")
                {
                    GameObject.Find("His01_manager").GetComponent<His01_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "His02_interaction")
                {
                    GameObject.Find("His02_manager").GetComponent<His02_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "His03_interaction")
                {
                    GameObject.Find("His03_manager").GetComponent<His03_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "His04_interaction")
                {
                    GameObject.Find("His04_manager").GetComponent<His04_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "His05_interaction")
                {
                    GameObject.Find("His05_manager").GetComponent<His05_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                // POR ----------------------------------------------------------------------
                else if (hit.transform.gameObject.tag == "Por01_interaction")
                {
                    GameObject.Find("Por01_manager").GetComponent<Por01_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Por02_interaction")
                {
                    GameObject.Find("Por02_manager").GetComponent<Por02_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Por03_interaction")
                {
                    GameObject.Find("Por03_manager").GetComponent<Por03_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Por04_interaction")
                {
                    GameObject.Find("Por04_manager").GetComponent<Por04_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
                else if (hit.transform.gameObject.tag == "Por05_interaction")
                {
                    GameObject.Find("Por05_manager").GetComponent<Por05_manager>()
                        .ClickInteraction(hit.transform.gameObject);
                }
            }
        }
    }
}