using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField]
    GameObject CameraTarget;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (this.gameObject.activeInHierarchy)
        {
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, this.CameraTarget.transform.rotation, Time.smoothDeltaTime * 5f);
        }
    }
}
