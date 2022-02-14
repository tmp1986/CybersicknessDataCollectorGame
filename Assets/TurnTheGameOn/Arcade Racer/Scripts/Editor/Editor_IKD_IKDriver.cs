namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;
	using UnityEditor;

	[CustomEditor(typeof(IKD_IKDriver))]
	public class Editor_IKD_IKDriver : Editor {

		private bool showCurrentIKDriverTargets, showCurrentIKTargetObjects, showIKSteeringWheelTargets, showOtherIKTargetObjects;

		public override void OnInspectorGUI()
		{
			// inspector script reference
			GUI.enabled = false;
			EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((IKD_IKDriver)target), typeof(IKD_IKDriver), false);
			GUI.enabled = true;

            EditorGUILayout.BeginVertical("Box");
            EditorGUI.BeginChangeCheck();

            SerializedProperty avatarSettings = serializedObject.FindProperty("avatarSettings");
            EditorGUILayout.PropertyField(avatarSettings, true);

            SerializedProperty animator = serializedObject.FindProperty("animator");
            EditorGUILayout.PropertyField(animator, true);

            SerializedProperty steeringWheel = serializedObject.FindProperty("steeringWheel");
            EditorGUILayout.PropertyField(steeringWheel, true);

            SerializedProperty readOnlySteeringWheel = serializedObject.FindProperty("readOnlySteeringWheel");
            EditorGUILayout.PropertyField(readOnlySteeringWheel, true);

            SerializedProperty vehicleRigidbody = serializedObject.FindProperty("vehicleRigidbody");
            EditorGUILayout.PropertyField(vehicleRigidbody, true);

            SerializedProperty gearText = serializedObject.FindProperty("gearText");
            EditorGUILayout.PropertyField(gearText, true);


            if (EditorGUI.EndChangeCheck())
                serializedObject.ApplyModifiedProperties();

            showCurrentIKDriverTargets = EditorGUILayout.Foldout(showCurrentIKDriverTargets, "IK Control Points", true);
            if (showCurrentIKDriverTargets)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUI.BeginChangeCheck();

                SerializedProperty headLookIKCP = serializedObject.FindProperty("headLookIKCP");
                EditorGUILayout.PropertyField(headLookIKCP, true);

                SerializedProperty targetLeftHandIK = serializedObject.FindProperty("targetLeftHandIK");
                EditorGUILayout.PropertyField(targetLeftHandIK, true);

                SerializedProperty targetRightHandIK = serializedObject.FindProperty("targetRightHandIK");
                EditorGUILayout.PropertyField(targetRightHandIK, true);

                SerializedProperty targetLeftFootIK = serializedObject.FindProperty("targetLeftFootIK");
                EditorGUILayout.PropertyField(targetLeftFootIK, true);

                SerializedProperty targetRightFootIK = serializedObject.FindProperty("targetRightFootIK");
                EditorGUILayout.PropertyField(targetRightFootIK, true);

                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
            }

            showCurrentIKTargetObjects = EditorGUILayout.Foldout(showCurrentIKTargetObjects, "Current IK Targets", true);
            if (showCurrentIKTargetObjects)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUI.BeginChangeCheck();

                SerializedProperty leftHandObj = serializedObject.FindProperty("leftHandObj");
                EditorGUILayout.PropertyField(leftHandObj, true);

                SerializedProperty rightHandObj = serializedObject.FindProperty("rightHandObj");
                EditorGUILayout.PropertyField(rightHandObj, true);

                SerializedProperty leftFootObj = serializedObject.FindProperty("leftFootObj");
                EditorGUILayout.PropertyField(leftFootObj, true);

                SerializedProperty rightFootObj = serializedObject.FindProperty("rightFootObj");
                EditorGUILayout.PropertyField(rightFootObj, true);

                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
            }

            showIKSteeringWheelTargets = EditorGUILayout.Foldout(showIKSteeringWheelTargets, "IK Targets", true);
            if (showIKSteeringWheelTargets)
            {
                EditorGUILayout.BeginVertical("Box");
                EditorGUI.BeginChangeCheck();

                SerializedProperty lhswt_W = serializedObject.FindProperty("lhswt_W");
                EditorGUILayout.PropertyField(lhswt_W, true);

                SerializedProperty lhswt_NW = serializedObject.FindProperty("lhswt_NW");
                EditorGUILayout.PropertyField(lhswt_NW, true);

                SerializedProperty lhswt_N = serializedObject.FindProperty("lhswt_N");
                EditorGUILayout.PropertyField(lhswt_N, true);

                SerializedProperty lhswt_NE = serializedObject.FindProperty("lhswt_NE");
                EditorGUILayout.PropertyField(lhswt_NE, true);

                SerializedProperty lhswt_E = serializedObject.FindProperty("lhswt_E");
                EditorGUILayout.PropertyField(lhswt_E, true);

                SerializedProperty lhswt_SE = serializedObject.FindProperty("lhswt_SE");
                EditorGUILayout.PropertyField(lhswt_SE, true);

                SerializedProperty lhswt_S = serializedObject.FindProperty("lhswt_S");
                EditorGUILayout.PropertyField(lhswt_S, true);

                SerializedProperty lhswt_SW = serializedObject.FindProperty("lhswt_SW");
                EditorGUILayout.PropertyField(lhswt_SW, true);

                SerializedProperty rhswt_W = serializedObject.FindProperty("rhswt_W");
                EditorGUILayout.PropertyField(rhswt_W, true);

                SerializedProperty rhswt_NW = serializedObject.FindProperty("rhswt_NW");
                EditorGUILayout.PropertyField(rhswt_NW, true);

                SerializedProperty rhswt_N = serializedObject.FindProperty("rhswt_N");
                EditorGUILayout.PropertyField(rhswt_N, true);

                SerializedProperty rhswt_NE = serializedObject.FindProperty("rhswt_NE");
                EditorGUILayout.PropertyField(rhswt_NE, true);

                SerializedProperty rhswt_E = serializedObject.FindProperty("rhswt_E");
                EditorGUILayout.PropertyField(rhswt_E, true);

                SerializedProperty rhswt_SE = serializedObject.FindProperty("rhswt_SE");
                EditorGUILayout.PropertyField(rhswt_SE, true);

                SerializedProperty rhswt_S = serializedObject.FindProperty("rhswt_S");
                EditorGUILayout.PropertyField(rhswt_S, true);

                SerializedProperty rhswt_SW = serializedObject.FindProperty("rhswt_SW");
                EditorGUILayout.PropertyField(rhswt_SW, true);

                SerializedProperty handShift = serializedObject.FindProperty("handShift");
                EditorGUILayout.PropertyField(handShift, true);

                SerializedProperty footBrake = serializedObject.FindProperty("footBrake");
                EditorGUILayout.PropertyField(footBrake, true);

                SerializedProperty footGas = serializedObject.FindProperty("footGas");
                EditorGUILayout.PropertyField(footGas, true);

                SerializedProperty leftFootIdle = serializedObject.FindProperty("leftFootIdle");
                EditorGUILayout.PropertyField(leftFootIdle, true);

                SerializedProperty footClutch = serializedObject.FindProperty("footClutch");
                EditorGUILayout.PropertyField(footClutch, true);

                SerializedProperty rightFootIdle = serializedObject.FindProperty("rightFootIdle");
                EditorGUILayout.PropertyField(rightFootIdle, true);

                SerializedProperty returnShiftTarget = serializedObject.FindProperty("returnShiftTarget");
                EditorGUILayout.PropertyField(returnShiftTarget, true);

                if (EditorGUI.EndChangeCheck())
                    serializedObject.ApplyModifiedProperties();
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();

            //EditorGUILayout.LabelField("IK Target Transforms", EditorStyles.centeredGreyMiniLabel);
            //EditorGUILayout.BeginVertical("Box");
            //EditorGUILayout.EndVertical();



            //			// target reference
            //			IKD_IKDriver ikd_IKDriver = (IKD_IKDriver)target;
            //			m_Object = new SerializedObject(target);
            //			// editor button width
            //			float buttonWidth = (Screen.width - 30) / 3.0f;

            //			#region Inspector Menu Buttons
            //			EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
            //			EditorGUILayout.LabelField ("IK Driver Configuration Options", EditorStyles.centeredGreyMiniLabel);
            //			// Inspector Buttons - Row 1
            //			EditorGUILayout.BeginHorizontal ();
            //			if (GUILayout.Button ("Avatar", GUILayout.Width (buttonWidth) ))
            //			{
            //				ikd_IKDriver.editorView = "avatar";					/// Avatar
            //			}
            //			if (GUILayout.Button ("Head", GUILayout.Width (buttonWidth) ))
            //			{
            //				ikd_IKDriver.editorView = "head";					/// Head
            //			}
            //			if (GUILayout.Button ("Input", GUILayout.Width (buttonWidth) ))
            //			{
            //				ikd_IKDriver.editorView = "input";					/// Input
            //			}
            //			EditorGUILayout.EndHorizontal ();
            //			// Inspector Buttons - Row 2
            //			EditorGUILayout.BeginHorizontal ();
            //			if (GUILayout.Button ("Shifting", GUILayout.Width (buttonWidth) ))
            //			{
            //				ikd_IKDriver.editorView = "shifting";				/// Shifting
            //			}
            //			if (GUILayout.Button ("Steering", GUILayout.Width (buttonWidth) ))
            //			{
            //				ikd_IKDriver.editorView = "steering";				/// Steering
            //			}
            //			if (GUILayout.Button ("Targets", GUILayout.Width (buttonWidth) ))
            //			{
            //				ikd_IKDriver.editorView = "iktargets";				/// IK Targets
            //			}
            //			EditorGUILayout.EndHorizontal ();
            //			EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
            //			#endregion

            //			// inspector menus
            //			EditorGUILayout.BeginVertical("Box");
            //			#region Avatar
            //			if (ikd_IKDriver.editorView == "avatar") 
            //			{
            //				EditorGUILayout.LabelField ("IK Avatar Settings", EditorStyles.centeredGreyMiniLabel);
            //				EditorGUILayout.BeginVertical("Box");
            //				//ikd_IKDriver.ikActive = EditorGUILayout.Toggle ("IK Active", ikd_IKDriver.ikActive);
            //				//ikd_IKDriver.avatarPosition = EditorGUILayout.Vector3Field ("Avatar Position", ikd_IKDriver.avatarPosition);
            //				//ikd_IKDriver.shiftHand = (IKD_IKDriver.TargetSide) EditorGUILayout.EnumPopup ("Shift Hand", ikd_IKDriver.shiftHand);
            //				//ikd_IKDriver.clutchFoot = (IKD_IKDriver.TargetSide) EditorGUILayout.EnumPopup ("Clutch Foot", ikd_IKDriver.clutchFoot);
            //				//ikd_IKDriver.brakeFoot = (IKD_IKDriver.TargetSide) EditorGUILayout.EnumPopup ("Brake Foot", ikd_IKDriver.brakeFoot);
            //				//ikd_IKDriver.gasFoot = (IKD_IKDriver.TargetSide) EditorGUILayout.EnumPopup ("Gas Foot", ikd_IKDriver.gasFoot);		

            //				EditorGUILayout.EndVertical();

            //				EditorGUILayout.BeginVertical("Box");
            //				//maxValue_rotateLeft = ikd_IKDriver.maxRotateLeft;
            //				//maxValue_rotateRight = ikd_IKDriver.maxRotateRight;

            //				EditorGUILayout.LabelField ("Torso Settings", EditorStyles.centeredGreyMiniLabel);

            //				//ikd_IKDriver.defaultTorsoLeanIn = EditorGUILayout.FloatField ("Default Lean In", ikd_IKDriver.defaultTorsoLeanIn);

            //				//SerializedProperty torsoCurve = serializedObject.FindProperty ("torsoCurve");
            //				//EditorGUI.BeginChangeCheck ();
            //				//EditorGUILayout.PropertyField (torsoCurve, true);
            //				//if (EditorGUI.EndChangeCheck ())
            //				//	serializedObject.ApplyModifiedProperties ();

            //				EditorGUILayout.BeginVertical("Box");
            //					EditorGUILayout.BeginHorizontal();
            //					//	EditorGUILayout.LabelField ("Rotate Range", GUILayout.MaxWidth(Screen.width * 0.3f));
            //					//	EditorGUI.BeginChangeCheck();
            //					//	EditorGUILayout.MinMaxSlider( ref maxValue_rotateLeft, ref maxValue_rotateRight, -15.0f, 15.0f );
            //					EditorGUILayout.EndHorizontal();
            //					//maxValue_rotateLeft = EditorGUILayout.FloatField ("Max Rotate Left", maxValue_rotateLeft);
            //					//maxValue_rotateRight = EditorGUILayout.FloatField ("Max Rotate Right", maxValue_rotateRight);
            //					//ikd_IKDriver.maxRotateLeft = maxValue_rotateLeft;
            //					//ikd_IKDriver.maxRotateRight = maxValue_rotateRight;
            //					//if (EditorGUI.EndChangeCheck())
            //					//	serializedObject.ApplyModifiedProperties();
            //				EditorGUILayout.EndVertical();
            //				EditorGUILayout.BeginVertical("Box");
            //				//axValue_leanLeft = ikd_IKDriver.maxLeanLeft;
            //				//maxValue_leanRight = ikd_IKDriver.maxLeanRight;
            //					EditorGUILayout.BeginHorizontal();
            //					//EditorGUILayout.LabelField ("Lean Range", GUILayout.MaxWidth(Screen.width * 0.3f));
            //					EditorGUI.BeginChangeCheck();
            //					//EditorGUILayout.MinMaxSlider( ref maxValue_leanLeft, ref maxValue_leanRight, -15.0f, 15.0f );
            //					EditorGUILayout.EndHorizontal();
            //					//maxValue_leanLeft = EditorGUILayout.FloatField ("Max Lean Left", maxValue_leanLeft);
            //					//maxValue_leanRight = EditorGUILayout.FloatField ("Max lean Right", maxValue_leanRight);

            //					//ikd_IKDriver.maxLeanLeft = maxValue_leanLeft;
            //					//ikd_IKDriver.maxLeanRight = maxValue_leanRight;
            //					//if (EditorGUI.EndChangeCheck())
            //					//	serializedObject.ApplyModifiedProperties();
            //				EditorGUILayout.EndVertical();
            //				EditorGUILayout.EndVertical();

            //			}
            //			#endregion

            //			#region Head
            //			if (ikd_IKDriver.editorView == "head") 
            //			{
            //				//minValue = ikd_IKDriver.maxLookLeft;
            //				//maxValue = ikd_IKDriver.maxLookRight;
            //				//minValueSpeed = ikd_IKDriver.minLookSpeed;
            //				//maxValueSpeed = ikd_IKDriver.maxLookSpeed;

            //				//EditorGUILayout.LabelField ("IK Head Look Settings", EditorStyles.centeredGreyMiniLabel);

            //				EditorGUILayout.BeginVertical("Box");

            //				EditorGUILayout.BeginHorizontal();
            //				//EditorGUILayout.LabelField ("Look Range", GUILayout.MaxWidth(Screen.width * 0.3f));

            //				//EditorGUI.BeginChangeCheck();
            //				//EditorGUILayout.MinMaxSlider( ref minValue, ref maxValue, minLimit, maxLimit );
            //				EditorGUILayout.EndHorizontal();
            //				//minValue = EditorGUILayout.FloatField ("Max Look Left", minValue);
            //				//maxValue = EditorGUILayout.FloatField ("Max Look Right", maxValue);

            //				//ikd_IKDriver.maxLookLeft = minValue;

            //				//ikd_IKDriver.maxLookRight = maxValue;
            //				//if (EditorGUI.EndChangeCheck())
            //				//	serializedObject.ApplyModifiedProperties();
            //				///
            //				//SerializedProperty defaultLookXPos = serializedObject.FindProperty("defaultLookXPos");
            //				//EditorGUI.BeginChangeCheck();
            //				//EditorGUILayout.PropertyField(defaultLookXPos, true);
            //				//if (EditorGUI.EndChangeCheck())
            //				//	serializedObject.ApplyModifiedProperties();
            //				EditorGUILayout.EndVertical();

            //				EditorGUILayout.BeginVertical("Box");
            //				EditorGUILayout.BeginHorizontal();
            //				//EditorGUILayout.LabelField ("Look Speed", GUILayout.MaxWidth(Screen.width * 0.3f));
            //				//EditorGUILayout.MinMaxSlider( ref minValueSpeed, ref maxValueSpeed, minLimitSpeed, maxLimitSpeed );
            //				EditorGUILayout.EndHorizontal();
            //				//minValueSpeed = EditorGUILayout.FloatField ("Steer Look Speed", minValueSpeed);
            //				//maxValueSpeed = EditorGUILayout.FloatField ("Snap Back Speed", maxValueSpeed);
            //				//ikd_IKDriver.minLookSpeed = minValueSpeed;
            //				//ikd_IKDriver.maxLookSpeed = maxValueSpeed;
            //				EditorGUILayout.EndVertical();
            //			}
            //			#endregion

            //			#region Steering
            //			if (ikd_IKDriver.editorView == "steering")
            //			{
            //				//EditorGUILayout.LabelField ("IK Steering Settings", EditorStyles.centeredGreyMiniLabel);
            //				EditorGUILayout.BeginVertical("Box");
            //				//ikd_IKDriver.steeringTargets = (IKD_IKDriver.SteeringTargets) EditorGUILayout.EnumPopup ("Steering Targets", ikd_IKDriver.steeringTargets);
            //				//ikd_IKDriver.controlSteeringWheel = EditorGUILayout.Toggle ("Control Steering Wheel", ikd_IKDriver.controlSteeringWheel);
            //				//if (ikd_IKDriver.steeringTargets == IKD_IKDriver.SteeringTargets.All) 
            //				//{
            //				//	ikd_IKDriver.steeringWheelRotation = EditorGUILayout.Slider ("Steering Wheel Rotation", ikd_IKDriver.steeringWheelRotation, 0, 360);
            //				//} 
            //				//else if(ikd_IKDriver.steeringTargets == IKD_IKDriver.SteeringTargets.Two)
            //				//{
            //				//	ikd_IKDriver.steeringWheelRotationTwoTargets = EditorGUILayout.Slider ("Steering Wheel Rotation", ikd_IKDriver.steeringWheelRotationTwoTargets, 0, 90);
            //				//}
            //				//ikd_IKDriver.wheelShake = EditorGUILayout.Slider ("Wheel Shake", ikd_IKDriver.wheelShake, 0, 1);
            //				//ikd_IKDriver.defaultSteering = EditorGUILayout.Vector3Field ("Steering Wheel Rotation", ikd_IKDriver.defaultSteering);
            //				//ikd_IKDriver.steeringWheel = EditorGUILayout.ObjectField("Steering Wheel", ikd_IKDriver.steeringWheel, typeof(Transform), true) as Transform;
            ////				ikd_IKDriver.wheelCollider = EditorGUILayout.ObjectField("Wheel Collider", ikd_IKDriver.wheelCollider, typeof(WheelCollider), true) as WheelCollider;
            //				//ikd_IKDriver.steeringRotationSpeed = EditorGUILayout.FloatField ("Steering Rotation Speed", ikd_IKDriver.steeringRotationSpeed);
            //				EditorGUILayout.EndVertical();
            //			}
            //			#endregion

            //			#region Shifting
            //			if (ikd_IKDriver.editorView == "shifting")
            //			{
            //				//EditorGUILayout.LabelField ("IK Shift Settings", EditorStyles.centeredGreyMiniLabel);
            //				EditorGUILayout.BeginVertical("Box");
            //				//ikd_IKDriver.enableShifting = EditorGUILayout.Toggle ("Enable Shifting", ikd_IKDriver.enableShifting);
            //				//ikd_IKDriver.shift = EditorGUILayout.Toggle ("Shift", ikd_IKDriver.shift);
            //				//ikd_IKDriver.shiftAnimSpeed = EditorGUILayout.FloatField ("Shift Anim Speed", ikd_IKDriver.shiftAnimSpeed);
            //				EditorGUILayout.EndVertical();
            //			}
            //			#endregion

            //			#region Input
            //			if (ikd_IKDriver.editorView == "input")
            //			{
            //				//EditorGUILayout.LabelField ("IK Input Settings", EditorStyles.centeredGreyMiniLabel);
            //				//EditorGUILayout.BeginVertical("Box");

            //				//#region Avatar Input Type
            //				//SerializedProperty avatarInputType = serializedObject.FindProperty ("avatarInputType");
            //				//EditorGUI.BeginChangeCheck ();
            //				//EditorGUILayout.PropertyField (avatarInputType, true);
            //				//if (EditorGUI.EndChangeCheck ())
            //				//	serializedObject.ApplyModifiedProperties ();

            //				//EditorUtility.SetDirty (ikd_IKDriver);
            //				//#endregion

            //				//ikd_IKDriver.steerMultiplier = EditorGUILayout.FloatField ("AI Steer Multiplier", ikd_IKDriver.steerMultiplier);
            //				//ikd_IKDriver.steeringAxis = EditorGUILayout.TextField ("Steering Axis", ikd_IKDriver.steeringAxis);
            //				//ikd_IKDriver.throttleAxis = EditorGUILayout.TextField ("Throttle Axis", ikd_IKDriver.throttleAxis);

            //				//m_Property = m_Object.FindProperty("vehicleRigidbody");
            //				//EditorGUILayout.PropertyField(m_Property, new GUIContent("Vehicle Rigidbody"), true);

            //				//m_Property = m_Object.FindProperty("gearText");
            //				//EditorGUILayout.PropertyField(m_Property, new GUIContent("Gear Text"), true);

            //				//m_Object.ApplyModifiedProperties();

            //				//EditorGUILayout.EndVertical();
            //			}
            //			#endregion

            //			#region Targets
            //			if (ikd_IKDriver.editorView == "iktargets") 
            //			{

            //			}
            //			#endregion
            //			EditorGUILayout.EndVertical();
        }
	}
}

