using UnityEngine;
using TurnTheGameOn.IKDriver;

[RequireComponent(typeof(IKDVC_DriveSystem))]
public class AutomaticTransmission : MonoBehaviour
{
    public static AutomaticTransmission Instance = null;
    private IKDVC_DriveSystem driveSystem;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogWarning("Multiple Transmission instances assigned, this is not allowed.");
        }
    }

    private void OnEnable()
    {
        driveSystem = GetComponent<IKDVC_DriveSystem>();
        SetAutomaticTransmissionState();
    }

    public void SetAutomaticTransmissionState()
    {
        bool automaticTransmission = GameData.GetAutomaticTransmissionState() == "ON" ? true : false;
        driveSystem.vehicleSettings.manual = !automaticTransmission;
    }
}