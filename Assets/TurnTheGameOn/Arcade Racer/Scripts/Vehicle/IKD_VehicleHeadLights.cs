using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnTheGameOn.IKDriver{
	public class IKD_VehicleHeadLights : MonoBehaviour {

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