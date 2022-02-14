namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;
	using UnityEditor;
	using System.Collections;
	[CustomEditor(typeof(IKDVC_DriveSystem))] public class Editor_IKDVC_DriveSystem : Editor
	{
		public override void OnInspectorGUI()
		{
		GUI.enabled = false;
		EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((IKDVC_DriveSystem)target), typeof(IKDVC_DriveSystem), false);
		GUI.enabled = true;

		SerializedProperty vehicleSettings = serializedObject.FindProperty ("vehicleSettings");
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (vehicleSettings, true);
		if (EditorGUI.EndChangeCheck ())
			serializedObject.ApplyModifiedProperties ();

		SerializedProperty wheelSettings = serializedObject.FindProperty ("wheelSettings");
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (wheelSettings, true);
		if (EditorGUI.EndChangeCheck ())
			serializedObject.ApplyModifiedProperties ();

			SerializedProperty inputOverride = serializedObject.FindProperty ("inputOverride");
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (inputOverride, true);
		if (EditorGUI.EndChangeCheck ())
			serializedObject.ApplyModifiedProperties ();

		SerializedProperty wheels = serializedObject.FindProperty ("wheels");
		EditorGUI.BeginChangeCheck ();
		EditorGUILayout.PropertyField (wheels, true);
		if (EditorGUI.EndChangeCheck ())
			serializedObject.ApplyModifiedProperties ();

		if (GUILayout.Button ("Align Wheel Colliders") ) 
		{
			AlignWheelColliders ();
		}		
	}

	public void AlignWheelColliders()
	{
			IKDVC_DriveSystem vehicleController = (IKDVC_DriveSystem)target;			
			Transform defaultColliderParent = vehicleController.wheels [0].collider.transform.parent; // make a reference to the colliders original parent

			vehicleController.wheels [0].collider.transform.parent = vehicleController.wheels [0].mesh.transform;// move colliders to the reference positions
			vehicleController.wheels [1].collider.transform.parent = vehicleController.wheels [1].mesh.transform;
			vehicleController.wheels [2].collider.transform.parent = vehicleController.wheels [2].mesh.transform;
			vehicleController.wheels [3].collider.transform.parent = vehicleController.wheels [3].mesh.transform;
			
			vehicleController.wheels [0].collider.transform.position = new Vector3 (vehicleController.wheels [0].mesh.transform.position.x, 
				vehicleController.wheels [0].collider.transform.position.y, vehicleController.wheels [0].mesh.transform.position.z); //adjust the wheel collider positions on x and z axis to match the new wheel position
			vehicleController.wheels [1].collider.transform.position = new Vector3 (vehicleController.wheels [1].mesh.transform.position.x, 
				vehicleController.wheels [1].collider.transform.position.y, vehicleController.wheels [1].mesh.transform.position.z);
			vehicleController.wheels [2].collider.transform.position = new Vector3 (vehicleController.wheels [2].mesh.transform.position.x, 
				vehicleController.wheels [2].collider.transform.position.y, vehicleController.wheels [2].mesh.transform.position.z);
			vehicleController.wheels [3].collider.transform.position = new Vector3 (vehicleController.wheels [3].mesh.transform.position.x, 
				vehicleController.wheels [3].collider.transform.position.y, vehicleController.wheels [3].mesh.transform.position.z);
			
			vehicleController.wheels [0].collider.transform.parent = defaultColliderParent; // move colliders back to the original parent
			vehicleController.wheels [1].collider.transform.parent = defaultColliderParent;
			vehicleController.wheels [2].collider.transform.parent = defaultColliderParent;
			vehicleController.wheels [3].collider.transform.parent = defaultColliderParent;
		}
	}
}