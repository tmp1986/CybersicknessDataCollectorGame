using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetGameHub : MonoBehaviour
{
    public Color color;
    public string selectedGame = "";
    public string nextLevelName = "";
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       // GL.Clear(true, true, color);
    }

    public void SelectGame(GameObject btn)
    {
        selectedGame = btn.name.Replace("btn", "");
        PlayerPrefs.SetString("GameType", selectedGame);
        ThiagoMalheiros.SceneTransition.SceneLoader.LoadScene(nextLevelName);
    }
}
