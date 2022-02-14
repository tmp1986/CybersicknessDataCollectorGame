using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Demo : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if UNITY_EDITOR
		if(UnityEditor.PlayerSettings.colorSpace != ColorSpace.Linear)
		{
			bool r = UnityEditor.EditorUtility.DisplayDialog("Colored Cartoon Procedural Skybox", "We suggest you use Linear Color Space to get a better effect.", "Use Linear Color Space", "Dismiss");
			if(r)
			{
				UnityEditor.PlayerSettings.colorSpace = ColorSpace.Linear;
			}
		}
#endif
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnGUI()
	{
		Color c = GUI.color;
		GUI.color = Color.yellow;
		GUI.Label(new Rect(10,10,600,100), "Select Skybox Material of this scene.");
		GUI.Label(new Rect(10,50,600,100), "Change Properties of Skybox Material to observe.");
		GUI.Label(new Rect(10,90,600,100), "Rotate Directional Light to observe.");
		GUI.color = c;
	}
}
