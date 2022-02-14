namespace TurnTheGameOn.IKDriver
{
	using UnityEditor;

    [CustomEditor(typeof(IKD_AISettings))] public class IKDE_AISettings : Editor
    {
        private bool showCautionSettings;
        private bool showSensorSettings;
        private bool showInputSensitivitySettings;
        private bool showLookAheadSettings;
        private bool showStuckOrLostSettings;
        private bool showChangeLaneSettings;

        public override void OnInspectorGUI()
		{
            SerializedProperty routeProgressType = serializedObject.FindProperty("routeProgressType");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(routeProgressType, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            #region Change Lane Settings
            showChangeLaneSettings = EditorGUILayout.Foldout (showChangeLaneSettings, "Change Lane Settings", true);
			if (showChangeLaneSettings)
			{
                EditorGUILayout.BeginVertical("Box");
                SerializedProperty changeLaneCooldown = serializedObject.FindProperty ("changeLaneCooldown");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (changeLaneCooldown, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty changeLaneDistance = serializedObject.FindProperty ("changeLaneDistance");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (changeLaneDistance, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                EditorGUILayout.EndVertical();
            }
            #endregion
            EditorGUILayout.Space ();

            #region Stuck Or Lost Settings
            showStuckOrLostSettings = EditorGUILayout.Foldout (showStuckOrLostSettings, "Stuck or Lost Settings", true);
			if (showStuckOrLostSettings)
			{
                EditorGUILayout.BeginVertical("Box");
                SerializedProperty useWaypointReset = serializedObject.FindProperty ("useWaypointReset");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (useWaypointReset, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty useStuckReset = serializedObject.FindProperty ("useStuckReset");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (useStuckReset, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty waypointReset = serializedObject.FindProperty ("waypointReset");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (waypointReset, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty stuckReset = serializedObject.FindProperty ("stuckReset");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (stuckReset, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();
                
                EditorGUILayout.EndVertical();
            }
            #endregion
            EditorGUILayout.Space ();

            #region Sensor Settings
            showSensorSettings = EditorGUILayout.Foldout (showSensorSettings, "Sensor Settings", true);
			if (showSensorSettings)
			{
                EditorGUILayout.BeginVertical("Box");
                SerializedProperty enableSensors = serializedObject.FindProperty ("enableSensors");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (enableSensors, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty updateInterval = serializedObject.FindProperty ("updateInterval");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (updateInterval, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty detectionLayers = serializedObject.FindProperty ("detectionLayers");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (detectionLayers, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty sensorHeight = serializedObject.FindProperty ("sensorHeight");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (sensorHeight, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty sensorFCenterWidth = serializedObject.FindProperty ("sensorFCenterWidth");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (sensorFCenterWidth, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty sensorFSideWidth = serializedObject.FindProperty ("sensorFSideWidth");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (sensorFSideWidth, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty sensorLRWidth = serializedObject.FindProperty ("sensorLRWidth");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (sensorLRWidth, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty sensorFCenterLength = serializedObject.FindProperty ("sensorFCenterLength");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (sensorFCenterLength, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty sensorFSideLength = serializedObject.FindProperty ("sensorFSideLength");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (sensorFSideLength, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                    SerializedProperty sensorLRLength = serializedObject.FindProperty ("sensorLRLength");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (sensorLRLength, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();
                EditorGUILayout.EndVertical();
            }
            #endregion
            EditorGUILayout.Space ();

            #region Caution Settings
            showCautionSettings = EditorGUILayout.Foldout (showCautionSettings, "Caution Settings", true);
			if (showCautionSettings)
			{
                EditorGUILayout.BeginVertical("Box");
                SerializedProperty cautionMaxAngle = serializedObject.FindProperty ("cautionMaxAngle");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (cautionMaxAngle, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty cautionAngularVelocityFactor = serializedObject.FindProperty ("cautionAngularVelocityFactor");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (cautionAngularVelocityFactor, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty minCautionDefault = serializedObject.FindProperty ("minCautionDefault");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (minCautionDefault, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty minCautionIfGoToPitStopDefault = serializedObject.FindProperty ("minCautionIfGoToPitStopDefault");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (minCautionIfGoToPitStopDefault, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();

                SerializedProperty pitLaneCautionDistanceThreshold = serializedObject.FindProperty ("pitLaneCautionDistanceThreshold");
                EditorGUI.BeginChangeCheck ();
                EditorGUILayout.PropertyField (pitLaneCautionDistanceThreshold, true);
                if (EditorGUI.EndChangeCheck ())
                    serializedObject.ApplyModifiedProperties ();
                EditorGUILayout.EndVertical();
            }
            #endregion
            EditorGUILayout.Space ();

            SerializedProperty inputSensitivity = serializedObject.FindProperty("inputSensitivity");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(inputSensitivity, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            EditorGUILayout.Space();

            SerializedProperty lookAheadMin = serializedObject.FindProperty ("lookAheadMin");
			EditorGUI.BeginChangeCheck ();
			EditorGUILayout.PropertyField (lookAheadMin, true);
			if (EditorGUI.EndChangeCheck ())
				serializedObject.ApplyModifiedProperties ();

            SerializedProperty lookAheadMax = serializedObject.FindProperty("lookAheadMax");
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(lookAheadMax, true);
            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            

        }
    }
}