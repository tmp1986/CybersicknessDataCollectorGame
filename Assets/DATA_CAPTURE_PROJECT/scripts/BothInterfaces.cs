using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BothInterfaces : MonoBehaviour
{

    public Camera OVRCamera;
    public Camera NormalCamera;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRManager.isHmdPresent)
        {
            OVRCamera.enabled = true;
            NormalCamera.enabled = false;
        }
        else
        {
            OVRCamera.enabled = false;
            NormalCamera.enabled = true;
        }
    }
}
