namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;

	[System.Serializable] public struct IKD_VehicleInputSensitivity
	{
		[Range(0, 1)] public float steering;
		[Range(0, 1)] public float acceleration;
		[Range(0, 1)] public float brake;
	}
}