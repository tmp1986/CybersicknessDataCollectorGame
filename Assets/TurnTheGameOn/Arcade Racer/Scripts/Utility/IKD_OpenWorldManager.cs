namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class IKD_OpenWorldManager : MonoBehaviour
    {
        public static IKD_OpenWorldManager Instance = null;
        public string sceneName;
        public GameObject[] playerPrefabs;
        public GameObject playerController;
        public Transform spawnPoint;
        public bool spawnOnStart;
        public GameObject defaultCamera;

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
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
            playerController = Instantiate(playerPrefabs[0], spawnPoint.position, spawnPoint.rotation);
            defaultCamera.SetActive(false);
            if (LoadingScreen.Instance != null) LoadingScreen.Instance.DisableLoadingScreen(0.1f);
            if (PauseScreen.Instance != null) PauseScreen.Instance.canPause = true;
        }

    }
}