using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ThiagoMalheiros.SceneTransition.Example
{
    public class SettingsScript : MonoBehaviour
    {
        public Toggle VRSupport;
        public Toggle UseSceneForLoadingScreen;
        public Toggle FadeInLoadingScreen;
        public Toggle FadeOutLoadingScreen;
        public InputField FadeSeconds;
        public InputField MinimumLoadingScreenSeconds;
        public InputField FadeColor;

        private SceneLoader SceneLoaderInstance;

    	// Use this for initialization
    	void Start ()
        {
            SceneLoaderInstance = SceneLoader.Instance;
            VRSupport.isOn = SceneLoaderInstance.VRMode;
            UseSceneForLoadingScreen.isOn = SceneLoaderInstance.UseSceneForLoadingScreen;
            FadeInLoadingScreen.isOn = SceneLoaderInstance.FadeInLoadingScreen;
            FadeOutLoadingScreen.isOn = SceneLoaderInstance.FadeOutLoadingScreen;
            FadeSeconds.text = SceneLoaderInstance.FadeSeconds.ToString();
            MinimumLoadingScreenSeconds.text = SceneLoaderInstance.MinimumLoadingScreenSeconds.ToString();
            FadeColor.text = ColorToHex(SceneLoaderInstance.FadeColor);

            UpdateVisibleSettings();
        }

        public void ClickVRSupport()
        {
            SceneLoaderInstance.VRMode = VRSupport.isOn;

            UpdateVisibleSettings();
        }

        public void ClickUseSceneForLoadingScreen()
        {
            SceneLoaderInstance.UseSceneForLoadingScreen = UseSceneForLoadingScreen.isOn;

            UpdateVisibleSettings();
        }

        public void ClickFadeInLoadingScreen()
        {
            SceneLoaderInstance.FadeInLoadingScreen = FadeInLoadingScreen.isOn;
        }

        public void ClickFadeOutLoadingScreen()
        {
            SceneLoaderInstance.FadeOutLoadingScreen = FadeOutLoadingScreen.isOn;

            UpdateVisibleSettings();
        }

        public void ChangeFadeSeconds()
        {
            float.TryParse(FadeSeconds.text, out SceneLoaderInstance.FadeSeconds);
        }

        public void ChangeMinimumLoadingScreenSeconds()
        {
            float.TryParse(MinimumLoadingScreenSeconds.text, out SceneLoaderInstance.MinimumLoadingScreenSeconds);
        }

        public void ChangeFadeColor()
        {
            SceneLoaderInstance.FadeColor = HexToColor(FadeColor.text);
        }
    	
        private string ColorToHex(Color32 color)
        {
            string hex = color.r.ToString("X2") + color.g.ToString("X2") + color.b.ToString("X2");
            return hex;
        }

        private Color HexToColor(string hex)
        {
            if (hex.Length != 6)
            {
                return Color.clear;
            }
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            return new Color32(r, g, b, 255);
        }

        private void UpdateVisibleSettings()
        {
            var loadingScreen = !SceneLoaderInstance.VRMode || SceneLoaderInstance.UseSceneForLoadingScreen;

            MinimumLoadingScreenSeconds.transform.parent.gameObject.SetActive(loadingScreen);
            FadeInLoadingScreen.gameObject.SetActive(loadingScreen);
            FadeOutLoadingScreen.gameObject.SetActive(loadingScreen);
        }
    }
}
