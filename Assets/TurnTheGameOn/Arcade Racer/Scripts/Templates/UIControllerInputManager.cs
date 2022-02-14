namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    [RequireComponent(typeof(EventSystem))]
    [RequireComponent(typeof(StandaloneInputModule))]
    public class UIControllerInputManager : MonoBehaviour
    {
        public static UIControllerInputManager Instance = null;
        public bool enableControllerNavigation;
        private EventSystem eventSystem;
        private StandaloneInputModule standaloneInputModule;
        private GameObject selected;
        public InputModuleSettings[] inputModuleSettings;

        void Awake()
        {
            eventSystem = GetComponent<EventSystem>();
            standaloneInputModule = GetComponent<StandaloneInputModule>();
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

        void Update()
        {
            if (eventSystem.currentSelectedGameObject != selected)
            {
                selected = eventSystem.currentSelectedGameObject;
                eventSystem.SetSelectedGameObject(null);
                eventSystem.SetSelectedGameObject(selected);
                if (selected.GetComponent<Button>())
                {
                    selected.GetComponent<Button>().Select();
                }
                else if (selected.GetComponent<Toggle>())
                {
                    selected.GetComponent<Toggle>().Select();
                }
            }
            else
            {

            }
        }

        public void SetNewSelection(GameObject _newSelection)
        {
            eventSystem.SetSelectedGameObject(_newSelection);
            selected = _newSelection;
            if (selected)
            {
                if (selected.GetComponent<Button>())
                {
                    selected.GetComponent<Button>().Select();
                }
                else if (selected.GetComponent<Toggle>())
                {
                    selected.GetComponent<Toggle>().Select();
                }
            }
        }

        public void SetInputModuleForControllerType(string _inputType)
        {
            for (int i = 0; i < inputModuleSettings.Length; i++)
            {
                if (inputModuleSettings[i].inputType == _inputType)
                {
                    standaloneInputModule.horizontalAxis = inputModuleSettings[i].horizontalAxis;
                    standaloneInputModule.verticalAxis = inputModuleSettings[i].verticalAxis;
                    standaloneInputModule.submitButton = inputModuleSettings[i].submitButton;
                    standaloneInputModule.cancelButton = inputModuleSettings[i].cancelButton;
                    break;
                }
            }
            TryToUpdateVehicleControllerIfAvailable();
        }

        void TryToUpdateVehicleControllerIfAvailable()
        {
            RGT_PlayerController currentPlayerController = FindObjectOfType<RGT_PlayerController>();
            if (currentPlayerController) currentPlayerController.SetupInput();
        }


    }
}