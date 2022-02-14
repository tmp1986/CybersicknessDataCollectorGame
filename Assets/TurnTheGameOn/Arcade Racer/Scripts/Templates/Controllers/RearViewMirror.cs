using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearViewMirror : MonoBehaviour
{
    public static RearViewMirror Instance = null;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Debug.LogWarning("Multiple RearViewMirror instances assigned, this is not allowed.");
        }
    }

    private void OnEnable()
    {
        SetMirrorState();
    }

    public void SetMirrorState()
    {
        bool setActive = GameData.GetRearViewMirrorState() == "ON" ? true : false;
        gameObject.SetActive(setActive);
    }
}
