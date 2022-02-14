using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using PorcinoTM_CSPredictor;
using SkyFlightGame;

public class DATA_Manager : MonoBehaviour
{
    [Header("------ 0. INITIAL CONFIGURATION -----")]
    public string findGO = "IKD_VehicleController_LeftSteering";
    public GameObject playerGO;
    public new Camera camera;

    [Header("------ 1. USER PROFILE DATA -----")]
    //novos
    public string genere;
    public string age;
    public string experience;
    public string symptoms;
    public string glassesUse;
    public string visionProblems;
    public string flicker;
    public string eyeDominance;
    public string userPosture;

    [Header("------2. GAME SETTINGS - MANUAL-----")]

    //novos
    public string gameType = "Race";
    public bool staticFrame = false;
    public bool hapticFeedback = false;
    public string degreeOfControl = "total";
    public bool dofSimulation = false;
    public bool locomotion = false;
    public bool cameraAutoMovement = false;
   
   
    [Header("------2B. GAMEPLAY DATA - AUTO -----")]
    //novos
    public float timeStamp;
    private float lastCalculatedSpeed = 0.0f;
    public float playerSpeed = 0.0f;
    public float playerAcceleration = 0;
    public float[] cameraRotation = new float[3];
    public string regionOfInterest = "foreground";
    public float cameraFieldOfView;
    public float gameFps;


 
    [Header("------2C. DISCOMFORT LEVEL (1-5) -----")]
    public int discomfortLevel = 0;

    [Header("------3. EXPORTING DATA (Seconds) -----")]
    private string dirToExport;
    public int ExprtDataDelaySeconds = 1;





    public List<UnityEngine.Texture> discomfortLevelsImg;



    public float totalTimeBeforeGame = 0;

    private GameObject carSpeedDashboardGO;

    public GameObject discomfortPanel;

    public bool birdGame = false;

    public PorcinoTM_CSPredictor.DataManager dManager = new DataManager();


    bool firstTimeLoadEnd = false;
    private void Awake()
    {
        UnityEngine.XR.InputTracking.disablePositionalTracking = true;
        dirToExport = Application.dataPath + "\\..\\_DATA_\\DATA_EXPORT_"+gameType+"_"+ DateTime.Now.Year+"_" + DateTime.Now.Month +"_"+ DateTime.Now.Day + "_" + DateTime.Now.Hour +"_"+ DateTime.Now.Minute +"_"+ DateTime.Now.Second +"\\";
        if (!Directory.Exists(dirToExport))
            Directory.CreateDirectory(dirToExport);
    }

    
    // Use this for initialization
    void Start ()
    {
        if (playerGO == null)
            playerGO = GameObject.Find(findGO);
        if (discomfortPanel == null)
        {
            discomfortPanel = GameObject.FindGameObjectWithTag("DISCOMFORT_IMG");
        }
        discomfortLevel = 0;
        discomfortPanel.GetComponent<UnityEngine.UI.RawImage>().texture = discomfortLevelsImg[0];
        if (camera == null)
            camera = GameObject.Find("Helmet Camera Rig").GetComponent<Camera>();

        if (carSpeedDashboardGO == null)
        {
            carSpeedDashboardGO = GameObject.FindGameObjectWithTag("CarVelocity_Manager");
        }


        //Set Bio Data to Unity public variables 
        SetBioDataFromQA();

        //Input Profile Data
        dManager.InputUserProfileData(genere, age, experience, symptoms, flicker,glassesUse,visionProblems, userPosture, eyeDominance);
        dManager.InputGameSettingsData(staticFrame, hapticFeedback, degreeOfControl, dofSimulation, locomotion, cameraAutoMovement);
        
        InvokeRepeating("CallExport", 0, ExprtDataDelaySeconds);
        UnityEngine.XR.InputTracking.Recenter();
    }



