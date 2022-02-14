using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class EndScreen_Manager : MonoBehaviour
{
    public Color color;
    public TextMeshProUGUI topScoreName = null;
    public TextMeshProUGUI topScore;
    public TextMeshProUGUI yourScore;
    public TMP_InputField inputNewRecordName;



    // Start is called before the first frame update
    void Start()
    {
        if (topScoreName != null) // diferente de null eh pq entrou na cena end
        {
            topScoreName.text = "" + PlayerPrefs.GetString("topScoreName");
            topScore.text = "" + PlayerPrefs.GetInt("topscore");
            yourScore.text = "" + PlayerPrefs.GetInt("score");
        }
    }

    // Update is called once per frame
    void Update()
    {
       // GL.Clear(true, true, color);
    }


    public void OnClick() //onclick da cena end
    {
        PlayerPrefs.SetInt("score", 0);
        ThiagoMalheiros.SceneTransition.SceneLoader.LoadScene("PreGame");
       
        //SceneManager.LoadScene(0);
    }

    public void NewInputRecordButton() //inout score scene
    {
        PlayerPrefs.SetString("topScoreName", inputNewRecordName.text);
        ThiagoMalheiros.SceneTransition.SceneLoader.LoadScene("EndGame");
    }
}
