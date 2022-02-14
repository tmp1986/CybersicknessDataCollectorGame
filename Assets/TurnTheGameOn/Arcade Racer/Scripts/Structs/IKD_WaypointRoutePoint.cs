namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;
	[System.Serializable] public struct RoutePoint
	{
			public Vector3 position;
			public Vector3 direction;
			public RoutePoint(Vector3 position, Vector3 direction)
			{
				this.position = position;
				this.direction = direction;
			}
		}
}