    void CallExport()
    {
        CalculateTimeStamp();
        CalculateSpeed();
        CalculateCarAccelerationKPH();
        CalculateCameraRotation(camera);
        CalculateRegionOfInterest();
        CalculateFieldOfView(camera);
        CalculateGameFps();

        //verify if is the training mode or predicting mode, turn off on predicting mode
        CalculateDiscomfortLevel();

        dManager.InputGameUserInput(discomfortLevel);
        dManager.InputGamePlayData(timeStamp, playerSpeed, playerAcceleration, cameraRotation, regionOfInterest, cameraFieldOfView,gameFps);

        dManager.ExportDataSetXML(dirToExport);
        PlayerPrefs.SetString("xmldir", dirToExport + "\\FILE.XML");
    }



    private void SetBioDataFromQA()
    {
        genere = PlayerPrefs.GetString("genere");
        age = PlayerPrefs.GetString("age");
        experience = PlayerPrefs.GetString("experience");
        symptoms = PlayerPrefs.GetString("symptoms");
        flicker = PlayerPrefs.GetString("flicker");
        glassesUse = PlayerPrefs.GetString("glassesUse");
        visionProblems = PlayerPrefs.GetString("visionProblems");
        eyeDominance = PlayerPrefs.GetString("eyeDominance");
        userPosture = PlayerPrefs.GetString("userPosture");
    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            UnityEngine.XR.InputTracking.Recenter();
        }

        if (Input.GetKeyUp(KeyCode.E) || timeStamp >= 300)
        {
            
            int s = PlayerPrefs.GetInt("score");
            int ts = PlayerPrefs.GetInt("topscore");

            if (s > ts)
            {
                if (firstTimeLoadEnd == false)
                {
                    firstTimeLoadEnd = true;
                    PlayerPrefs.SetInt("topscore", s);
                    ThiagoMalheiros.SceneTransition.SceneLoader.LoadScene("Retirar_Hmd_NewRecordInputName");
                }
            }
            else
            {
                if (firstTimeLoadEnd == false)
                    ThiagoMalheiros.SceneTransition.SceneLoader.LoadScene("Retirar_Hmd_EndGame");
            }
        }
        CalculateDiscomfortLevel();
    }




    #region Collect Data from Scene Methods 


    private void CalculateTimeStamp()
    {
        timeStamp = TimerCountDown.initialTimeInSeconds; 
    }

    //Method to get the current speed Value in KPH
    private void  CalculateSpeed()
    {
        if (birdGame == false)
            playerSpeed = float.Parse(carSpeedDashboardGO.GetComponent<UnityEngine.UI.Text>().text);
        else
            playerSpeed = playerGO.GetComponent<SFGPlayerControls>().moveSpeed;
    }

    //Method to get the Acceleration Value in KPH
    private void CalculateCarAccelerationKPH()
    {
        float speed = playerSpeed;
        float acceleration = speed - lastCalculatedSpeed;
        playerAcceleration = acceleration;
        lastCalculatedSpeed = speed;
    }

    private void CalculateRegionOfInterest()
    {
        float d = Convert.ToInt32(rayCasterDistanceHit.hitDistanceMeters);
        if (d < 10)
        {
            regionOfInterest = "foreground";
        }
        if (d > 10 && d < 25)
        {
            regionOfInterest = "middleground";
        }
        if (d > 25)
        {
            regionOfInterest = "background";
        }
    }

    private void CalculateFieldOfView(Camera c)
    {
        cameraFieldOfView = c.fieldOfView;
    }

    private void CalculateCameraRotation(Camera c)
    {
        cameraRotation[0] = c.transform.rotation.x;
        cameraRotation[1] = c.transform.rotation.y;
        cameraRotation[2] = c.transform.rotation.z;
    }

    private void CalculateGameFps()
    {
            gameFps = FPSCounter.m_CurrentFps;

    }


    private void CalculateDiscomfortLevel()
    {
        discomfortLevel = Convert.ToInt32(SpeechRecognitionEngine.results);
        discomfortPanel.GetComponent<UnityEngine.UI.RawImage>().texture = discomfortLevelsImg[discomfortLevel];
    }
    #endregion


}
