namespace TurnTheGameOn.IKDriver
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(IKD_Waypoint)), CanEditMultipleObjects] public class IKDE_Waypoint : Editor
	{
//		public override void OnInspectorGUI()
//		{
//			// inspector script reference
//			GUI.enabled = false;
//			EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((IKD_Waypoint)target), typeof(IKD_Waypoint), false);
//			GUI.enabled = true;
//		}
		void OnSceneGUI ()
		{
			IKD_Waypoint waypoint = (IKD_Waypoint)target;

			Handles.Label(waypoint.transform.position + new Vector3(0,0.25f,0),
			"    Waypoint Number: " + waypoint.waypointNumber.ToString() + "\n" +
			"    AI Speed Limit: " + (waypoint.onReachWaypointSettings.AISpeedFactor * 100).ToString() + "%\n" +
			"    Junction Points: " + waypoint.junctionPoint.Length
			);
			// Vector3 position = waypoint.transform.position + Vector3.up * 2f;
    		// float size = 0.2f;
	        // float pickSize = size * 0.2f;

    	    // if (Handles.Button(position, Quaternion.identity, size, pickSize, Handles.RectangleHandleCap))
            // 	Debug.Log("The button was pressed!");		

			// Handles.BeginGUI();
        	// if (GUILayout.Button("Test Button", GUILayout.Width(100)))
        	// {
            // 	//
        	// }
        	// Handles.EndGUI();
		}
	}
}