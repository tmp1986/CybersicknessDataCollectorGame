using UnityEngine;

public static class GameData
{

    #region AudioState
    public static string GetAudioState()
    {
        return PlayerPrefs.GetString("AudioState", DefaultGameData.AudioState);
    }
    public static void SetAudioState(string newValue)
    {
        PlayerPrefs.SetString("AudioState", newValue);
    }
    #endregion

    #region QualitySettingsLevel
    public static int GetQualitySettingsLevel()
    {
        return PlayerPrefs.GetInt("QualitySettingsLevel", DefaultGameData.QualitySettingsLevel);
    }
    public static void SetQualitySettingsLevel(int newValue)
    {
        PlayerPrefs.SetInt("QualitySettingsLevel", newValue);
    }
    #endregion

    #region ControllerType
    public static string GetControllerType()
    {
        return PlayerPrefs.GetString("ControllerType", DefaultGameData.ControllerType);
    }
    public static void SetControllerType(string newValue)
    {
        PlayerPrefs.SetString("ControllerType", newValue);
    }
    #endregion

    #region AutomaticTransmissionState
    public static string GetAutomaticTransmissionState()
    {
        return PlayerPrefs.GetString("AutomaticTransmissionState", DefaultGameData.AutomaticTransmissionState);
    }
    public static void SetAutomaticTransmissionState(string newValue)
    {
        PlayerPrefs.SetString("AutomaticTransmissionState", newValue);
    }
    #endregion

    #region RearViewMirrorState
    public static string GetRearViewMirrorState()
    {
        return PlayerPrefs.GetString("RearViewMirrorState", DefaultGameData.RearViewMirrorState);
    }
    public static void SetRearViewMirrorState(string newValue)
    {
        PlayerPrefs.SetString("RearViewMirrorState", newValue);
    }
    #endregion

    #region FixedMiniMapRotationState
    public static string GetFixedMiniMapRotationState()
    {
        return PlayerPrefs.GetString("FixedMiniMapRotationState", DefaultGameData.FixedMiniMapRotationState);
    }
    public static void SetFixedMiniMapRotationState(string newValue)
    {
        PlayerPrefs.SetString("FixedMiniMapRotationState", newValue);
    }
    #endregion

    #region MiniMapState
    public static string GetMiniMapState()
    {
        return PlayerPrefs.GetString("MiniMapState", DefaultGameData.MiniMapState);
    }
    public static void SetMiniMapState(string newValue)
    {
        PlayerPrefs.SetString("MiniMapState", newValue);
    }
    #endregion

}