namespace TurnTheGameOn.Timer
{
	using UnityEngine;
	using UnityEngine.UI;
	using UnityEngine.EventSystems;
	using System.Collections;
	using UnityEditor;

	[CustomEditor(typeof(Timer))]
	public class GameManagerEditor : Editor 
	{
		[MenuItem("GameObject/UI/Timer")]
		static void Create()
		{
			GameObject canvasCheck = Selection.activeGameObject;
			if (canvasCheck == null) {
				Canvas findCanvas = GameObject.FindObjectOfType<Canvas> ();
				if (findCanvas != null) {
					canvasCheck = findCanvas.gameObject;
				} else {
					canvasCheck = new GameObject ("Canvas", typeof(Canvas));
					canvasCheck.GetComponent<Canvas> ().renderMode = RenderMode.ScreenSpaceOverlay;
					canvasCheck.AddComponent <GraphicRaycaster>();
				}
				GameObject instance = Instantiate (Resources.Load ("Timer", typeof(GameObject))) as GameObject;
				instance.transform.SetParent(canvasCheck.transform);
				instance.name = "Timer";
				instance.transform.localPosition = new Vector3(0,0,0);
				EditorUtility.FocusProjectWindow ();
				Selection.activeObject = instance;
				canvasCheck = null;
				canvasCheck = GameObject.Find ("Event System");
				if(canvasCheck == null)
				{
					canvasCheck = new GameObject ("Event System", typeof(EventSystem));
					canvasCheck.AddComponent<StandaloneInputModule> ();
				}
				canvasCheck = null;
				instance = null;		
			}
			else 
			{
				Canvas findCanvas = GameObject.FindObjectOfType<Canvas> ();
				if (findCanvas != null && canvasCheck.GetComponent<Canvas>() == null) 
				{
					canvasCheck = findCanvas.gameObject;
				}
				GameObject instance = Instantiate (Resources.Load ("Timer", typeof(GameObject))) as GameObject;
				instance.name = "Timer";
				instance.transform.SetParent(canvasCheck.transform);
				instance.transform.localPosition = new Vector3(0,0,0);
				EditorUtility.FocusProjectWindow ();
				Selection.activeObject = instance;
				canvasCheck = null;
				canvasCheck = GameObject.Find ("Event System");
				if(canvasCheck == null)
				{
					canvasCheck = new GameObject ("Event System", typeof(EventSystem));
					canvasCheck.AddComponent<StandaloneInputModule> ();
				}
				canvasCheck = null;
				instance = null;
			}
		}
		
		public override void OnInspectorGUI()
		{
			GUI.enabled = false; EditorGUILayout.ObjectField("Script:", MonoScript.FromMonoBehaviour((Timer)target), typeof(Timer), false); GUI.enabled = true;

			serializedObject.Update();
			Timer myTarget = (Timer)target;

			EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);			
			EditorGUILayout.LabelField ("Timer Options", EditorStyles.boldLabel);

