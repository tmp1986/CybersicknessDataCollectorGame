namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;
	[RequireComponent(typeof(IKDVC_DriveSystem))]
	public class IKDVC_Lights : MonoBehaviour
	{		
		public GameObject brakeLightsObject;
		public GameObject reverseLightsObject;
		private IKDVC_DriveSystem vehicleController;

		void OnEnable ()
		{
			if (!vehicleController) vehicleController = GetComponent <IKDVC_DriveSystem>();
			vehicleController.OnSetIsBraking += OnSetIsBraking;
			vehicleController.OnSetIsNotBraking += OnSetIsNotBraking;
			vehicleController.OnSetIsReversing += OnSetIsReversing;
			vehicleController.OnSetIsNotReversing += OnSetIsNotReversing;
		}

		void OnDisable ()
		{
			vehicleController.OnSetIsBraking -= OnSetIsBraking;
			vehicleController.OnSetIsNotBraking -= OnSetIsNotBraking;
			vehicleController.OnSetIsReversing -= OnSetIsReversing;
			vehicleController.OnSetIsNotReversing -= OnSetIsNotReversing;
		}

		void OnSetIsBraking ()
		{
			TurnOnBrakeLights();
		}

		void OnSetIsNotBraking ()
		{
			TurnOffBrakeLights();
		}

		void OnSetIsReversing ()
		{
			TurnOnReverseLights();
		}

		void OnSetIsNotReversing ()
		{
			TurnOffReverseLights();
		}

		void TurnOff()
		{
			TurnOffReverseLights ();
			TurnOffBrakeLights ();
		}

		void TurnOnBrakeLights ()
		{
			TurnOffReverseLights ();
			brakeLightsObject.SetActive(true);
		}

		void TurnOffBrakeLights ()
		{
			brakeLightsObject.SetActive(false);
		}

		void TurnOnReverseLights ()
		{
			TurnOffBrakeLights ();
			reverseLightsObject.SetActive(true);
		}

		void TurnOffReverseLights ()
		{
			reverseLightsObject.SetActive(false);
		}

		public Light headLightL;
		public Light headLightR;
		public Light headLightFlareL;
		public Light headLightFlareR;
		public Light tailLightL;
		public Light tailLightR;

		void Awake () {
			if (IKD_DemoController.instance) {
				if (IKD_DemoController.instance.isDay) {
					DisableHeadLights ();
				} else {
					EnableHeadLights ();
				}
			}
		}

		public void EnableHeadLights () {
			headLightL.enabled = true;
			headLightR.enabled = true;
			headLightFlareL.enabled = true;
			headLightFlareR.enabled = true;
			// tailLightL.enabled = true;
			// tailLightR.enabled = true;
		}

		public void DisableHeadLights () {
			headLightL.enabled = false;
			headLightR.enabled = false;
			headLightFlareL.enabled = false;
			headLightFlareR.enabled = false;
			// tailLightL.enabled = false;
			// tailLightR.enabled = false;
		}
		
	}
}