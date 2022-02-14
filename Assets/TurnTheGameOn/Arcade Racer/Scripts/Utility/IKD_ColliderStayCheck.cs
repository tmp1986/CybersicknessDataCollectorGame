namespace TurnTheGameOn.IKDriver
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class IKD_ColliderStayCheck : MonoBehaviour
    {
        public bool isEmpty { get; private set; }
    
        void FixedUpdate()
        {
            isEmpty = true;
        }
    
        void OnTriggerStay(Collider other)
        {
            isEmpty = false;
        }
    }    
}