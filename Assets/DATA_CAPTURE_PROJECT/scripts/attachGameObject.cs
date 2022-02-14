using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attachGameObject : MonoBehaviour
{

    public Camera mainCamera;
    public GameObject raycasterPrefab;
    // Start is called before the first frame update
    void Awake()
    {
        GameObject raycaster = Instantiate(raycasterPrefab,mainCamera.transform);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
