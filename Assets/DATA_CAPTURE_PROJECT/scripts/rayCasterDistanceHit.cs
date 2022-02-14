using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayCasterDistanceHit : MonoBehaviour
{

    public static float hitDistanceMeters = 0;

    void DrawRayHit()
    {

            RaycastHit hitInfo;
            if (Physics.Raycast(this.transform.position, this.transform.forward * 1000, out hitInfo, 1000))
            {
                //We have a hit!
                Debug.DrawLine(this.transform.position, hitInfo.point, Color.red, .01f, true);

                if (hitInfo.collider.gameObject != null)
                {
                    hitDistanceMeters = hitInfo.distance;
                }

            }
            else
            {
                Debug.DrawLine(this.transform.position, this.transform.forward * 1000, Color.green, .01f, true);
                hitDistanceMeters = 1000;
            }
  
    }

    // Update is called once per frame
    void Update()
    {
        DrawRayHit();
    }
}
