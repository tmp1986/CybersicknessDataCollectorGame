namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;
    using TurnTheGameOn.IKDriver;

    public class RGT_PlayerController : MonoBehaviour
    {
        public static RGT_PlayerController Instance = null;
        public GameObject miniMapCanvasPrefab;
        public GameObject miniMapCameraPrefab;
        public Image playerMiniMapIconPrefab;
        public RGT_MiniMapCanvas miniMapCanvas;
        public RGT_MiniMapCamera miniMapCamera;
        public IKD_InputAxesSettings[] inputAxesSettings;
        private IKDVC_PlayerInput playerInput;
        public IKD_AvatarSettings avatarSettings;
        public Behaviour[] disableOnPause;

        private void OnEnable()
        {
            SetupMiniMap();
            SetupInput();
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        }

        void SetupMiniMap()
        {
            if (miniMapCamera == null)
            {
                miniMapCanvas = Instantiate(miniMapCanvasPrefab, Vector3.zero, Quaternion.identity).GetComponent<RGT_MiniMapCanvas>();
                miniMapCamera = Instantiate(miniMapCameraPrefab, Vector3.zero, Quaternion.identity).GetComponent<RGT_MiniMapCamera>();
                miniMapCanvas.playerTransform = transform;
                miniMapCamera.target = transform;
                miniMapCanvas.miniMapCamera = miniMapCamera.GetComponent<RGT_MiniMapCamera>();
                miniMapCanvas.RegisterMiniMapIcon(gameObject, playerMiniMapIconPrefab, true);
            }
        }

        public void SetupInput()
        {
            
            playerInput = GetComponent<IKDVC_PlayerInput>();
            if (playerInput != null)
            {
                if (GameData.GetControllerType() == "XBOX_ONE_CONTROLLER")
                {
                    playerInput.playerInputSettings.inputAxes = inputAxesSettings[0];
                    avatarSettings.steeringAxis = inputAxesSettings[0].steering;
                }
                else if (GameData.GetControllerType() == "KEYBOARD")
                {
                    playerInput.playerInputSettings.inputAxes = inputAxesSettings[1];
                    avatarSettings.steeringAxis = inputAxesSettings[1].steering;
                }
            }
        }

        public void SetPauseState(bool isPaused)
        {
            for (int i = 0; i < disableOnPause.Length; i++)
            {
                disableOnPause[i].enabled = !isPaused;
            }
        }


    }
}