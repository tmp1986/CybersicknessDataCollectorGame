using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnTheGameOn.IKDriver {

	[System.Serializable]
	public class IKD_AIDetection : MonoBehaviour {

		public IKD_AI_SensorDetails[] info;
		[Space()]
		public bool enableSensors;
		public float updateInterval = 0.1f;
		public LayerMask layerMask;
		[Header("Sensor Height")]
		[Range(0,10)] public float height;
		[Header("Sensor Width")]
		[Range(0,3)] public float FCenterWidth;
		[Range(0,3)] public float FSideWidth;
		[Range(0,3)] public float LRWidth;
		[Header("Sensor Length")]
		[Range(0,30)] public float FCenterLength;
		[Range(0,30)] public float FSideLength;
		[Range(0,10)] public float LRLength;

		void OnEnable(){
			ToggleSensors (enableSensors);
		}

		[ContextMenu ("UpdateSensors")]
		public void UpdateSensors () {
				for (int i = 0; i < info.Length; i++) {
				// set
				info [i].sensor.number = i;
				info [i].sensor.updateInterval = updateInterval;
				info [i].sensor.layerMask = layerMask;
				info [i].sensor.centerWidth = FCenterWidth;	
				info [i].sensor.height = height;	

				switch (i)
				{
				case 0:
					info [i].sensor.width = FSideWidth - 0.02f;
					info [i].sensor.length = FSideLength;
					break;
				case 1:
					info [i].sensor.width = FSideWidth - 0.02f;
					info [i].sensor.length = FSideLength;
					break;
				case 2:
					info [i].sensor.width = FSideWidth - 0.02f;
					info [i].sensor.length = FSideLength;
					break;
				case 3:
					info [i].sensor.width = FCenterWidth - 0.02f;
					info [i].sensor.length = FCenterLength;
					break;
				case 4:
					info [i].sensor.width = FCenterWidth - 0.02f;
					info [i].sensor.length = FCenterLength;
					break;
				case 5:
					info [i].sensor.width = FCenterWidth - 0.02f;
					info [i].sensor.length = FCenterLength;
					break;
				case 6:
					info [i].sensor.width = FSideWidth - 0.02f;
					info [i].sensor.length = FSideLength;
					break;
				case 7:
					info [i].sensor.width = FSideWidth - 0.02f;
					info [i].sensor.length = FSideLength;
					break;
				case 8:
					info [i].sensor.width = FSideWidth - 0.02f;
					info [i].sensor.length = FSideLength;
					break;
				case 9:
					info [i].sensor.width = LRWidth - 0.02f;
					info [i].sensor.length = LRLength;
					break;
				case 10:
					info [i].sensor.width = LRWidth - 0.02f;
					info [i].sensor.length = LRLength;
					break;
				}
			}

			for (int i = 0; i < info.Length; i++) {
				info [i].sensor.UpdateSensorTransform ();
			}

		}					

		void Update () {
			if (enableSensors) 
			{
				for (int i = 0; i < info.Length; i++) {
					// get
					info [i].hit = info [i].sensor.hit;
					info [i].distance = info [i].sensor.hitDistance;
					info [i].tag = info [i].sensor.hitTag;
					info [i].layer = info [i].sensor.hitLayer;
					// set
					info [i].sensor.updateInterval = updateInterval;
					info [i].sensor.layerMask = layerMask;
				}
			}
		}

		void ToggleSensors (bool isTrue) {
			for (int i = 0; i < info.Length; i++) {
				info [i].sensor.enabled = true;
			}
		}

	}
}