namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class QuitGameTriggers : MonoBehaviour
    {
        public void Button_Quit()
        {
            if (StartScreen.Instance) StartScreen.Instance.canvasGroup.interactable = false;
            if (PauseScreen.Instance) PauseScreen.Instance.canvasGroup.interactable = false;
            gameObject.SetActive(false);
            gameObject.SetActive(true);
            QuitConfirmScreen.Instance.Open();
        }
    }
}