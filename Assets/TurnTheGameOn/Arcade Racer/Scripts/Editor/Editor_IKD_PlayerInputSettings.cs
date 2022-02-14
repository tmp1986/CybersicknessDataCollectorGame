namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(IKD_PlayerInputSettings))]
	public class Editor_IKDVC_PlayerInputSettings : Editor
	{
		public override void OnInspectorGUI()
		{
			IKD_PlayerInputSettings vehicleInput = (IKD_PlayerInputSettings)target;

            SerializedProperty defaultCanvas = serializedObject.FindProperty ("defaultCanvas");
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (defaultCanvas, true);
			if (EditorGUI.EndChangeCheck ())
				serializedObject.ApplyModifiedProperties ();

			SerializedProperty mobileCanvas = serializedObject.FindProperty ("mobileCanvas");
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (mobileCanvas, true);
			if (EditorGUI.EndChangeCheck ())
				serializedObject.ApplyModifiedProperties ();

            SerializedProperty uIType = serializedObject.FindProperty("uIType");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(uIType, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            if (vehicleInput.uIType == UIType.Mobile)
            {
                IKD_StaticUtility.m_IKD_UtilitySettings.useMobileController = true;

                SerializedProperty mobileSteeringType = serializedObject.FindProperty("mobileSteeringType");
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.PropertyField(mobileSteeringType, true);
                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
            }
            else
            {
                IKD_StaticUtility.m_IKD_UtilitySettings.useMobileController = false;
            }

            SerializedProperty inputAxes = serializedObject.FindProperty("inputAxes");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(inputAxes, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            EditorUtility.SetDirty (vehicleInput);
		}
	}
}