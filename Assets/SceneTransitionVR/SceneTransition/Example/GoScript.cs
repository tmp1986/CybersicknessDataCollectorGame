using UnityEngine;
using System.Collections;
using ThiagoMalheiros.SceneTransition;

namespace ThiagoMalheiros.SceneTransition.Example
{
    public class GoScript : MonoBehaviour
    {
        public string ToScene;

        public void GoToNextScene()
        {
            SceneLoader.LoadScene(ToScene);
        }
    }
}
