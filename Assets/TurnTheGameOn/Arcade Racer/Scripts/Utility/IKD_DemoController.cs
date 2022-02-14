using UnityEngine;

namespace TurnTheGameOn.IKDriver
{
	public class IKD_DemoController : MonoBehaviour
	{
		public static IKD_DemoController instance {get; private set;}

		public bool isDay;
		public GameObject leftSteeringController;
		public GameObject rightSteeringController;
		public Transform spawnPoint;
		public DemoVehicles defaultDemoVehicle;
		private Transform currentDemoVehicle;

		void Awake () 
		{
			if (instance == null) 
			{
				instance = this;
			}
			else 
			{
				Destroy (this);
			}
			// spawn the default vehicle
			if (defaultDemoVehicle == DemoVehicles.LeftSteering)
			{
				currentDemoVehicle = Instantiate (leftSteeringController, spawnPoint.position, spawnPoint.rotation).transform;
			}
			else if (defaultDemoVehicle == DemoVehicles.RightSteering)
			{
				currentDemoVehicle = Instantiate (rightSteeringController, spawnPoint.position, spawnPoint.rotation).transform;
			}
			currentDemoVehicle.name = "Spawned IK Driver Prefab";
			spawnPoint.gameObject.SetActive (false);
		}

	}
}