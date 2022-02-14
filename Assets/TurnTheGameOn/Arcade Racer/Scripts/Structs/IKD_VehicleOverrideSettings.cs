namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;

	[System.Serializable] public struct IKD_VehicleInputOverride
	{
		public bool overrideBrake;
		public bool overrideAcceleration;
		public bool overrideSteering;
		[Range(-1,1)] public float overrideSteeringPower;
		public float overrideBrakePower;
		public float overrideAccelerationPower;
	}
}