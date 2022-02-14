using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKD_MaterialManager : MonoBehaviour
{
    public static IKD_MaterialManager Instance = null;
    public Material[] getmaterial;
    int materialIndex;

    void OnEnable()
    {
        if (Instance == null)
        {
            Instance = this;
            materialIndex = -1;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public Material GetMaterial()
    {
        materialIndex = materialIndex < getmaterial.Length - 1 ? materialIndex + 1 : 0;
        return getmaterial[materialIndex];
    }
}
