namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;

	[System.Serializable] public struct IKD_VehicleWheel
	{
        public string name;
        public GameObject mesh;
        public Transform meshTransform;
        public WheelCollider collider;
        public IKDVC_Wheel wheel;
        public AudioSource audioSource;
        public ParticleSystem wheelSmokeParticleSystem;
	}
}