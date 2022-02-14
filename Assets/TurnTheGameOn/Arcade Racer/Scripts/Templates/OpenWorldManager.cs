namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class OpenWorldManager : MonoBehaviour
    {
        public static OpenWorldManager Instance = null;
        public string sceneName;
        public GameObject[] playerPrefabs;
        public RGT_PlayerController playerController;
        public Transform spawnPoint;
        public bool spawnOnStart;
        public GameObject defaultCamera;

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //SceneManager.SetActiveScene(SceneManager.GetActiveScene());
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
            //DontDestroyOnLoad(gameObject);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == sceneName)
            {
                if (spawnOnStart)
                {
                    Invoke("SpawnPlayer", 1.0f);
                }
            }
        }

        public void SpawnPlayer()
        {
            playerController = Instantiate(playerPrefabs[0], spawnPoint.position, spawnPoint.rotation).GetComponent<RGT_PlayerController>();
            defaultCamera.SetActive(false);
            LoadingScreen.Instance.DisableLoadingScreen(0.5f);
        }

    }
}