//public override void OnInspectorGUI()
//{
//    // inspector script reference
//    GUI.enabled = false;
//    EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((IKD_IKDriver)target), typeof(IKD_IKDriver), false);
//    GUI.enabled = true;
//    // target reference
//    IKD_IKDriver ikd_IKDriver = (IKD_IKDriver)target;
//    m_Object = new SerializedObject(target);
//    // editor button width
//    float buttonWidth = (Screen.width - 30) / 3.0f;

//    #region Inspector Menu Buttons
//    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
//    EditorGUILayout.LabelField("IK Driver Configuration Options", EditorStyles.centeredGreyMiniLabel);
//    // Inspector Buttons - Row 1
//    EditorGUILayout.BeginHorizontal();
//    if (GUILayout.Button("Avatar", GUILayout.Width(buttonWidth)))
//    {
//        ikd_IKDriver.editorView = "avatar";                 /// Avatar
//    }
//    if (GUILayout.Button("Head", GUILayout.Width(buttonWidth)))
//    {
//        ikd_IKDriver.editorView = "head";                   /// Head
//    }
//    if (GUILayout.Button("Input", GUILayout.Width(buttonWidth)))
//    {
//        ikd_IKDriver.editorView = "input";                  /// Input
//    }
//    EditorGUILayout.EndHorizontal();
//    // Inspector Buttons - Row 2
//    EditorGUILayout.BeginHorizontal();
//    if (GUILayout.Button("Shifting", GUILayout.Width(buttonWidth)))
//    {
//        ikd_IKDriver.editorView = "shifting";               /// Shifting
//    }
//    if (GUILayout.Button("Steering", GUILayout.Width(buttonWidth)))
//    {
//        ikd_IKDriver.editorView = "steering";               /// Steering
//    }
//    if (GUILayout.Button("Targets", GUILayout.Width(buttonWidth)))
//    {
//        ikd_IKDriver.editorView = "iktargets";              /// IK Targets
//    }
//    EditorGUILayout.EndHorizontal();
//    EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
//    #endregion

