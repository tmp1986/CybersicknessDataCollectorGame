using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowUserData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<UnityEngine.UI.Text>().text =
             "genere: " + PlayerPrefs.GetString("genere") + "\n" +
             "age: " + PlayerPrefs.GetString("age") + "\n" +
             "experience: " + PlayerPrefs.GetString("experience") + "\n" +
             "symptoms: " + PlayerPrefs.GetString("symptoms") + "\n" +
             "flicker: " + PlayerPrefs.GetString("flicker") + "\n" +
             "eyeDominance: " + PlayerPrefs.GetString("eyeDominance") + "\n";


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
