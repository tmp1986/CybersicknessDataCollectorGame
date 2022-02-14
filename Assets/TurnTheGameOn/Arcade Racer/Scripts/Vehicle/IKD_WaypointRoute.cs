namespace TurnTheGameOn.IKDriver
{
    using UnityEngine;
    using System.Collections.Generic;
    [System.Serializable]
    public class IKD_WaypointRoute : MonoBehaviour
    {
        public WaypointData[] waypointData;
        public List<WaypointData> waypointDataList;

        [ContextMenu("ConvertWaypointArrayToList")]
        public void ConvertWaypointArrayToList()
        {
            waypointDataList = new List<WaypointData>(waypointData);
        }

        public int routeIndex;
        public PitStopLaneType pitStopLaneType;
        public int pitAreaIndex;
        public Color pathColor = new Color(0, 1, 0, 0.298f), selectedPathColor = new Color(0, 1, 0, 1); // gizmo colors
        public Color junctionColor = new Color(1, 1, 0, 0.298f), selectedJunctionColor = new Color(1, 1, 0, 1);
        public Color pitJunctionColor = new Color(1, 0, 0, 0.298f), selectedPitJunctionColor = new Color(1, 0, 0, 1);
        public float totalRouteDistance { get; private set; }
        public IKD_ColliderStayCheck pitAreaExitCheck;
        public bool stopForTrafficLight { get; private set; }
        public GameObject[] spawnTrafficVehicles;
        private GameObject spawnedTrafficVehicle;
        private Vector3 spawnPosition;

        #region Awake
        private void Awake()
        {
            for (int i = 0; i < waypointDataList.Count; i++)
            {
                waypointDataList[i]._transform.hasChanged = false;
            }
            if (waypointDataList.Count > 2) CachePositionsAndDistances(); // ensure route data is updated and cached
        }

        private void Start()
        {
            SpawnTrafficVehicles();
        }

        [HideInInspector] private Vector3 pointA, pointB; // temp variables - cached to prevent runtime gc
        [HideInInspector] private float accumulateDistance = 0; // temp variables - cached to prevent runtime gc
        private void CachePositionsAndDistances()
        {
                totalRouteDistance = waypointDataList[waypointDataList.Count - 1]._waypoint.onReachWaypointSettings.distanceFromStart;
            accumulateDistance = 0;
            for (int i = 1; i < waypointDataList.Count + 1; i++)
            {
                waypointDataList[i - 1]._waypoint.onReachWaypointSettings.parentRoute = this;
                pointA = waypointDataList[i - 1]._transform.position;
                if (i == 1) pointB = waypointDataList[i - 1]._transform.position;
                else pointB = (i < waypointDataList.Count) ? waypointDataList[i]._transform.position : waypointDataList[0]._transform.position;
                accumulateDistance += (pointA - pointB).magnitude;
                waypointDataList[i - 1]._waypoint.onReachWaypointSettings.distanceFromStart = accumulateDistance;
                waypointDataList[i - 1]._waypoint.onReachWaypointSettings.distanceFromStart = accumulateDistance;
                waypointDataList[i - 1]._waypoint.waypointRoute = this;
            }
        }
        #endregion

        #region Public Get Route Point and Position
        [HideInInspector] private Vector3 pointAA, pointBB, delta; // temp variables - cached to prevent runtime gc
        public RoutePoint GetRoutePoint(float _routeDistanceTraveled)
        {
            pointAA = GetRoutePosition(_routeDistanceTraveled); // position and direction
            pointBB = GetRoutePosition(_routeDistanceTraveled + 0.01f);
            delta = pointBB - pointAA;
            return new RoutePoint(pointAA, delta.normalized);
        }
        [HideInInspector] private int p0n, p1n, p2n, p3n, _point; // temp variable - cached to prevent runtime gc
        [HideInInspector] private Vector3 P0, P1, P2, P3; // temp variable - cached to prevent runtime gc
        [HideInInspector] private float distanceBetweenCurrentPoints, positionFromStart, _pointA, _pointB; // temp variables - cached to prevent runtime gc
        public Vector3 GetRoutePosition(float _routeDistanceTraveled)
        {
            _point = 0;
            positionFromStart = 0;
            if (_routeDistanceTraveled >= totalRouteDistance) _routeDistanceTraveled = Mathf.Repeat(_routeDistanceTraveled, totalRouteDistance);

            for (int i = 0; i < totalRouteDistance; i++)
            {
                if (waypointTransformsChanged) break;
                if (positionFromStart < _routeDistanceTraveled && waypointTransformsChanged == false)
                {

                    if (_point <= waypointDataList.Count - 1 && waypointDataList != null)
                    {
                        if (positionFromStart < _routeDistanceTraveled)
                        {
                            //	Debug.Log (_point);
                            ++_point;
                            positionFromStart = _point == 0 ? 0 : waypointDataList[_point - 1]._waypoint.onReachWaypointSettings.distanceFromStart;
                        }

                    }
                }
            }

            p1n = ((_point - 1) + waypointDataList.Count) % waypointDataList.Count; // get nearest two points, ensuring points wrap-around start & end of circuit
            p2n = _point;
            _pointA = _pointB = 0.0f;
            _pointA = p1n == 0 ? 0.0f : waypointDataList[p1n - 1]._waypoint.onReachWaypointSettings.distanceFromStart;

            //if (closeRoute)
            //{
                if (p2n > waypointDataList.Count) p2n = 1;
                else _pointB = p2n == 0 ? 0.0f : waypointDataList[p2n - 1]._waypoint.onReachWaypointSettings.distanceFromStart;
            //}
            //else
            //{
               // _pointB = p2n == 0 ? 0.0f : waypointDataList[p2n - 1]._waypoint.onReachWaypointSettings.distanceFromStart;
            //}
            distanceBetweenCurrentPoints = Mathf.InverseLerp(_pointA, _pointB, _routeDistanceTraveled);
            // smooth catmull-rom calculation between the two relevant points // get indices for the surrounding 2 points, because // four points are required by the catmull-rom function
            p0n = ((_point - 2) + waypointDataList.Count) % waypointDataList.Count;
            p3n = (_point + 1) % waypointDataList.Count;
            // 2nd point may have been the 'last' point - a dupe of the first, // (to give a value of max track distance instead of zero) // but now it must be wrapped back to zero if that was the case.
            p2n = p2n % waypointDataList.Count;
            if (waypointDataList[p0n]._transform != null) P0 = waypointDataList[p0n]._transform.position;
            if (waypointDataList[p1n]._transform != null) P1 = waypointDataList[p1n]._transform.position;
            if (waypointDataList[p2n]._transform != null) P2 = waypointDataList[p2n]._transform.position;
            if (waypointDataList[p3n]._transform != null) P3 = waypointDataList[p3n]._transform.position;
            //if (!closeRoute && P3 == waypointDataList[0]._transform.position) P3 = P2;
            return CatmullRom(P0, P1, P2, P3, distanceBetweenCurrentPoints);
        }

        private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float i)
        {
            return 0.5f * ((2 * p1) + (-p0 + p2) * i + (2 * p0 - 5 * p1 + 4 * p2 - p3) * i * i + (-p0 + 3 * p1 - 3 * p2 + p3) * i * i * i);
        }
        #endregion

        #region Traffic Control
        public void StopForTrafficlight(bool _stop)
        {
            stopForTrafficLight = _stop;
        }

        public void SpawnTrafficVehicles()
        {
            for (int i = 0, j = 0; i < spawnTrafficVehicles.Length && j < waypointDataList.Count - 1; i++, j++)
            {
                spawnPosition = waypointDataList[j]._transform.position;
                spawnPosition.y += 3;
                spawnedTrafficVehicle = Instantiate(spawnTrafficVehicles[i], spawnPosition, waypointDataList[j]._transform.rotation);
                spawnedTrafficVehicle.GetComponent<IKDVC_AI>().waypointRoute = this;
                spawnedTrafficVehicle.transform.LookAt(waypointDataList[j + 1]._transform);
                j += 1; // increase j again tospawn vehicles with more space between
            }
        }
        #endregion

        #region Unity Editor Helper Methods
        [HideInInspector] private GameObject newWaypoint;
        public void ClickToSpawnNextWaypoint(Vector3 _position)
        {
            newWaypoint = Instantiate(Resources.Load("IKD_Waypoint"), _position, Quaternion.identity, gameObject.transform) as GameObject;
            WaypointData newPoint = new WaypointData();
            newPoint._name = newWaypoint.name = "IKD_Waypoint " + (waypointDataList.Count + 1);
            newPoint._transform = newWaypoint.transform;
            newPoint._waypoint = newWaypoint.GetComponent<IKD_Waypoint>();
            newPoint._waypoint.waypointNumber = waypointDataList.Count + 1;
            waypointDataList.Add(newPoint);
        }

        public void SpawnPitAreaExitCollider()
        {
            GameObject newCollider = Instantiate(Resources.Load("IKD_PitAreaExitCollider"), transform.position, Quaternion.identity, gameObject.transform) as GameObject;
            newCollider.name = "IKD_PitAreaExitCollider";
            newCollider.transform.SetAsFirstSibling();
            pitAreaExitCheck = newCollider.GetComponent<IKD_ColliderStayCheck>();
        }
        #endregion

        #region Gizmos
        private void OnDrawGizmos() { if (alwaysDrawGizmos) DrawGizmos(false); }
        private void OnDrawGizmosSelected() { if (alwaysDrawGizmos) DrawGizmos(true); }
        public bool alwaysDrawGizmos = true;
        [HideInInspector] Transform arrowPointer;
        public float arrowHeadWidth = 10.0f;
        public float arrowHeadLength = 2.0f;
        private Vector3 arrowHeadScale = new Vector3(1, 0, 1);
        private Transform junctionPosition;
        private Matrix4x4 previousMatrix;
        private int lookAtIndex;
        public float updateGizmoCoolDown = 0.1f;
        private float updateGizmoTimer;
        private bool waypointTransformsChanged;
        public bool canUpdateGizmos;
        private void DrawGizmos(bool selected)
        {
            if (canUpdateGizmos)
            {
                for (int i = 0; i < waypointDataList.Count; i++)
                {
                    if (waypointDataList[i]._transform != null)
                    {
                        if (waypointDataList[i]._transform.hasChanged)
                        {
                            waypointDataList[i]._transform.hasChanged = false;
                            waypointTransformsChanged = true;
                            canUpdateGizmos = false;
                            updateGizmoTimer = 0.0f;
                            break;
                        }
                    }
                    else { break; }
                }
            }
            else if (canUpdateGizmos == false)
            {
                updateGizmoTimer += Time.deltaTime;
                if (updateGizmoTimer >= updateGizmoCoolDown)
                {
                    canUpdateGizmos = true;
                    waypointTransformsChanged = false;
                }
            }

            if (canUpdateGizmos)
            {
                {
                    if (waypointDataList.Count > 2)
                    {
                        if (waypointDataList.Count > 2) CachePositionsAndDistances();
                        Gizmos.color = selected ? selectedPathColor : pathColor;
                        //float Length = totalRouteDistance - 1;
                        //Vector3 prev = waypointDataList[0]._transform.position;
                        //for (float dist = 0; dist < Length; dist += Length / 100)
                        // {
                        //    if (dist <= waypointDataList[waypointDataList.Count - 2]._waypoint.onReachWaypointSettings.distanceFromStart)
                        //    {
                        //        Vector3 next = GetRoutePosition(dist + 1);
                        //        Gizmos.DrawLine(prev, next);
                        //        prev = next;
                        //    }
                        //}
                        for (int i = 1; i < waypointDataList.Count; i++)
                        {
                            Gizmos.DrawLine(waypointDataList [i - 1]._transform.position, waypointDataList[i]._transform.position);
                        }
                        if (!arrowPointer)
                        {
                            arrowPointer = new GameObject("IK_Driver_Waypoint_Gizmo_Helper").transform;
                            arrowPointer.hideFlags = HideFlags.HideInHierarchy;
                        }
                        for (int i = 0; i < waypointDataList.Count; i++) // Draw Arrows to junctions
                        {
                            if (waypointDataList[i]._waypoint != null)
                            {
                                Gizmos.color = selected ? selectedJunctionColor : junctionColor;
                                Debug.Log(waypointDataList[i]._waypoint);
                                if (waypointDataList[i]._waypoint.onReachWaypointSettings.newRoutePoints.Length > 0)
                                {
                                    for (int j = 0; j < waypointDataList[i]._waypoint.onReachWaypointSettings.newRoutePoints.Length; j++)
                                    {
                                        Gizmos.DrawLine(waypointDataList[i]._transform.position, waypointDataList[i]._waypoint.onReachWaypointSettings.newRoutePoints[j].transform.position);
                                    }
                                }
                                for (int j = 0; j < waypointDataList[i]._waypoint.junctionPoint.Length; ++j)
                                {
                                    Gizmos.DrawLine(waypointDataList[i]._transform.position, waypointDataList[i]._waypoint.junctionPoint[j].transform.position);
                                    junctionPosition = waypointDataList[i]._waypoint.junctionPoint[j].transform;
                                    previousMatrix = Gizmos.matrix;
                                    arrowPointer.position = waypointDataList[i]._waypoint.junctionPoint[j].transform.position; //waypointData [i]._transform.position;
                                    arrowPointer.LookAt(waypointDataList[i]._transform);
                                    Gizmos.matrix = Matrix4x4.TRS(junctionPosition.position, arrowPointer.rotation, arrowHeadScale);
                                    Gizmos.DrawFrustum(Vector3.zero, arrowHeadWidth, arrowHeadLength, 0.0f, 5.0f);
                                    Gizmos.matrix = previousMatrix;
                                }
                                if (waypointDataList[i]._waypoint.pitLane != null)
                                {
                                    Gizmos.color = selected ? selectedPitJunctionColor : pitJunctionColor;
                                    Gizmos.DrawLine(waypointDataList[i]._transform.position, waypointDataList[i]._waypoint.pitLane.transform.position);
                                    junctionPosition = waypointDataList[i]._waypoint.pitLane.transform;
                                    previousMatrix = Gizmos.matrix;
                                    arrowPointer.position = waypointDataList[i]._waypoint.pitLane.transform.position; //waypointData [i]._transform.position;
                                    arrowPointer.LookAt(waypointDataList[i]._transform);
                                    Gizmos.matrix = Matrix4x4.TRS(junctionPosition.position, arrowPointer.rotation, arrowHeadScale);
                                    Gizmos.DrawFrustum(Vector3.zero, arrowHeadWidth, arrowHeadLength, 0.0f, 5.0f);
                                    Gizmos.matrix = previousMatrix;
                                }
                                if (waypointDataList[i]._waypoint.pitArea != null)
                                {
                                    Gizmos.color = selected ? selectedPitJunctionColor : pitJunctionColor;
                                    Gizmos.DrawLine(waypointDataList[i]._transform.position, waypointDataList[i]._waypoint.pitArea.transform.position);
                                    junctionPosition = waypointDataList[i]._waypoint.pitArea.transform;
                                    previousMatrix = Gizmos.matrix;
                                    arrowPointer.position = waypointDataList[i]._waypoint.pitArea.transform.position; //waypointData [i]._transform.position;
                                    arrowPointer.LookAt(waypointDataList[i]._transform);
                                    Gizmos.matrix = Matrix4x4.TRS(junctionPosition.position, arrowPointer.rotation, arrowHeadScale);
                                    Gizmos.DrawFrustum(Vector3.zero, arrowHeadWidth, arrowHeadLength, 0.0f, 5.0f);
                                    Gizmos.matrix = previousMatrix;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        Gizmos.color = selected ? selectedPathColor : pathColor;
                        for (int i = 0; i < waypointDataList.Count; i++) // Draw Arrows to connecting waypoints
                        {
                            previousMatrix = Gizmos.matrix;
                            if (waypointDataList[waypointDataList.Count - 2]._waypoint != null && waypointDataList[i]._waypoint != null)
                            {
                                arrowPointer.position = i == 0 ? waypointDataList[waypointDataList.Count - 2]._waypoint.transform.position : waypointDataList[i]._waypoint.transform.position;
                                lookAtIndex = i == 0 ? waypointDataList.Count - 1 : i - 1;
                                if (i == 0)
                                {
                                    arrowPointer.LookAt(waypointDataList[waypointDataList.Count - 1]._waypoint.transform);
                                    arrowPointer.position = waypointDataList[i]._waypoint.transform.position;
                                    arrowPointer.Rotate(0, 180, 0);
                                }
                                else arrowPointer.LookAt(waypointDataList[lookAtIndex]._waypoint.transform);
                                Gizmos.matrix = Matrix4x4.TRS(waypointDataList[lookAtIndex]._waypoint.transform.position, arrowPointer.rotation, arrowHeadScale);
                                Gizmos.DrawFrustum(Vector3.zero, arrowHeadWidth, arrowHeadLength, 0.0f, 5.0f);
                            }
                            else
                            {
                                break;
                            }
                            previousMatrix = Gizmos.matrix;
                        }

                    }

                }
            }

        }
        #endregion

    }
}