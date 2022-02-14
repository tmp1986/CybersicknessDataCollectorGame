using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TurnTheGameOn.IKDriver {
	[ExecuteInEditMode][System.Serializable]
	public class IKD_AISensor : MonoBehaviour {

		public bool hit { get; private set; }
		public float hitDistance { get; private set; }
		public string hitTag { get; private set; }
		public string hitLayer { get; private set; }
		public Transform hitTransform { get; private set; }
		// options
		public Color detectColor;
		public Color normalColor;
		// set through custom inspector
		[HideInInspector] public LayerMask layerMask;
		[HideInInspector] public Vector3 boxSize;
		[HideInInspector] public int number;
		[HideInInspector] public float updateInterval;
		[HideInInspector] public float height;
		[HideInInspector] public float length;
		[HideInInspector] public float width;
		[HideInInspector] public float centerWidth;
		// cached to prevent gc
		[HideInInspector] private Vector3 origin;
		[HideInInspector] private Vector3 direction;
		[HideInInspector] private Quaternion rotation;
		[HideInInspector] private Vector3 sensorPosition;
		[HideInInspector] private RaycastHit boxHit;
		[HideInInspector] private Color color;
		[HideInInspector] private Vector3 offset;
		[HideInInspector] private Transform transformRef;
		[HideInInspector] private float updateIntervalTimer;

		void Update ()
		{
			updateIntervalTimer -= Time.deltaTime;
			if (updateIntervalTimer <= 0.0) {
				updateIntervalTimer = updateInterval;
				BoxCast ();					
			}
		}

		void BoxCast ()
		{
			origin = transform.position;
			direction = this.transform.forward;
			rotation = transform.rotation;
			if (Physics.BoxCast (origin, boxSize, direction, out boxHit, rotation, length, layerMask, QueryTriggerInteraction.UseGlobal)) {
				hitDistance = boxHit.distance;
				hitTag = boxHit.transform.tag;
				hitTransform = boxHit.transform;
				hitLayer = LayerMask.LayerToName (boxHit.transform.gameObject.layer);
				hit = true;
			}
			else {
				hitDistance = length;
				hit = false;
				hitLayer = "";
				hitTag = "";
			}
		}

		public void OnDrawGizmos ()
		{
			if (hitDistance == length) {
				color = normalColor;
			}
			else {
				color = detectColor;
			}
			Gizmos.color = color;
			//Debug.DrawLine (origin, origin + direction * hitDistance, col);
			offset = new Vector3 (boxSize.x * 2.0f, boxSize.y * 2.0f, hitDistance);
			DrawCube (origin + direction * (hitDistance/2), transform.rotation, offset);
		}

		public static void DrawCube(Vector3 position, Quaternion rotation, Vector3 scale)
		{
			Matrix4x4 cubeTransform = Matrix4x4.TRS(position, rotation, scale);
			Matrix4x4 oldGizmosMatrix = Gizmos.matrix;
			Gizmos.matrix *= cubeTransform;
			Gizmos.DrawCube(Vector3.zero, Vector3.one);
			Gizmos.matrix = oldGizmosMatrix;
		}

		public void UpdateSensorTransform ()
		{
			boxSize.x = width;
			boxSize.y = height;
			switch (number)
			{
			case 0:
				sensorPosition = transform.localPosition;
				sensorPosition.x = ((centerWidth + 0.01f) * -2.0f) - (centerWidth + 0.01f) - ((width + 0.03f) * 5);//- width - width - width - width;// + (width * -4.0f);
				transform.localPosition = sensorPosition;
				break;
			case 1:
				sensorPosition = transform.localPosition;
				sensorPosition.x = ((centerWidth + 0.01f) * -2.0f) - (centerWidth + 0.01f) - ((width + 0.02f) * 3);// - width - width;// + (width * -3.0f);
				transform.localPosition = sensorPosition;
				break;
			case 2:
				sensorPosition = transform.localPosition;
				sensorPosition.x = ((centerWidth + 0.01f) * -2.0f) - (centerWidth + 0.01f) - (width + 0.01f);// + (width * -2.0f);
				transform.localPosition = sensorPosition;
				break;
			case 3:
				sensorPosition = transform.localPosition;
				sensorPosition.x = ((centerWidth + 0.01f) * -2.0f);
				transform.localPosition = sensorPosition;
				break;
			case 4:
				sensorPosition = transform.localPosition;
				sensorPosition.x = 0;
				transform.localPosition = sensorPosition;
				break;
			case 5:
				sensorPosition = transform.localPosition;
				sensorPosition.x = (centerWidth + 0.01f) * 2.0f;
				transform.localPosition = sensorPosition;
				break;
			case 6:
				sensorPosition = transform.localPosition;
				sensorPosition.x = ((centerWidth + 0.01f) * 2.0f) + (centerWidth + 0.01f) + (width + 0.01f);// + (width * -2.0f);
				transform.localPosition = sensorPosition;
				break;
			case 7:
				sensorPosition = transform.localPosition;
				sensorPosition.x = ((centerWidth + 0.01f) * 2.0f) + (centerWidth + 0.01f) + ((width + 0.02f) * 3);// - width - width;// + (width * -3.0f);
				transform.localPosition = sensorPosition;
				break;
			case 8:
				sensorPosition = transform.localPosition;
				sensorPosition.x = ((centerWidth + 0.01f) * 2.0f) + (centerWidth + 0.01f) + ((width + 0.03f) * 5);// - width - width;// + (width * -3.0f);
				transform.localPosition = sensorPosition;
				break;
			}
		}

	}
}