//    // inspector menus
//    EditorGUILayout.BeginVertical("Box");
//    #region Avatar
//    if (ikd_IKDriver.editorView == "avatar")
//    {
//        EditorGUILayout.LabelField("IK Avatar Settings", EditorStyles.centeredGreyMiniLabel);
//        EditorGUILayout.BeginVertical("Box");
//        //ikd_IKDriver.ikActive = EditorGUILayout.Toggle ("IK Active", ikd_IKDriver.ikActive);
//        //ikd_IKDriver.avatarPosition = EditorGUILayout.Vector3Field ("Avatar Position", ikd_IKDriver.avatarPosition);
//        //ikd_IKDriver.shiftHand = (IKD_IKDriver.TargetSide) EditorGUILayout.EnumPopup ("Shift Hand", ikd_IKDriver.shiftHand);
//        //ikd_IKDriver.clutchFoot = (IKD_IKDriver.TargetSide) EditorGUILayout.EnumPopup ("Clutch Foot", ikd_IKDriver.clutchFoot);
//        //ikd_IKDriver.brakeFoot = (IKD_IKDriver.TargetSide) EditorGUILayout.EnumPopup ("Brake Foot", ikd_IKDriver.brakeFoot);
//        //ikd_IKDriver.gasFoot = (IKD_IKDriver.TargetSide) EditorGUILayout.EnumPopup ("Gas Foot", ikd_IKDriver.gasFoot);		

