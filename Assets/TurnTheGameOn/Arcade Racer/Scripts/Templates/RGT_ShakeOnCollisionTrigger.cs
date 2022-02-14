namespace TurnTheGameOn.RacingGameTemplate
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class RGT_ShakeOnCollisionTrigger : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            float impactMagnitude = collision.relativeVelocity.magnitude;
            ShakeTransformManager.Instance.DoDefaultShake(impactMagnitude);
        }

    }
}