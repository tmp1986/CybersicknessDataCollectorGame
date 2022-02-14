using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changeToSceneAfterSeconds : MonoBehaviour
{
    public Color color;
    public float initialTimeInSeconds = 10;

    public string nextLevelName = "Gameplay";
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // GL.Clear(true, true, color);
        CountDown();
    }

    void CountDown()
    {
        initialTimeInSeconds -= Time.deltaTime;

       if (initialTimeInSeconds <= 0)
        {
            ThiagoMalheiros.SceneTransition.SceneLoader.LoadScene(nextLevelName);
        }
    }
}
