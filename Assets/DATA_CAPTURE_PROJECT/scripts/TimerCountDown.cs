using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCountDown : MonoBehaviour
{
    public static float initialTimeInSeconds;
    public GameObject textObject;

    public bool countDown = false;


    private void Start()
    {
        initialTimeInSeconds = 0;
    }
    void Update()
    {
        if (textObject == null)
        {
            textObject = GameObject.FindGameObjectWithTag("GameTimer_Manager");
        }
        else
        {
            if (countDown)
            {
                if (initialTimeInSeconds <= 0)
                {
                    textObject.GetComponent<Text>().text = "0:00";
                    Debug.Log("Out of Time.");
                }
                else
                {
                    CountDown();
                }
            }
            else
            {
                 Count();
            }
        }
    }

    void CountDown()
    {
        initialTimeInSeconds -= Time.deltaTime;

        string minutes = ((int)initialTimeInSeconds / 60).ToString();
        string seconds = ((int)initialTimeInSeconds % 60).ToString();

        string zeroPlaceHolder;
        if (seconds.Length < 2)
            zeroPlaceHolder = "0";
        else
            zeroPlaceHolder = "";

        textObject.GetComponent<Text>().text = minutes + ":" + zeroPlaceHolder + seconds;
    }

    void Count()
    {
        initialTimeInSeconds += Time.deltaTime;

        string minutes = ((int)initialTimeInSeconds / 60).ToString();
        string seconds = ((int)initialTimeInSeconds % 60).ToString();

        string zeroPlaceHolder;
        if (seconds.Length < 2)
            zeroPlaceHolder = "0";
        else
            zeroPlaceHolder = "";

        textObject.GetComponent<Text>().text = minutes + ":" + zeroPlaceHolder + seconds;
    }
}


