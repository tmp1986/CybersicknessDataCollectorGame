using UnityEngine;

public class LoadingScreenDisable : MonoBehaviour
{
    public float waitTime;
    void Start()
    {
        LoadingScreen.Instance.DisableLoadingScreen(waitTime);
        Destroy(this);
    }
}
