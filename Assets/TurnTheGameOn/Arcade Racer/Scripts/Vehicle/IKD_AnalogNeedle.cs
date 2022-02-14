using UnityEngine;

namespace TurnTheGameOn.IKDriver
{
	public class IKD_AnalogNeedle : MonoBehaviour 
	{
		public RectTransform needleRectTransform;
		public float minAngle, maxAngle;
		public float minValue, maxValue;
		private float currentRotation;
		private float linearPoint;
		private float curentPoint;
		private Vector3 newRotation;
		public float rotationSmooth = 0.1f;
		private float previousPoint;
		float currentVelocity;

		public void SetValue (float value)
		{
			if (System.Single.IsNaN (value)) return;
			curentPoint = Mathf.InverseLerp (minValue, maxValue, value);
			linearPoint = Mathf.SmoothDamp (previousPoint, curentPoint, ref currentVelocity, rotationSmooth);
			previousPoint = linearPoint;
			currentRotation = Mathf.Lerp (minAngle, maxAngle, linearPoint);
			if (System.Single.IsNaN (currentRotation)) return;
			newRotation = new Vector3 (0, 0, currentRotation);
			needleRectTransform.localRotation = Quaternion.Euler (newRotation);
		}

	}
}