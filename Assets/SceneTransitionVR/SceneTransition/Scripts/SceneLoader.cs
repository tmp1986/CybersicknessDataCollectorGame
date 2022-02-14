using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

namespace ThiagoMalheiros.SceneTransition
{
    public class SceneLoader : MonoBehaviour
    {
        private static SceneLoader SceneLoaderInstance;
        
        public GameObject LoadingScreen;
        public Image FadeImage;
        public Material FadeMaterial;
        [Tooltip("Use with VR.")]
        public bool VRMode = false;
        [Tooltip("When checked, use the Loading scene as the Loading screen (instead of the Loading UI).")]
        public bool UseSceneForLoadingScreen = true;
        [Tooltip("The name of the Loading scene to load.")]
        public string LoadingSceneName = "Loading";
        [Tooltip("When checked, fade in the loading screen.")]
        public bool FadeInLoadingScreen = true;
        [Tooltip("When checked, fade out the loading screen.")]
        public bool FadeOutLoadingScreen = true;
        [Tooltip("The number of seconds to animate the fade.")]
        public float FadeSeconds = 1f;
        [Tooltip("The number of seconds to show the loading screen after fade in. Set it to 0 to go to the new scene as soon as it's ready.")]
        public float MinimumLoadingScreenSeconds = 1f;
        [Tooltip("The color to use in the fade animation.")]
        public Color FadeColor = Color.black;
        [Tooltip("Whether to ignore timeScale. This is useful if you pause the sceen by setting the timescale to 0.")]
        public bool IgnoreTimeScale = false;

        // Get the progress of the load
        [HideInInspector]
        public float Progress = 0;

        private AsyncOperation SceneLoadingOperation;

        private bool FadingIn = true;
        private bool FadingOut = false;
        private float FadeTime = 0;
        private Color FadeClearColor;
        private bool Loading = false;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static SceneLoader Instance
        {
            get
            {
                if (SceneLoaderInstance == null)
                {
                    SceneLoader sceneLoader = (SceneLoader)GameObject.FindObjectOfType(typeof(SceneLoader));
                    if (sceneLoader != null)
                    {
                        SceneLoaderInstance = sceneLoader;
                    }
                    else
                    {
                        GameObject SceneLoaderPrefab = Resources.Load<GameObject>("SceneTransitionVR/SceneTransition/Prefabs/SceneLoader");
                        SceneLoaderInstance = (GameObject.Instantiate(SceneLoaderPrefab)).GetComponent<SceneLoader>();
                    }
                }
                return SceneLoaderInstance;
            }
        }

        /// <summary>
        /// Loads a scene.
        /// </summary>
        /// <param name="name">Name of the scene to load</param>
        public static void LoadScene(string name)
        {
            Instance.Load(name);
        }

        /// <summary>
        /// Loads a scene.
        /// </summary>
        /// <param buildIndex="buildIndex">Build index of the scene to load</param>
        public static void LoadScene(int buildIndex)
        {
            Instance.Load(buildIndex);
        }

        /// <summary>
        /// Awake
        /// </summary>
        public void Awake()
        {
            Object.DontDestroyOnLoad(this.gameObject);

            // Get rid of any old SceneLoaders
            if (SceneLoaderInstance != null && SceneLoaderInstance != this)
            {
                Destroy(SceneLoaderInstance.gameObject);
                SceneLoaderInstance = this;
            }

            if (VRMode && FadeMaterial == null)
            {
                throw new System.Exception("Fade Material is required for VR Support!");
            }
            if (!VRMode && FadeImage == null)
            {
                throw new System.Exception("Fade Image is required!");
            }
        }

        /// <summary>
        /// Start
        /// </summary>
        public void Start()
        {
            SetFadersEnabled(true);
            if (LoadingScreen != null)
            {
                LoadingScreen.SetActive(false);
            }
            if (!Loading)
            {
                BeginFadeIn();
            }
        }

        /// <summary>
        /// Update 
        /// </summary>
        void Update()
        {
            GL.Clear(true, true, Camera.main.backgroundColor);
            if (FadingIn)
            {
                UpdateFadeIn();
            }
            else if (FadingOut)
            {
                UpdateFadeOut();
            }
        }

        /// <summary>
        /// Begins the fade out.
        /// </summary>
        public void BeginFadeOut()
        {
            UpdateCamera();
            if (FadingIn && FadeTime > 0)
            {
                FadeTime = 1 / FadeTime; // Reverse fade
            }
            else
            {
                FadeTime = 0;
                setFadeColor(Color.clear);
            }
            SetFadersEnabled(true);
            FadingIn = false;
            FadingOut = true;
            FadeClearColor = FadeColor;
            FadeClearColor.a = 0;
        }

        /// <summary>
        /// Begins the fade in.
        /// </summary>
        public void BeginFadeIn()
        {
            UpdateCamera();
            if (FadingOut && FadeTime > 0)
            {
                FadeTime = 1 / FadeTime; // Reverse fade
            }
            else
            {
                FadeTime = 0;
                setFadeColor(FadeColor);
            }
            SetFadersEnabled(true);
            FadingIn = true;
            FadingOut = false;
            FadeClearColor = FadeColor;
            FadeClearColor.a = 0;
        }

        /// <summary>
        /// Ends the fade in.
        /// </summary>
        private void EndFadeIn()
        {
            setFadeColor(Color.clear);
            SetFadersEnabled(false);
            FadingIn = false;
        }

        /// <summary>
        /// Ends the fade out.
        /// </summary>
        private void EndFadeOut()
        {
            setFadeColor(FadeColor);
            FadingOut = false;
        }
        
