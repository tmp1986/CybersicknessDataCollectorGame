namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;
    using UnityEngine.UI;
    using System.Collections;

    public class RGT_MiniMapObject : MonoBehaviour
    {
        public Image miniMapIconPrefab;
        private RGT_MiniMapCanvas miniMapCanvas;

        void Start()
        {
            StartCoroutine(RegisterMiniMapIcon());
            //miniMapCanvas.RegisterMiniMapIcon(gameObject, miniMapIconPrefab, false);
        }

        private void OnDestroy()
        {
            miniMapCanvas.RemoveMiniMapIcon(gameObject);
        }

        IEnumerator RegisterMiniMapIcon()
        {
            while (miniMapCanvas == null)
            {
                miniMapCanvas = FindObjectOfType<RGT_MiniMapCanvas>();
                if (miniMapCanvas == null)
                {
                    yield return new WaitForSeconds(0.1f);
                }
            }
            miniMapCanvas.RegisterMiniMapIcon(gameObject, miniMapIconPrefab, false);
        }

    }
}