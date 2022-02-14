namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;
    using TMPro;

    public class OptionsScreen : ScreenBase
    {
        public static OptionsScreen Instance = null;
        public Toggle audioToggle;
        public Toggle automaticTransmissionToggle;
        public Toggle rearViewMirrorToggle;
        public Toggle fixedMiniMapRotationToggle;
        public Toggle miniMapToggle;
        public TMP_Dropdown qualityDropdown;
        public TMP_Dropdown controllerTypeDropdown;
        public GameObject firstSelectedPlayerControlsScreen;
        public GameObject playerControlsScreen;

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

        private void Start()
        {
            if (GameData.GetAudioState() == "OFF")
            {
                audioToggle.isOn = false;
            }
            else if (GameData.GetAudioState() == "ON")
            {
                audioToggle.isOn = true;
            }
            ToggleSound();

            if (GameData.GetAutomaticTransmissionState() == "OFF")
            {
                automaticTransmissionToggle.isOn = false;
            }
            else if (GameData.GetAutomaticTransmissionState() == "ON")
            {
                automaticTransmissionToggle.isOn = true;
            }
            ToggleAutomaticTransmission();

            if (GameData.GetRearViewMirrorState() == "OFF")
            {
                rearViewMirrorToggle.isOn = false;
            }
            else if (GameData.GetRearViewMirrorState() == "ON")
            {
                rearViewMirrorToggle.isOn = true;
            }
            ToggleRearViewMirror();

            if (GameData.GetMiniMapState() == "OFF")
            {
                miniMapToggle.isOn = false;
            }
            else if (GameData.GetMiniMapState() == "ON")
            {
                miniMapToggle.isOn = true;
            }
            ToggleMiniMap();

            if (GameData.GetFixedMiniMapRotationState() == "OFF")
            {
                fixedMiniMapRotationToggle.isOn = false;
            }
            else if (GameData.GetFixedMiniMapRotationState() == "ON")
            {
                fixedMiniMapRotationToggle.isOn = true;
            }
            ToggleFixedMiniMapRotation();


            qualityDropdown.value = GameData.GetQualitySettingsLevel();
            QualitySettings.SetQualityLevel(qualityDropdown.value, true);

            if (GameData.GetControllerType() == "XBOX_ONE_CONTROLLER")
            {
                controllerTypeDropdown.value = 0;
            }
            else if (GameData.GetControllerType() == "KEYBOARD")
            {
                controllerTypeDropdown.value = 1;
            }
        }

        void OnEnable()
        {
            OnOpenCallback += OnOpenOptionsScreen;
            OnCloseCallback += OnCloseOptionsScreen;
        }

        void OnDisable()
        {
            OnOpenCallback -= OnOpenOptionsScreen;
            OnCloseCallback -= OnCloseOptionsScreen;
        }


        public void OnOpenOptionsScreen()
        {
            if (StartScreen.Instance)
            {
                StartScreen.Instance.Close();
            }
            if (PauseScreen.Instance)
            {
                if (PauseScreen.Instance.isPaused)
                {
                    PauseScreen.Instance.canvasGroup.interactable = false;
                }
            }
        }

        public void OnCloseOptionsScreen()
        {
            if (StartScreen.Instance)
            {
                StartScreen.Instance.Open();
            }
            if (PauseScreen.Instance)
            {
                if (PauseScreen.Instance.isPaused)
                {
                    PauseScreen.Instance.Open();
                    PauseScreen.Instance.canvasGroup.interactable = true;
                }
            }
        }

        public void ToggleSound()
        {
            if (audioToggle.isOn)
            {
                AudioListener.pause = false;
                GameData.SetAudioState("ON");
            }
            else
            {
                AudioListener.pause = true;
                GameData.SetAudioState("OFF");
            }
        }

        public void SelectQualitySettingsLevel()
        {
            QualitySettings.SetQualityLevel(qualityDropdown.value);
            GameData.SetQualitySettingsLevel(qualityDropdown.value);
        }

        public void SelectControllerType()
        {
            if (controllerTypeDropdown.value == 0) // 0 is xbox one controller
            {
                GameData.SetControllerType("XBOX_ONE_CONTROLLER");
                if (UIControllerInputManager.Instance != null)
                {
                    UIControllerInputManager.Instance.SetInputModuleForControllerType("XBOX_ONE_CONTROLLER");
                }
            }
            else if (controllerTypeDropdown.value == 1) // 1 is keyboard
            {
                GameData.SetControllerType("KEYBOARD");
                if (UIControllerInputManager.Instance != null)
                {
                    UIControllerInputManager.Instance.SetInputModuleForControllerType("KEYBOARD");
                }
            }
        }

        public void ToggleAutomaticTransmission()
        {
            if (automaticTransmissionToggle.isOn)
            {
                GameData.SetAutomaticTransmissionState("ON");
            }
            else
            {
                GameData.SetAutomaticTransmissionState("OFF");
            }
            if (AutomaticTransmission.Instance != null) AutomaticTransmission.Instance.SetAutomaticTransmissionState();
        }

        public void ToggleRearViewMirror()
        {
            if (rearViewMirrorToggle.isOn)
            {
                GameData.SetRearViewMirrorState("ON");
            }
            else
            {
                GameData.SetRearViewMirrorState("OFF");
            }
            if (RearViewMirror.Instance != null) RearViewMirror.Instance.SetMirrorState();
        }

        public void ToggleFixedMiniMapRotation()
        {
            if (fixedMiniMapRotationToggle.isOn)
            {
                GameData.SetFixedMiniMapRotationState("ON");
            }
            else
            {
                GameData.SetFixedMiniMapRotationState("OFF");
            }
            if (RGT_MiniMapCanvas.Instance != null) RGT_MiniMapCanvas.Instance.SetFixedMiniMapRotationState();
        }

        public void ToggleMiniMap()
        {
            if (miniMapToggle.isOn)
            {
                GameData.SetMiniMapState("ON");
            }
            else
            {
                GameData.SetMiniMapState("OFF");
            }
            if (RGT_MiniMapCanvas.Instance != null) RGT_MiniMapCanvas.Instance.SetMiniMapState();
        }

        public void OpenPlayerControlsScreen()
        {
            playerControlsScreen.SetActive(true);
            
            //Close();

            UIControllerInputManager.Instance.SetNewSelection(firstSelectedPlayerControlsScreen);
        }

        public void ClosePlayerControlsScreen()
        {
            playerControlsScreen.SetActive(false);
            UIControllerInputManager.Instance.SetNewSelection(firstButtonSelection);
        }

    }
}