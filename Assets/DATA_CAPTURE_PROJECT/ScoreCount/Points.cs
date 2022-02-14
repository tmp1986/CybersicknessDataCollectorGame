using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    // public static int score = -1;


    public int score = 0;
    public int topScore = 0;
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("score", 0);
        topScore = PlayerPrefs.GetInt("topscore");

    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.D) && Input.GetKeyUp(KeyCode.UpArrow))
        {
            print("debug mode");

            PlayerPrefs.SetInt("score", 0);
            PlayerPrefs.SetInt("topscore", 0);
            PlayerPrefs.SetString("topScoreName", "Ninguem");

            ThiagoMalheiros.SceneTransition.SceneLoader.LoadScene("EndGame");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        score = PlayerPrefs.GetInt("score") + 1;
        PlayerPrefs.SetInt("score", score);
        
        //if ( score > PlayerPrefs.GetInt("topscore"))
        //{
        //    PlayerPrefs.SetInt("topscore", score);
        //}
            

       
    }

}
