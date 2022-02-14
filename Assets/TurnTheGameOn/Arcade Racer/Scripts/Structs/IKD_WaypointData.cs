namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;
	[System.Serializable] public struct WaypointData
	{
		public string _name;
		public Transform _transform;
		public IKD_Waypoint _waypoint;
	}
}