//        EditorGUILayout.EndVertical();

//        EditorGUILayout.BeginVertical("Box");
//        //maxValue_rotateLeft = ikd_IKDriver.maxRotateLeft;
//        //maxValue_rotateRight = ikd_IKDriver.maxRotateRight;

//        EditorGUILayout.LabelField("Torso Settings", EditorStyles.centeredGreyMiniLabel);

//        //ikd_IKDriver.defaultTorsoLeanIn = EditorGUILayout.FloatField ("Default Lean In", ikd_IKDriver.defaultTorsoLeanIn);

//        //SerializedProperty torsoCurve = serializedObject.FindProperty ("torsoCurve");
//        //EditorGUI.BeginChangeCheck ();
//        //EditorGUILayout.PropertyField (torsoCurve, true);
//        //if (EditorGUI.EndChangeCheck ())
//        //	serializedObject.ApplyModifiedProperties ();

//        EditorGUILayout.BeginVertical("Box");
//        EditorGUILayout.BeginHorizontal();
//        //	EditorGUILayout.LabelField ("Rotate Range", GUILayout.MaxWidth(Screen.width * 0.3f));
//        //	EditorGUI.BeginChangeCheck();
//        //	EditorGUILayout.MinMaxSlider( ref maxValue_rotateLeft, ref maxValue_rotateRight, -15.0f, 15.0f );
//        EditorGUILayout.EndHorizontal();
//        //maxValue_rotateLeft = EditorGUILayout.FloatField ("Max Rotate Left", maxValue_rotateLeft);
//        //maxValue_rotateRight = EditorGUILayout.FloatField ("Max Rotate Right", maxValue_rotateRight);
//        //ikd_IKDriver.maxRotateLeft = maxValue_rotateLeft;
//        //ikd_IKDriver.maxRotateRight = maxValue_rotateRight;
//        //if (EditorGUI.EndChangeCheck())
//        //	serializedObject.ApplyModifiedProperties();
//        EditorGUILayout.EndVertical();
//        EditorGUILayout.BeginVertical("Box");
//        //axValue_leanLeft = ikd_IKDriver.maxLeanLeft;
//        //maxValue_leanRight = ikd_IKDriver.maxLeanRight;
//        EditorGUILayout.BeginHorizontal();
//        //EditorGUILayout.LabelField ("Lean Range", GUILayout.MaxWidth(Screen.width * 0.3f));
//        EditorGUI.BeginChangeCheck();
//        //EditorGUILayout.MinMaxSlider( ref maxValue_leanLeft, ref maxValue_leanRight, -15.0f, 15.0f );
//        EditorGUILayout.EndHorizontal();
//        //maxValue_leanLeft = EditorGUILayout.FloatField ("Max Lean Left", maxValue_leanLeft);
//        //maxValue_leanRight = EditorGUILayout.FloatField ("Max lean Right", maxValue_leanRight);

