namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class QuitConfirmScreen : ScreenBase
    {
        public static QuitConfirmScreen Instance = null;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(gameObject);
        }

        public void Button_Yes()
        {
            Application.Quit();
        }

        public void Button_No()
        {
            Close();
            if (StartScreen.Instance) StartScreen.Instance.Open();
        }

        void OnEnable()
        {
            OnOpenCallback += OnOpenQuitConfirmScreenScreen;
            OnCloseCallback += OnCloseQuitConfirmScreenScreen;
        }

        void OnDisable()
        {
            OnOpenCallback -= OnOpenQuitConfirmScreenScreen;
            OnCloseCallback -= OnCloseQuitConfirmScreenScreen;
        }

        public void OnOpenQuitConfirmScreenScreen()
        {
            if (PauseScreen.Instance)
            {
                if (PauseScreen.Instance.isPaused)
                {
                    PauseScreen.Instance.canvasGroup.interactable = false;
                }
            }
        }

        public void OnCloseQuitConfirmScreenScreen()
        {
            if (PauseScreen.Instance)
            {
                if (PauseScreen.Instance.isPaused)
                {
                    PauseScreen.Instance.Open();
                    PauseScreen.Instance.canvasGroup.interactable = true;
                }
            }
        }


    }
}
