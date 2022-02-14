using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using TMPro;
using System;

public class CyberSicknessRateCalculateToSVR : MonoBehaviour
{

    public TextMeshProUGUI cstext;
    // Start is called before the first frame update
    void Start()
    {
      readLogNode(PlayerPrefs.GetString("xmldir"));
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void readLogNode(string xmlPath)
    {
        XmlDocument xml = new XmlDocument();

        try
        {
            xml.Load(xmlPath); // suppose that myXmlString contains "<Names>...</Names>"
        }
        catch(Exception ex)
        {
            print(ex);
        }


        XmlNodeList UserGenere = xml.SelectNodes("/USERDATA/Data/UserGenere");
        XmlNodeList UserAge = xml.SelectNodes("/USERDATA/Data/UserAge");
        XmlNodeList UserExperience = xml.SelectNodes("/USERDATA/Data/UserExperience");
        XmlNodeList UserSymptoms = xml.SelectNodes("/USERDATA/Data/UserSymptoms");
  

        XmlNodeList xnList = xml.SelectNodes("/USERDATA/Data/DiscomfortLevel");

        float pesoUserGenere = 0;
        float pesoUserAge = 0;
        float pesoUserExperience = 0;
        float pesoUserSymptoms = 0;

        if (UserGenere[0].InnerText.ToUpper() == "FEMININO")
            pesoUserGenere = 0.25f;
        else
            pesoUserGenere = 0.15f;

        if (UserAge[0].InnerText == "18 A 35")
            pesoUserAge = 0.1f;
        if (UserAge[0].InnerText == "36 A 50")
            pesoUserAge = 0.15f;
        if (UserAge[0].InnerText == "+50")
            pesoUserAge = 0.25f;

        if (UserExperience[0].InnerText == "NENHUMA")
            pesoUserExperience = 0.25f;
        if (UserExperience[0].InnerText == "ALGUMA")
            pesoUserExperience = 0.15f;
        if (UserExperience[0].InnerText == "MUITA")
            pesoUserExperience = 0.1f;

        if (UserSymptoms[0].InnerText == "NENHUM")
            pesoUserSymptoms = 0.1f;
        if (UserSymptoms[0].InnerText == "ALGUM")
            pesoUserSymptoms = 0.15f;
        if (UserSymptoms[0].InnerText == "MUITO")
            pesoUserSymptoms = 0.25f;


        float soma = 0;
        float count = xnList.Count; 
        foreach (XmlNode xn in xnList)
        {
            soma = soma + XmlConvert.ToInt32(xn.InnerText);
           
        }

        float  media = soma / count;

        
        float v = ((media * pesoUserGenere) + (media * pesoUserAge) + (media * pesoUserExperience) + (media * pesoUserSymptoms)) * 100;
        cstext.text = "CYBERSICKNESS\n" + (Math.Round(v, 2)).ToString() + "%";
    }
}
