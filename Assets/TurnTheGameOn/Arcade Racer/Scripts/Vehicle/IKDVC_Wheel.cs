namespace TurnTheGameOn.IKDriver
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class IKDVC_Wheel : MonoBehaviour
    {
        public bool isSkidding { get; private set; }
        public bool isPlayingAudio { get; private set; }
        public bool canPlayAudio;
        private Transform skidTrail;
        public Transform skidTrailParent;
        private Vector3 previousRotationEuler;
        private Vector3 colliderOffset;
        private WheelHit hit;
        private ParticleSystem.EmissionModule em;
        private string currentSkidTrailPrefabName;

        public void WheelSkid (IKDVC_DriveSystem vehicleController, WheelCollider wheelCollider, Transform skidPrefab, ParticleSystem skidParticles, float skidAmount, AudioSource audioSource, IKDVC_Audio vehicleAudio)
        {
            if (skidParticles)
            {
                skidParticles.transform.position = transform.position - transform.up * wheelCollider.radius;
                em = skidParticles.emission;
                em.enabled = true;
                skidParticles.Emit (1);
                if (audioSource)
                {
                    if (audioSource.enabled && !isPlayingAudio)
                    {
                        if (!vehicleAudio.isPlayingSkidAudio) audioSource.Play ();
                        vehicleAudio.isPlayingSkidAudio = true;
                        isPlayingAudio = true;
                    }
                }
            }
            if (!isSkidding)
            {
                isSkidding = true;
                SkidOn(wheelCollider, skidPrefab);
            }
            else if (skidTrail != null && isSkidding)
            {
                if (previousRotationEuler != skidTrail.transform.eulerAngles || skidTrail.name != skidPrefab.name)
                {
                   SkidOn(wheelCollider, skidPrefab); 
                }
            }
            previousRotationEuler = skidTrail.transform.eulerAngles;
        }
        
        void SkidOn(WheelCollider wheelCollider, Transform skidPrefab)
        {
            if (skidTrail != null)
            {
                skidTrail.parent = skidTrailParent;
                skidTrail = null;
            }
            skidTrail = Instantiate(skidPrefab);
            skidTrail.parent = transform;
            if (skidTrail)
            {
                skidTrail.localPosition = -Vector3.up * wheelCollider.radius;
                colliderOffset = wheelCollider.center * 2;
                skidTrail.localPosition += colliderOffset;
            }
        }

        public void WheelSkid_Stop(AudioSource audioSource, IKDVC_Audio vehicleAudio, ParticleSystem skidParticles)
        {
            if (isSkidding)
            {
                em = skidParticles.emission;
                em.enabled = false;
                audioSource.Stop();
                vehicleAudio.isPlayingSkidAudio = false;
                isPlayingAudio = false;
                isSkidding = false;
                skidTrail.parent = skidTrailParent;
                skidTrail = null;
            }
        }

    }
}