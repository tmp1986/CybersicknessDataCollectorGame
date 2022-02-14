namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;	

	public class IKD_VehicleCameraSwitch : MonoBehaviour 
	{
		public IKDVC_DriveSystem vehicleController;
		public IKDVC_Audio vehicleAudio;
		public string currentState { get; private set; }
		public int currentStateIndex { get; private set; }
		public IKD_VehicleCameraStateSettings [] cameraStateSettings;
		public Camera carCamera;
		public Camera helmetCamera;
		public Camera lookBackCamera;
		public Transform vehicle;
		public string startState = "ChaseCamera";
		private GameObject uiHud;
        private GameObject uiMiniMapHud;
        public bool isLookingBack { get; private set; }

        void Start ()
		{
			uiHud = GameObject.Find ("UI HUD");
            uiMiniMapHud = GameObject.Find("UI MINIMAP HUD");
            OnChangeState (startState);
		}

		public void SetNextState ()
		{			
			currentStateIndex = currentStateIndex == cameraStateSettings.Length - 1 ? 0 : currentStateIndex += 1;
			currentState = cameraStateSettings [currentStateIndex].stateName;
			SetStateSettings (currentStateIndex);
		}

		public void OnChangeState (string newState) {
			currentState = newState;
			for (int i = 0; i < cameraStateSettings.Length; i++) {
				if (currentState == cameraStateSettings [i].stateName) {
					SetStateSettings (i);
					currentStateIndex = i;
					break;
				}
			}
			SetEnabledCamera ();
		}

		void SetStateSettings (int index) {
			// set enabledObjects
			for (int i = 0; i < cameraStateSettings [index].enabledObjects.Length; i++) {
				if (cameraStateSettings [index].enabledObjects != null) {
					if (cameraStateSettings [index].enabledObjects [i] != null)
						cameraStateSettings [index].enabledObjects [i].SetActive (true);
				}
			}
			// set disabledObjects
			for (int i = 0; i < cameraStateSettings [index].disabledObjects.Length; i++) {
				if (cameraStateSettings [index].disabledObjects != null) {
					if (cameraStateSettings [index].disabledObjects [i] != null)
						cameraStateSettings [index].disabledObjects [i].SetActive (false);
				}
			}
			// set enabledBehaviours
			for (int i = 0; i < cameraStateSettings [index].enabledBehaviours.Length; i++) {
				if (cameraStateSettings [index].enabledBehaviours != null) {
					if (cameraStateSettings [index].enabledBehaviours [i])
						cameraStateSettings [index].enabledBehaviours [i].enabled = true;
				}
			}
			// set disabledBehaviours
			for (int i = 0; i < cameraStateSettings [index].disabledBehaviours.Length; i++) {
				if (cameraStateSettings [index].disabledBehaviours != null) {
					if (cameraStateSettings [index].disabledBehaviours [i])
						cameraStateSettings [index].disabledBehaviours [i].enabled = false;
				}
			}			
			vehicleAudio.SetVolumeLimit (cameraStateSettings [index].volumelimit, cameraStateSettings [index].shiftVolume); // set audioLimiter
		}

		public void SetEnabledCamera ()
		{
			if (carCamera.enabled)
			{
				helmetCamera.enabled = currentState == "HelmetCamera" ? true : false;
				carCamera.enabled = currentState == "ChaseCamera" ? true : false;
			}
			else if (helmetCamera.enabled)
			{
				carCamera.enabled = currentState == "ChaseCamera" ? true : false;
				helmetCamera.enabled = currentState == "HelmetCamera" ? true : false;
			}
			else
			{
				carCamera.enabled = currentState == "ChaseCamera" ? true : false;
				helmetCamera.enabled = currentState == "HelmetCamera" ? true : false;
			}
			transform.parent = currentState == "ChaseCamera" ? null : currentState == "HelmetCamera" ? vehicle : null;
			if (!vehicleController) vehicleController = GetComponent <IKD_VehicleCamera> ().carController;
			if (uiHud)
			{	
				if (currentState == "ChaseCamera")
				{
					uiHud.SetActive (true);
                    if (uiMiniMapHud) uiMiniMapHud.SetActive(true);
                }
				else {
					uiHud.SetActive (false);
                    if (uiMiniMapHud) uiMiniMapHud.SetActive(false);
                }
			}
		}


		/// <summary>
		/// Enter the camera 'Look Back' state
		/// </summary>
		public void LookBackCamera ()
		{
			carCamera.enabled = false;
			helmetCamera.enabled = false;
			lookBackCamera.gameObject.SetActive (true);
            isLookingBack = true;

        }

		/// <summary>
		/// Exit the Camera 'Look Back' state
		/// </summary>
		public void OnLookBackKeyUp ()
		{
			lookBackCamera.gameObject.SetActive (false);
			SetEnabledCamera ();
            isLookingBack = false;
        }

		/// <summary>
		/// Cycles through the configured vehicle cameras
		/// </summary>
		public void CycleCamera()
		{
			SetNextState ();
			SetEnabledCamera ();
		}

	}
}