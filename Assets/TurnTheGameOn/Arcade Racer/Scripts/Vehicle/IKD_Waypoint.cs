namespace TurnTheGameOn.IKDriver
{
    using UnityEngine;

    [ExecuteInEditMode]
    public class IKD_Waypoint : MonoBehaviour
    {
        public IKD_WaypointRoute waypointRoute;
        public int waypointNumber;
        public IKD_OnReachWaypointSettings onReachWaypointSettings;
        public IKD_Waypoint[] junctionPoint;
        public IKD_Waypoint pitLane;
        public IKD_Waypoint pitArea;
        public float distanceFromStartLoopEnd;

        private void OnEnable()
        {
            onReachWaypointSettings.waypointIndexnumber = waypointNumber;
        }

        void OnTriggerEnter(Collider col)
        {
            col.transform.root.SendMessage("OnReachedWaypoint", onReachWaypointSettings, SendMessageOptions.DontRequireReceiver);
            if (waypointNumber == waypointRoute.waypointDataList.Count)
            {
                if (onReachWaypointSettings.newRoutePoints.Length == 0)
                {
                    col.transform.root.SendMessage("StopDriving", SendMessageOptions.DontRequireReceiver);
                }
            }
        }

        public void TriggerNextWaypoint(IKDVC_AI _IKDVC_AI)
        {
            _IKDVC_AI.OnReachedWaypoint(onReachWaypointSettings);
            if (waypointNumber == waypointRoute.waypointDataList.Count)
            {
                if (onReachWaypointSettings.newRoutePoints.Length == 0)
                {
                    _IKDVC_AI.StopDriving();
                }
            }
        }
    }

}