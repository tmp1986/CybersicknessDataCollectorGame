using UnityEngine;
using ThiagoMalheiros.SceneTransition;

public class ChangeSceneTrigger : MonoBehaviour
{
    public string ChangeToScene;
    public GameObject WhenTriggeredBy;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == WhenTriggeredBy)
        {
            SceneLoader.LoadScene(ChangeToScene);
        }
    }
}
