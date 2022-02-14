using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class QA_Manager : MonoBehaviour
{
   // public Color color;
    public string genere;
    public string age;
    public string experience;
    public string symptoms;
    public string glassesUse;
    public string visionProblems;
    public string flicker;
    public string eyeDominance;
    public string userPosture;

    public List<GameObject> questionsGoList = new List<GameObject>();


    public string NextLevelName = "";


    // Start is called before the first frame update
    void Start()
    {

        NextLevelName = PlayerPrefs.GetString("GameType");

        foreach (GameObject q in questionsGoList)
        {
            q.gameObject.SetActive(false);
        }

        questionsGoList[0].SetActive(true);
        

    }

    // Update is called once per frame
    void Update()
    {
       
      //  GL.Clear(true, true, color);
    }


    public void SetGenere(string g)
    {
        genere = g;
    }

    public void questao_respondida(GameObject g)
    {
        GL.Clear(true, true, Camera.main.backgroundColor);
        string qNum = g.name.Substring(0, 2).Replace("Q","");
        // string opNum = g.name.Split('p')[1];
        GameObject text = g.transform.GetChild(0).gameObject;

        bool nextScene = false;

        GL.Clear(true,true,Color.black);

        if (qNum != "X")
        {
            if (System.Convert.ToInt16(qNum) == 1)
            {
                genere = text.GetComponent<TextMeshProUGUI>().text;
            }

            if (System.Convert.ToInt16(qNum) == 2)
            {
                age = text.GetComponent<TextMeshProUGUI>().text;
            }

            if (System.Convert.ToInt16(qNum) == 3)
            {
                experience = text.GetComponent<TextMeshProUGUI>().text;
            }

            if (System.Convert.ToInt16(qNum) == 4)
            {
                symptoms = text.GetComponent<TextMeshProUGUI>().text;
            }

            if (System.Convert.ToInt16(qNum) == 5)
            {
                glassesUse = text.GetComponent<TextMeshProUGUI>().text;
            }

            if (System.Convert.ToInt16(qNum) == 6)
            {
                visionProblems = text.GetComponent<TextMeshProUGUI>().text;
            }

            if (System.Convert.ToInt16(qNum) == 7)
            {
                flicker = text.GetComponent<TextMeshProUGUI>().text;
            }

            if (System.Convert.ToInt16(qNum) == 8)
            {
                eyeDominance = text.GetComponent<TextMeshProUGUI>().text;
            }

            if (System.Convert.ToInt16(qNum) == 9)
            {
                userPosture = text.GetComponent<TextMeshProUGUI>().text;
            }

            for (int i = 0; i < questionsGoList.Count; i++)
            {
                if (i != System.Convert.ToInt16(qNum))
                {
                    questionsGoList[i].SetActive(false);
                }
                else
                {
                    questionsGoList[i].SetActive(true);
                }
            }
        }
        else
        {
            nextScene = true;
        }

      


        if (nextScene)
        {
            PlayerPrefs.SetString("genere", genere);
            PlayerPrefs.SetString("age", age);
            PlayerPrefs.SetString("experience", experience);
            PlayerPrefs.SetString("symptoms", symptoms);
            PlayerPrefs.SetString("glassesUse", glassesUse);
            PlayerPrefs.SetString("visionProblems", visionProblems);
            PlayerPrefs.SetString("flicker", flicker);
            PlayerPrefs.SetString("eyeDominance", eyeDominance);
            PlayerPrefs.SetString("userPosture", userPosture);

            ThiagoMalheiros.SceneTransition.SceneLoader.LoadScene("Manual_"+ NextLevelName);
            //SceneManager.LoadScene(sceneToStart.handle);
        }

    }
}
