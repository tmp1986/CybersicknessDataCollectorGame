namespace TurnTheGameOn.IKDriver
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	[System.Serializable] public struct IKD_AI_SensorDetails
	{
		public string name;
		public string layer;
		public string tag;
		public bool hit;
		public float distance;
		public IKD_AISensor sensor;
	}
}