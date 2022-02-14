using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controlPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x - 0.05f,this.transform.position.y ,this.transform.position.z);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x + 0.05f, this.transform.position.y, this.transform.position.z);
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x,this.transform.position.y + 0.05f, this.transform.position.z);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 0.05f, this.transform.position.z);
        }

    }
}
