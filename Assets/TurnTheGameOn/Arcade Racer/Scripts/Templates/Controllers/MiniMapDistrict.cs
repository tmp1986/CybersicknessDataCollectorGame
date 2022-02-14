namespace TurnTheGameOn.ArcadeRacer
{
    using UnityEngine;

    public class MiniMapDistrict : MonoBehaviour
    {
        public string districtName;
        public string detectionLayer = "Player";
        void OnTriggerEnter(Collider collisionInfo)
        {
            if (RGT_MiniMapCanvas.Instance != null)
            {
                if (collisionInfo.gameObject.layer == LayerMask.NameToLayer(detectionLayer))
                {
                    RGT_MiniMapCanvas.Instance.SetDistrict(districtName);
                }
            }
        }

    }
}