			EditorGUILayout.BeginVertical("Box");
			//Timer State
			SerializedProperty timerState = serializedObject.FindProperty ("timerState");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(timerState, true);
			if(EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();

			EditorGUI.BeginChangeCheck();
			myTarget.timerType = (Timer.TimerType) EditorGUILayout.EnumPopup("Timer Type", myTarget.timerType);
			if(EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();

			if (myTarget.timerType == Timer.TimerType.CountUp)
			{
				//Set Start Time
				SerializedProperty setStartTimeForCountUp = serializedObject.FindProperty ("setStartTimeForCountUp");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(setStartTimeForCountUp, true);
				if(EditorGUI.EndChangeCheck())
					serializedObject.ApplyModifiedProperties();
			}
			if (myTarget.timerType == Timer.TimerType.CountDown || (myTarget.timerType == Timer.TimerType.CountUp && myTarget.setStartTimeForCountUp) || (myTarget.timerType == Timer.TimerType.CountUpInfinite && myTarget.setStartTimeForCountUp))
			{				
				myTarget.startTime = EditorGUILayout.FloatField("Start Time", myTarget.startTime);
			}
			if (myTarget.timerType == Timer.TimerType.CountUp) 
			{
				myTarget.finishTime = EditorGUILayout.FloatField("Finish Time", myTarget.finishTime);
			}
			myTarget.timerSpeed = EditorGUILayout.Slider ("Timer Speed", myTarget.timerSpeed, 0, 100);

			

			if (myTarget.timerType == Timer.TimerType.CountUpInfinite)
			{				
					//Use System Time
				SerializedProperty useSystemTime = serializedObject.FindProperty ("useSystemTime");
					EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(useSystemTime, true);
					if(EditorGUI.EndChangeCheck())
						serializedObject.ApplyModifiedProperties();
			}
			EditorGUILayout.EndVertical ();

			

			if (myTarget.timerType != Timer.TimerType.CountUpInfinite)
			{
				EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);
				EditorGUILayout.LabelField ("Time's Up Event Options", EditorStyles.boldLabel);

				EditorGUILayout.BeginVertical ("Box");
				//Set Zero Timescale
				SerializedProperty setZeroTimescale = serializedObject.FindProperty ("setZeroTimescale");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(setZeroTimescale, true);
				if(EditorGUI.EndChangeCheck())
					serializedObject.ApplyModifiedProperties();
				
				//Times Up Event
				SerializedProperty timesUpEvent = serializedObject.FindProperty ("timesUpEvent");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(timesUpEvent, true);
				if(EditorGUI.EndChangeCheck())
					serializedObject.ApplyModifiedProperties();
				//Destroy On Finish Array
				SerializedProperty destroyOnFinish = serializedObject.FindProperty ("destroyOnFinish");
				EditorGUI.BeginChangeCheck ();
				EditorGUILayout.PropertyField (destroyOnFinish, true);
				if (EditorGUI.EndChangeCheck ())
					serializedObject.ApplyModifiedProperties ();
				//Load Scenes Options
				myTarget.loadSceneOn = (Timer.LoadSceneOn)EditorGUILayout.EnumPopup ("Load Scene On", myTarget.loadSceneOn);
				if (myTarget.loadSceneOn == Timer.LoadSceneOn.SpecificTime)
				{
					myTarget.loadSceneName = EditorGUILayout.TextField ("Scene Name", myTarget.loadSceneName);
					myTarget.timeToLoadScene = EditorGUILayout.FloatField ("Time to Load Scene", myTarget.timeToLoadScene);
				}
				if (myTarget.loadSceneOn == Timer.LoadSceneOn.TimesUp) 
				{
					myTarget.loadSceneName = EditorGUILayout.TextField ("Scene Name", myTarget.loadSceneName);
				}
				EditorGUILayout.EndVertical ();
			}

			EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);

			EditorGUILayout.LabelField ("UI Text Output", EditorStyles.boldLabel);
			EditorGUILayout.BeginVertical("Box");
			
		//		Timer Text
			SerializedProperty textType = serializedObject.FindProperty ("textType");
			EditorGUI.BeginChangeCheck();
			EditorGUILayout.PropertyField(textType, true);
			if(EditorGUI.EndChangeCheck())
				serializedObject.ApplyModifiedProperties();

			if (myTarget.textType == Timer.TextType.DefaultText)
			{
				SerializedProperty timerTextDefault = serializedObject.FindProperty ("timerTextDefault");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(timerTextDefault, true);
				if(EditorGUI.EndChangeCheck())
					serializedObject.ApplyModifiedProperties();
			}
			else if (myTarget.textType == Timer.TextType.TextMeshProUGUI)
			{
				SerializedProperty timerTextTMPUGUI = serializedObject.FindProperty ("timerTextTMPUGUI");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(timerTextTMPUGUI, true);
				if(EditorGUI.EndChangeCheck())
					serializedObject.ApplyModifiedProperties();
			}

			if (myTarget.timerType == Timer.TimerType.CountUpInfinite) {				
				//Display Options
				SerializedProperty displayOptions = serializedObject.FindProperty ("displayOptions");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(displayOptions, true);
				if(EditorGUI.EndChangeCheck())
					serializedObject.ApplyModifiedProperties();			
				
			}

			if (myTarget.timerType != Timer.TimerType.CountUpInfinite) 
			{
				//Time Format
				SerializedProperty timerFormat = serializedObject.FindProperty ("timerFormat");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(timerFormat, true);
				if(EditorGUI.EndChangeCheck())
					serializedObject.ApplyModifiedProperties();
				//
				SerializedProperty leadingZero = serializedObject.FindProperty ("leadingZero");
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(leadingZero, true);
				if(EditorGUI.EndChangeCheck())
					serializedObject.ApplyModifiedProperties();
			}

			// EditorGUILayout.HelpBox ("public timer variables for reference: " + 
			// 		"\nmillisecond " + myTarget.millisecond.ToString () +
			// 		"\nsecond        " + myTarget.second.ToString () +
			// 		"\nminute         " + myTarget.minute.ToString () +
			// 		"\nhour             " + myTarget.hour.ToString () +
			// 		"\nday              " + myTarget.day.ToString ()
			//                          , MessageType.None);

			EditorGUILayout.EndVertical ();
	///
	///
	///
		//	Help Box

			EditorGUILayout.LabelField ("", GUI.skin.horizontalSlider);

			if (GUI.changed) EditorUtility.SetDirty (target);
			EditorGUILayout.HelpBox ("This component requires an event system, if you don't have one in your scene select 'Game Object/UI/Event System'.", MessageType.Info);
			if (GUI.changed) EditorUtility.SetDirty (target);
		}
		
	}
}