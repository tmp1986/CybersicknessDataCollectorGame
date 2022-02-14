namespace TurnTheGameOn.IKDriver
{
    using UnityEngine;
    using UnityEngine.Events;

    [System.Serializable]
    public struct IKD_OnReachWaypointSettings
    {
        public IKD_WaypointRoute parentRoute;
        public int waypointIndexnumber;
        public bool useSpeedLimit;
        public float speedLimit;
        public bool useBrakeTrigger;
        public bool releaseBrakeWhenStopped;
        [Range(0.01f, 1f)] public float brakeAmount;
        [Range(0.01f, 1f)] public float AISpeedFactor;
        [Range(0, 1)] public float caution;
        public bool stopDriving;
        public float distanceFromStart;
        public IKD_Waypoint[] newRoutePoints;
        public UnityEvent OnReachWaypointEvent;
    }
}