//        //ikd_IKDriver.maxLeanLeft = maxValue_leanLeft;
//        //ikd_IKDriver.maxLeanRight = maxValue_leanRight;
//        //if (EditorGUI.EndChangeCheck())
//        //	serializedObject.ApplyModifiedProperties();
//        EditorGUILayout.EndVertical();
//        EditorGUILayout.EndVertical();

//    }
//    #endregion

//    #region Head
//    if (ikd_IKDriver.editorView == "head")
//    {
//        //minValue = ikd_IKDriver.maxLookLeft;
//        //maxValue = ikd_IKDriver.maxLookRight;
//        //minValueSpeed = ikd_IKDriver.minLookSpeed;
//        //maxValueSpeed = ikd_IKDriver.maxLookSpeed;

//        //EditorGUILayout.LabelField ("IK Head Look Settings", EditorStyles.centeredGreyMiniLabel);

//        EditorGUILayout.BeginVertical("Box");

//        EditorGUILayout.BeginHorizontal();
//        //EditorGUILayout.LabelField ("Look Range", GUILayout.MaxWidth(Screen.width * 0.3f));

//        //EditorGUI.BeginChangeCheck();
//        //EditorGUILayout.MinMaxSlider( ref minValue, ref maxValue, minLimit, maxLimit );
//        EditorGUILayout.EndHorizontal();
//        //minValue = EditorGUILayout.FloatField ("Max Look Left", minValue);
//        //maxValue = EditorGUILayout.FloatField ("Max Look Right", maxValue);

