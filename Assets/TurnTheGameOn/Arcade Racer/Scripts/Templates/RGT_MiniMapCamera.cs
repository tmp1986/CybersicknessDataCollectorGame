namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class RGT_MiniMapCamera : MonoBehaviour
    {
        public Camera miniMapCamera { get; private set; }
        public bool fixedRotation;
        public Transform target;
        public Vector3 offset = new Vector3(0f, 7.5f, 0f);

        private void OnEnable()
        {
            miniMapCamera = GetComponent<Camera>();
        }

        void Start()
        {
            if (target == null)
            {
                Debug.LogWarning("MiniMapCamera target is not set, this script will be disabled.");
                enabled = false;
            }
            //fixedRotation = RGT_PlayerPrefs.playableVehicles.fixedMiniMapRotation;
            transform.parent = null;
        }

        private void LateUpdate()
        {
            if (fixedRotation)
            {
                transform.position = target.position + offset;
                transform.LookAt(target);
            }
            else
            {
                transform.eulerAngles = new Vector3(90, target.rotation.eulerAngles.y, 0);
                transform.position = target.position + offset;
            }
        }

    }
}