using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using System;

public class AdditiveSceneLoader : MonoBehaviour
{
	public String [] scenesToLoad;
	private int additiveScenesLoaded;

	public UnityEvent triggerOnComplete;

	void OnEnable ()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void Awake ()
	{
		for (int i = 0; i < scenesToLoad.Length; i++)
		{
			if (!String.IsNullOrEmpty(scenesToLoad [i])) StartCoroutine (LoadSceneAsync (scenesToLoad [i], OnProgress, OnFailure, OnComplete)); // load an art scene if on is assigned
		}		
	}	

	public void UnloadScene (string s)
	{
		if (SceneManager.GetSceneByName (s).isLoaded)
		{
			Debug.Log ("unloading scene " + s);
			//AsyncOperation sceneUnloading = SceneManager.UnloadSceneAsync (s);
			SceneManager.UnloadSceneAsync (s);
		}
	}

	public static IEnumerator LoadSceneAsync (string sceneName, Action<float> OnProgress, Action OnFailure, Action OnComplete)
	{
		// if (SceneManager.GetSceneByName (sceneName) == null)
		// {
		// 	OnFailure ();
		// 	yield break;
		// }
		float startTime;
		startTime = Time.timeSinceLevelLoad;
		AsyncOperation operation = SceneManager.LoadSceneAsync (sceneName, LoadSceneMode.Additive);

		while (operation.progress < 1f)
		{
			OnProgress (operation.progress);
			yield return null;
		}

		
		//SceneManager.SetActiveScene(SceneManager.GetSceneByName (sceneName));
		float totalTime = Time.timeSinceLevelLoad - startTime;
		Debug.Log ("total time to load " + sceneName + ": " + totalTime);

        OnComplete ();
	}
	#region Callbacks
	void OnSceneLoaded (Scene scene, LoadSceneMode mode)
	{
		if (scene.name == scenesToLoad[scenesToLoad.Length - 1]) Invoke ("OnLoadingAllScenesComplete", 0.5f);
	}
	
	void OnLoadingAllScenesComplete ()
	{
		triggerOnComplete.Invoke();
	}

	void OnFailure ()
	{
		Debug.LogError ("scene not loaded - check to make sure it's in the build settings");
	}

	void OnComplete ()
	{
        
		Debug.Log("scene loading is complete");
	}

	void OnProgress (float f)
	{
		Debug.Log("Loading scene progress: " + f);
	}
	#endregion

}