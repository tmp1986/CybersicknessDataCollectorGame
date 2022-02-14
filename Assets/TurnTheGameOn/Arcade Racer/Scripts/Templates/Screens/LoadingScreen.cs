using System.Collections;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance = null;
    public GameObject loadingScreen;
    public string[] loadingTextLoop;
    public float loadingTextSpeed = 0.4f;
    public TMPro.TextMeshProUGUI loadingText;

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

    IEnumerator LoopLoadingText()
    {
        while(loadingScreen.gameObject.activeInHierarchy)
        {
            for (int i = 0; i < loadingTextLoop.Length; i++)
            {
                loadingText.text = loadingTextLoop[i];
                yield return new WaitForSeconds(loadingTextSpeed);
            }
        }
    }

    public void EnableLoadingScreen()
    {
        loadingScreen.SetActive(true);
        StartCoroutine(LoopLoadingText());
    }

    public void DisableLoadingScreen(float waitTime)
    {
        Invoke("DisableLoadingScreen", waitTime);
    }

    void DisableLoadingScreen()
    {
        loadingScreen.SetActive(false);
    }

}