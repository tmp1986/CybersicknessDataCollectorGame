namespace TurnTheGameOn.IKDriver
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[System.Serializable] public struct IKD_VehicleCameraStateSettings
	{
		public string stateName;
		[Range (0.0f, 1.0f)] public float volumelimit;
		[Range (0.0f, 1.0f)] public float shiftVolume;
		public GameObject [] enabledObjects;
		public GameObject [] disabledObjects;
		public Behaviour [] enabledBehaviours;
		public Behaviour [] disabledBehaviours;
	}
}