//        //ikd_IKDriver.maxLookLeft = minValue;

//        //ikd_IKDriver.maxLookRight = maxValue;
//        //if (EditorGUI.EndChangeCheck())
//        //	serializedObject.ApplyModifiedProperties();
//        ///
//        //SerializedProperty defaultLookXPos = serializedObject.FindProperty("defaultLookXPos");
//        //EditorGUI.BeginChangeCheck();
//        //EditorGUILayout.PropertyField(defaultLookXPos, true);
//        //if (EditorGUI.EndChangeCheck())
//        //	serializedObject.ApplyModifiedProperties();
//        EditorGUILayout.EndVertical();

//        EditorGUILayout.BeginVertical("Box");
//        EditorGUILayout.BeginHorizontal();
//        //EditorGUILayout.LabelField ("Look Speed", GUILayout.MaxWidth(Screen.width * 0.3f));
//        //EditorGUILayout.MinMaxSlider( ref minValueSpeed, ref maxValueSpeed, minLimitSpeed, maxLimitSpeed );
//        EditorGUILayout.EndHorizontal();
//        //minValueSpeed = EditorGUILayout.FloatField ("Steer Look Speed", minValueSpeed);
//        //maxValueSpeed = EditorGUILayout.FloatField ("Snap Back Speed", maxValueSpeed);
//        //ikd_IKDriver.minLookSpeed = minValueSpeed;
//        //ikd_IKDriver.maxLookSpeed = maxValueSpeed;
//        EditorGUILayout.EndVertical();
//    }
//    #endregion