        /// <summary>
        /// Fade in as a scene is starting
        /// </summary>
        private void UpdateFadeIn()
        {
            FadeTime += deltaTime / FadeSeconds;
            setFadeColor(Color.Lerp(FadeColor, FadeClearColor, FadeTime));
            
            if (FadeTime > 1)
            {
                EndFadeIn();
            }
        }
        
        /// <summary>
        /// Fade out as a scene is ending
        /// </summary>
        private void UpdateFadeOut()
        {
            FadeTime += deltaTime / FadeSeconds;
            setFadeColor(Color.Lerp(FadeClearColor, FadeColor, FadeTime));
            
            if (FadeTime > 1)
            {
                EndFadeOut();
            }
        }
        
        /// <summary>
        /// Loads a scene
        /// </summary>
        /// <param name="name">Name of the scene to load</param>
        public void Load(string name)
        {
            if (!Loading)
            {
                var scene = new Scene
                {
                    SceneName = name
                };
                StartCoroutine(InnerLoad(scene));
            }
        }

        /// <summary>
        /// Loads a scene
        /// </summary>
        /// <param buildIndex="buildIndex">Build index of the scene to load</param>
        public void Load(int buildIndex)
        {
            if (!Loading)
            {
                var scene = new Scene
                {
                    BuildIndex = buildIndex
                };
                StartCoroutine(InnerLoad(scene));
            }
        }

        /// <summary>
        /// Coroutine for loading the scene
        /// </summary>
        /// <returns>The load.</returns>
        /// <param name="name">Name of the scene to load</param>
        IEnumerator InnerLoad(Scene scene)
        {
            Loading = true;
            Progress = 0;

            // Fade out
            BeginFadeOut();
            while (FadingOut)
            {
                yield return 0;
            }

            if (UseSceneForLoadingScreen)
            {
                //Show loading scene
                SceneManager.LoadScene(LoadingSceneName);
            }
            else
            {
                if (!VRMode && LoadingScreen != null)
                {
                    LoadingScreen.SetActive (true);
                }
            }
                
            // Fade in
            if (UseSceneForLoadingScreen || !VRMode)
            {
                if (FadeInLoadingScreen)
                {
                    BeginFadeIn();
                    while (FadingIn)
                    {
                        yield return 0;
                    }
                }
                else
                {
                    EndFadeIn();
                }
            }

            var startTime = time;

            //Start to load the level we want in the background
            if (!string.IsNullOrEmpty(scene.SceneName))
            {
                SceneLoadingOperation = SceneManager.LoadSceneAsync(scene.SceneName);
            }
            else
            {
                SceneLoadingOperation = SceneManager.LoadSceneAsync(scene.BuildIndex);
            }
            SceneLoadingOperation.allowSceneActivation = false;

            //Wait for the level to finish loading
            while (SceneLoadingOperation.progress < 0.9f)
            {
                Progress = SceneLoadingOperation.progress;
                yield return 0;
            }
            Progress = 1f;

            // Wait for MinimumLoadingScreenSeconds
            if (UseSceneForLoadingScreen || !VRMode)
            {
                while (time - startTime < MinimumLoadingScreenSeconds)
                {
                    yield return 0;
                }
            }

            SetFadersEnabled (true); // Enable Faders in new scene before switching to it

            // Fade out
            if (UseSceneForLoadingScreen || !VRMode)
            {
                if (FadeOutLoadingScreen)
                {
                    BeginFadeOut();
                    while (FadingOut)
                    {
                        yield return 0;
                    }
                }
                else
                {
                    EndFadeOut();
                }
            }

            SceneLoadingOperation.allowSceneActivation = true;
            
            while (!SceneLoadingOperation.isDone)
            {
                yield return 0;
            }
            if (LoadingScreen != null)
            {
                LoadingScreen.SetActive(false);
            }

            // Fade in
            BeginFadeIn();

            Loading = false; // At this point is should be safe to start a new load even though it's still fading in
        }

        /// <summary>
        /// Setup the sceneLoader canvas based on the VR Support
        /// </summary>
        private void UpdateCamera()
        {
            if (VRMode && FadeMaterial != null)
            {
                // Find all cameras and add fade material to them (initially disabled)
                foreach (Camera c in Camera.allCameras)
                {
                    if (c.gameObject.GetComponent<ScreenFadeControl>() == null)
                    {
                        var fadeControl = c.gameObject.AddComponent<ScreenFadeControl>();
                        fadeControl.fadeMaterial = FadeMaterial;
                    }
                }
            }
        }

        /// <summary>
        /// Sets if the faders are enabled.
        /// </summary>
        /// <param name="value">If set to <c>true</c> value.</param>
        private void SetFadersEnabled(bool value)
        {
            if (VRMode)
            {
                // Find all cameras and set enabled
                foreach (Camera c in Camera.allCameras)
                {
                    if (c.gameObject.GetComponent<ScreenFadeControl> () != null)
                    {
                        c.gameObject.GetComponent<ScreenFadeControl> ().enabled = value;
                    }
                }
            }
            else
            {
                FadeImage.gameObject.SetActive(value);
            }
        }

        /// <summary>
        /// Sets the color of the fade.
        /// </summary>
        /// <param name="value">Value.</param>
        private void setFadeColor(Color value)
        {
            if (VRMode)
            {
                FadeMaterial.color = value;
            }
            else
            {
                FadeImage.color = value;
            }
        }

        private float deltaTime
        {
            get
            {
                return IgnoreTimeScale ? Time.unscaledDeltaTime : Time.deltaTime;
            }
        }

        private float time
        {
            get
            {
                return IgnoreTimeScale ? Time.unscaledTime : Time.time;
            }
        }
    }

    class Scene
    {
        public string SceneName;
        public int BuildIndex;
    }
}
