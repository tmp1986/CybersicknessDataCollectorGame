namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class OptionsScreenTriggers : MonoBehaviour
    {
        public void OpenOptionsScreen()
        {
            OptionsScreen.Instance.Open();
        }

        public void CloseOptionsScreen()
        {
            OptionsScreen.Instance.Open();
        }
    }
}