//    #region Steering
//    if (ikd_IKDriver.editorView == "steering")
//    {
//        //EditorGUILayout.LabelField ("IK Steering Settings", EditorStyles.centeredGreyMiniLabel);
//        EditorGUILayout.BeginVertical("Box");
//        //ikd_IKDriver.steeringTargets = (IKD_IKDriver.SteeringTargets) EditorGUILayout.EnumPopup ("Steering Targets", ikd_IKDriver.steeringTargets);
//        //ikd_IKDriver.controlSteeringWheel = EditorGUILayout.Toggle ("Control Steering Wheel", ikd_IKDriver.controlSteeringWheel);
//        //if (ikd_IKDriver.steeringTargets == IKD_IKDriver.SteeringTargets.All) 
//        //{
//        //	ikd_IKDriver.steeringWheelRotation = EditorGUILayout.Slider ("Steering Wheel Rotation", ikd_IKDriver.steeringWheelRotation, 0, 360);
//        //} 
//        //else if(ikd_IKDriver.steeringTargets == IKD_IKDriver.SteeringTargets.Two)
//        //{
//        //	ikd_IKDriver.steeringWheelRotationTwoTargets = EditorGUILayout.Slider ("Steering Wheel Rotation", ikd_IKDriver.steeringWheelRotationTwoTargets, 0, 90);
//        //}
//        //ikd_IKDriver.wheelShake = EditorGUILayout.Slider ("Wheel Shake", ikd_IKDriver.wheelShake, 0, 1);
//        //ikd_IKDriver.defaultSteering = EditorGUILayout.Vector3Field ("Steering Wheel Rotation", ikd_IKDriver.defaultSteering);
//        //ikd_IKDriver.steeringWheel = EditorGUILayout.ObjectField("Steering Wheel", ikd_IKDriver.steeringWheel, typeof(Transform), true) as Transform;
//        //				ikd_IKDriver.wheelCollider = EditorGUILayout.ObjectField("Wheel Collider", ikd_IKDriver.wheelCollider, typeof(WheelCollider), true) as WheelCollider;
//        //ikd_IKDriver.steeringRotationSpeed = EditorGUILayout.FloatField ("Steering Rotation Speed", ikd_IKDriver.steeringRotationSpeed);
//        EditorGUILayout.EndVertical();
//    }
//    #endregion

//    #region Shifting
//    if (ikd_IKDriver.editorView == "shifting")
//    {
//        //EditorGUILayout.LabelField ("IK Shift Settings", EditorStyles.centeredGreyMiniLabel);
//        EditorGUILayout.BeginVertical("Box");
//        //ikd_IKDriver.enableShifting = EditorGUILayout.Toggle ("Enable Shifting", ikd_IKDriver.enableShifting);
//        //ikd_IKDriver.shift = EditorGUILayout.Toggle ("Shift", ikd_IKDriver.shift);
//        //ikd_IKDriver.shiftAnimSpeed = EditorGUILayout.FloatField ("Shift Anim Speed", ikd_IKDriver.shiftAnimSpeed);
//        EditorGUILayout.EndVertical();
//    }
//    #endregion

//    #region Input
//    if (ikd_IKDriver.editorView == "input")
//    {
//        //EditorGUILayout.LabelField ("IK Input Settings", EditorStyles.centeredGreyMiniLabel);
//        //EditorGUILayout.BeginVertical("Box");

//        //#region Avatar Input Type
//        //SerializedProperty avatarInputType = serializedObject.FindProperty ("avatarInputType");
//        //EditorGUI.BeginChangeCheck ();
//        //EditorGUILayout.PropertyField (avatarInputType, true);
//        //if (EditorGUI.EndChangeCheck ())
//        //	serializedObject.ApplyModifiedProperties ();

//        //EditorUtility.SetDirty (ikd_IKDriver);
//        //#endregion

//        //ikd_IKDriver.steerMultiplier = EditorGUILayout.FloatField ("AI Steer Multiplier", ikd_IKDriver.steerMultiplier);
//        //ikd_IKDriver.steeringAxis = EditorGUILayout.TextField ("Steering Axis", ikd_IKDriver.steeringAxis);
//        //ikd_IKDriver.throttleAxis = EditorGUILayout.TextField ("Throttle Axis", ikd_IKDriver.throttleAxis);

//        //m_Property = m_Object.FindProperty("vehicleRigidbody");
//        //EditorGUILayout.PropertyField(m_Property, new GUIContent("Vehicle Rigidbody"), true);

//        //m_Property = m_Object.FindProperty("gearText");
//        //EditorGUILayout.PropertyField(m_Property, new GUIContent("Gear Text"), true);

//        //m_Object.ApplyModifiedProperties();

//        //EditorGUILayout.EndVertical();
//    }
//    #endregion

//    #region Targets
//    if (ikd_IKDriver.editorView == "iktargets")
//    {
//        EditorGUILayout.LabelField("IK Target Transforms", EditorStyles.centeredGreyMiniLabel);
//        EditorGUILayout.BeginVertical("Box");
//        showCurrentIKDriverTargets = EditorGUILayout.Foldout(showCurrentIKDriverTargets, "IK Control Points", true);
//        if (showCurrentIKDriverTargets)
//        {
//            EditorGUILayout.BeginVertical("Box");
//            ikd_IKDriver.headLookIKCP = EditorGUILayout.ObjectField("Head Look", ikd_IKDriver.headLookIKCP, typeof(Transform), true) as Transform;
//            ikd_IKDriver.targetLeftHandIK = EditorGUILayout.ObjectField("Left Hand", ikd_IKDriver.targetLeftHandIK, typeof(Transform), true) as Transform;
//            ikd_IKDriver.targetRightHandIK = EditorGUILayout.ObjectField("Right Hand", ikd_IKDriver.targetRightHandIK, typeof(Transform), true) as Transform;
//            ikd_IKDriver.targetLeftFootIK = EditorGUILayout.ObjectField("Left Foot", ikd_IKDriver.targetLeftFootIK, typeof(Transform), true) as Transform;
//            ikd_IKDriver.targetRightFootIK = EditorGUILayout.ObjectField("Right Foot", ikd_IKDriver.targetRightFootIK, typeof(Transform), true) as Transform;
//            EditorGUILayout.EndVertical();
//        }

