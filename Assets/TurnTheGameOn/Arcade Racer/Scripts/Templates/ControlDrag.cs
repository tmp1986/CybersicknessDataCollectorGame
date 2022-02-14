using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlDrag : MonoBehaviour
{
    public Rigidbody _rigidbody;
    public float maxDrag, maxAngularDrag;
    public float minDrag, minAngularDrag;
    void Update ()
    {
        if (Input.GetAxis ("Vertical") > 0)
        {
            _rigidbody.drag = minDrag;
            _rigidbody.angularDrag = minAngularDrag;
        }
        else
        {
            _rigidbody.drag = maxDrag;
            _rigidbody.angularDrag = maxAngularDrag;
        }
    }
}