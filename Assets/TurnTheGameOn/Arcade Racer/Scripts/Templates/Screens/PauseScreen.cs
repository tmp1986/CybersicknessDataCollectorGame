namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class PauseScreen : ScreenBase
    {
        public static PauseScreen Instance = null;
        public string pauseButton;
        public bool isPaused { get; private set; }
        public bool canPause;

        AudioSource[] allAudioSourcesFoundWhenPaused;
        bool[] wasPaused;
        public GameObject firstSelected;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                OnOpenCallback += OnOpenPauseScreen;
                OnCloseCallback += OnClosePauseScreen;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        void OnEnable()
        {
            
        }

        void OnDisable()
        {
           // OnOpenCallback -= OnOpenPauseScreen;
           // OnCloseCallback -= OnClosePauseScreen;
        }

        public void OnOpenPauseScreen()
        {
            RGT_PlayerController.Instance.SetPauseState(true);
            Time.timeScale = 0.0f;
            allAudioSourcesFoundWhenPaused = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
            wasPaused = new bool[allAudioSourcesFoundWhenPaused.Length];
            for (int i = 0; i < allAudioSourcesFoundWhenPaused.Length; i++)
            {
                if (allAudioSourcesFoundWhenPaused[i].enabled && allAudioSourcesFoundWhenPaused[i].isPlaying)
                {
                    allAudioSourcesFoundWhenPaused[i].Pause();
                    wasPaused[i] = true;
                }
            }
            //canvasGroup.interactable = true;
        }

        public void OnClosePauseScreen()
        {
            Time.timeScale = 1.0f;
            for (int i = 0; i < allAudioSourcesFoundWhenPaused.Length; i++)
            {
                if (allAudioSourcesFoundWhenPaused[i].enabled && wasPaused[i] == true && allAudioSourcesFoundWhenPaused[i].gameObject.activeInHierarchy)
                {
                    allAudioSourcesFoundWhenPaused[i].Play();
                }
            }
            isPaused = false;
            UIControllerInputManager.Instance.SetNewSelection(null);
            enabled = true;
            RGT_PlayerController.Instance.SetPauseState(false);
            //canvasGroup.interactable = false;
        }

        private void Update()
        {
            if (!isPaused && canPause && Input.GetButtonDown(pauseButton))
            {
                if (isPaused)
                {
                    //isPaused = false;
                    //Close();
                }
                else
                {
                    isPaused = true;
                    Open();
                    enabled = false;
                }
            }
        }
    }
}