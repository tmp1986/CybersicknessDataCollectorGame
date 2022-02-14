namespace TurnTheGameOn.IKDriver
{
    using UnityEngine;

    [CreateAssetMenu(fileName = "IKD_AISettings", menuName = "TurnTheGameOn/IK Driver/IKD_AISettings")]
    public class IKD_AISettings : ScriptableObject
    {
        public RouteProgressType routeProgressType;
        public float changeLaneCooldown = 3.0f;
        public float changeLaneDistance = 4.0f;
        public IKD_VehicleInputSensitivity inputSensitivity;
        public bool useWaypointReset = true;
        public bool useStuckReset = true;
        public float waypointReset = 60.0f;
        public float stuckReset = 5.0f;

        #region SensorSettings
        public bool enableSensors = true;
        public float updateInterval = 0.03f;
        public LayerMask detectionLayers;
        [Header("Height")]
        [Range(0, 10)] public float sensorHeight = 1.0f;
        [Header("Width")]
        [Range(0, 3)] public float sensorFCenterWidth = 0.35f;
        [Range(0, 3)] public float sensorFSideWidth = 0.45f;
        [Range(0, 10)] public float sensorLRWidth = 1.75f;
        [Header("Length")]
        [Range(0, 30)] public float sensorFCenterLength = 14.0f;
        [Range(0, 30)] public float sensorFSideLength = 17.5f;
        [Range(0, 10)] public float sensorLRLength = 3.0f;
        #endregion

        #region CautionSettings
        [Range(0, 180)] public float cautionMaxAngle = 162.0f;
        [Range(0, 1)] public float cautionAngularVelocityFactor = 1.0f;
        public float minCautionDefault;
        public float minCautionIfGoToPitStopDefault = 0.4f;
        public float pitLaneCautionDistanceThreshold = 20.0f;
        public float autoCautionDistanceThreshold = 20.0f;
        public float cautionSlowSpeed = 0.2f;
        #endregion

        #region LookAheadSettings
        public float lookAheadMin = 1.0f;
        public float lookAheadMax = 1.0f;
        #endregion

        public bool useWaypointDistanceThreshold;
        public float waypointDistanceThreshold = 4.0f;
    }
}