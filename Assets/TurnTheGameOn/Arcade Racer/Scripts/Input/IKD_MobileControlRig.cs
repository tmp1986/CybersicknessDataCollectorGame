using UnityEngine;

namespace TurnTheGameOn.IKDriver
{
	public class IKD_MobileControlRig : MonoBehaviour {
		
		#region Public Variables
		public IKDVC_DriveSystem vehicleController;
		public GameObject turnLeftButton;
		public GameObject turnRightButton;
		public GameObject steeringJoystick;
		public GameObject tiltInput;
		public GameObject steeringWheel;
		public GameObject shiftUpButton;
		public GameObject shiftDownButton;
		#endregion
		
		#region Main Methods
		void Start ()
		{
			foreach (Transform t in transform)
			{
				t.gameObject.SetActive(true);
			}

			steeringJoystick.SetActive (false);
			turnLeftButton.SetActive (false);
			turnRightButton.SetActive (false);
			tiltInput.SetActive (false);
			steeringWheel.SetActive (false);

			MobileSteeringType mobileSteeringType = vehicleController.vehicleInput.playerInputSettings.mobileSteeringType;
			switch (mobileSteeringType) 
			{
			case MobileSteeringType.UIButtons:			// Arrow Button Steering
				turnLeftButton.SetActive (true);
				turnRightButton.SetActive (true);
				break;
			case MobileSteeringType.Tilt:				// Tilt Steering
				tiltInput.SetActive (true);
				break;
			case MobileSteeringType.UIJoystick:			// Joystick Steering
				steeringJoystick.SetActive (true);
				break;
			case MobileSteeringType.UISteeringWheel:	// Steering Wheel
				steeringWheel.SetActive (true);
				break;
			}

			shiftUpButton.SetActive (vehicleController.vehicleSettings.manual);
			shiftDownButton.SetActive (vehicleController.vehicleSettings.manual);
		}
		#endregion	

	}
}