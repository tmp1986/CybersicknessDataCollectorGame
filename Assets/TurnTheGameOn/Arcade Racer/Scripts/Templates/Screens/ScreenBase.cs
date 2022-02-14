namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class ScreenBase : MonoBehaviour
    {
        public CanvasGroup canvasGroup;
        public GameObject screenObject;
        public GameObject firstButtonSelection;

        public delegate void OnOpen();
        public event OnOpen OnOpenCallback;
        public void Open()
        {
            canvasGroup.interactable = true;
            screenObject.SetActive(true);
            SetFirstSelection();
            if (OnOpenCallback != null) OnOpenCallback();
        }

        public delegate void OnClose();
        public event OnClose OnCloseCallback;
        public void Close()
        {
            canvasGroup.interactable = false;
            screenObject.SetActive(false);
            if (OnCloseCallback != null) OnCloseCallback();
        }

        public delegate void OnSetFirstSelection();
        public event OnSetFirstSelection OnSetFirstSelectionCallback;
        public void SetFirstSelection()
        {
            
            UIControllerInputManager.Instance.SetNewSelection(firstButtonSelection);
            if (OnSetFirstSelectionCallback != null) OnSetFirstSelectionCallback();
        }
    }
}