//        showCurrentIKTargetObjects = EditorGUILayout.Foldout(showCurrentIKTargetObjects, "Current IK Targets", true);
//        if (showCurrentIKTargetObjects)
//        {
//            EditorGUILayout.BeginVertical("Box");
//            ikd_IKDriver.leftHandObj = EditorGUILayout.ObjectField("Left Hand", ikd_IKDriver.leftHandObj, typeof(Transform), true) as Transform;
//            ikd_IKDriver.rightHandObj = EditorGUILayout.ObjectField("Right Hand", ikd_IKDriver.rightHandObj, typeof(Transform), true) as Transform;
//            ikd_IKDriver.leftFootObj = EditorGUILayout.ObjectField("Left Foot", ikd_IKDriver.leftFootObj, typeof(Transform), true) as Transform;
//            ikd_IKDriver.rightFootObj = EditorGUILayout.ObjectField("Right Foot", ikd_IKDriver.rightFootObj, typeof(Transform), true) as Transform;
//            EditorGUILayout.EndVertical();
//        }

//        showIKSteeringWheelTargets = EditorGUILayout.Foldout(showIKSteeringWheelTargets, "IK Targets", true);
//        if (showIKSteeringWheelTargets)
//        {
//            EditorGUILayout.BeginVertical("Box");
//            ikd_IKDriver.lhswt_W = EditorGUILayout.ObjectField("LH W", ikd_IKDriver.lhswt_W, typeof(Transform), true) as Transform;
//            ikd_IKDriver.lhswt_NW = EditorGUILayout.ObjectField("LH NW", ikd_IKDriver.lhswt_NW, typeof(Transform), true) as Transform;
//            ikd_IKDriver.lhswt_N = EditorGUILayout.ObjectField("LH N", ikd_IKDriver.lhswt_N, typeof(Transform), true) as Transform;
//            ikd_IKDriver.lhswt_NE = EditorGUILayout.ObjectField("LH NE", ikd_IKDriver.lhswt_NE, typeof(Transform), true) as Transform;
//            ikd_IKDriver.lhswt_E = EditorGUILayout.ObjectField("LH E", ikd_IKDriver.lhswt_E, typeof(Transform), true) as Transform;
//            ikd_IKDriver.lhswt_SE = EditorGUILayout.ObjectField("LH SE", ikd_IKDriver.lhswt_SE, typeof(Transform), true) as Transform;
//            ikd_IKDriver.lhswt_S = EditorGUILayout.ObjectField("LH S", ikd_IKDriver.lhswt_S, typeof(Transform), true) as Transform;
//            ikd_IKDriver.lhswt_SW = EditorGUILayout.ObjectField("LH SW", ikd_IKDriver.lhswt_SW, typeof(Transform), true) as Transform;
//            ikd_IKDriver.rhswt_W = EditorGUILayout.ObjectField("RH W", ikd_IKDriver.rhswt_W, typeof(Transform), true) as Transform;
//            ikd_IKDriver.rhswt_NW = EditorGUILayout.ObjectField("RH NW", ikd_IKDriver.rhswt_NW, typeof(Transform), true) as Transform;
//            ikd_IKDriver.rhswt_N = EditorGUILayout.ObjectField("RH N", ikd_IKDriver.rhswt_N, typeof(Transform), true) as Transform;
//            ikd_IKDriver.rhswt_NE = EditorGUILayout.ObjectField("RH NE", ikd_IKDriver.rhswt_NE, typeof(Transform), true) as Transform;
//            ikd_IKDriver.rhswt_E = EditorGUILayout.ObjectField("RH E", ikd_IKDriver.rhswt_E, typeof(Transform), true) as Transform;
//            ikd_IKDriver.rhswt_SE = EditorGUILayout.ObjectField("RH SE", ikd_IKDriver.rhswt_SE, typeof(Transform), true) as Transform;
//            ikd_IKDriver.rhswt_S = EditorGUILayout.ObjectField("RH S", ikd_IKDriver.rhswt_S, typeof(Transform), true) as Transform;
//            ikd_IKDriver.rhswt_SW = EditorGUILayout.ObjectField("RH SW", ikd_IKDriver.rhswt_SW, typeof(Transform), true) as Transform;
//            ikd_IKDriver.handShift = EditorGUILayout.ObjectField("Hand Shift", ikd_IKDriver.handShift, typeof(Transform), true) as Transform;
//            ikd_IKDriver.footBrake = EditorGUILayout.ObjectField("Foot Brake", ikd_IKDriver.footBrake, typeof(Transform), true) as Transform;
//            ikd_IKDriver.footGas = EditorGUILayout.ObjectField("Foot Gas", ikd_IKDriver.footGas, typeof(Transform), true) as Transform;
//            ikd_IKDriver.leftFootIdle = EditorGUILayout.ObjectField("Left Foot Idle", ikd_IKDriver.leftFootIdle, typeof(Transform), true) as Transform;
//            ikd_IKDriver.footClutch = EditorGUILayout.ObjectField("Foot Clutch", ikd_IKDriver.footClutch, typeof(Transform), true) as Transform;
//            ikd_IKDriver.rightFootIdle = EditorGUILayout.ObjectField("Foot Idle", ikd_IKDriver.rightFootIdle, typeof(Transform), true) as Transform;
//            EditorGUILayout.EndVertical();
//        }
//        EditorGUILayout.EndVertical();
//    }
//    #endregion
//    EditorGUILayout.EndVertical();
//}