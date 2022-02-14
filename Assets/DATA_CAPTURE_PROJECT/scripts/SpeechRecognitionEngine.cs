using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Windows.Speech;

public class SpeechRecognitionEngine : MonoBehaviour
{
    public string[] keywords = new string[] { "0", "1", "2", "3","4","5" };
    public ConfidenceLevel confidence = ConfidenceLevel.Medium;
    public float speed = 1;

    public static string results = "0";
    //public Image target;

    protected PhraseRecognizer recognizer;
    protected string word = "";

    private void Start()
    {
        results = "0";

        if (keywords != null)
        {
            recognizer = new KeywordRecognizer(keywords, confidence);
            recognizer.OnPhraseRecognized += Recognizer_OnPhraseRecognized;
            recognizer.Start();
        }
    }

    private void Recognizer_OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        word = args.text;
        results = word;

       // DATA_Manager.discomfortLevel = Convert.ToInt32(results);
    }

    private void Update()
    {
        //var x = target.transform.position.x;
        //var y = target.transform.position.y;

        //switch (word)
        //{
        //    case "up":
        //        y += speed;
        //        break;
        //    case "down":
        //        y -= speed;
        //        break;
        //    case "left":
        //        x -= speed;
        //        break;
        //    case "right":
        //        x += speed;
        //        break;
        //}

        //target.transform.position = new Vector3(x, y, 0);
    }

    private void OnApplicationQuit()
    {
        if (recognizer != null && recognizer.IsRunning)
        {
            recognizer.OnPhraseRecognized -= Recognizer_OnPhraseRecognized;
            recognizer.Stop();
        }
    }
}
