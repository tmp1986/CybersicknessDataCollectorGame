namespace TurnTheGameOn.IKDriver
{
    using UnityEngine;
    [RequireComponent(typeof(IKDVC_DriveSystem))]
    public class IKDVC_AI : MonoBehaviour
    {
        public IKD_AISettings AISettings;
        public IKD_WaypointRoute waypointRoute;
        private IKDVC_DriveSystem driveSystem;
        public GameObject sensors;
        public Transform driveTarget;
        public bool startDrivingOnStart;
        public float startDrivingTime;
        public float startingProgress;
        public IKD_AI_PitStopSettings pitStopSettings;
        public IKD_AI_SensorDetails[] detectionData;
        private Rigidbody _rigidbody;
        private Transform _transform;
        private IKD_Waypoint nextWaypoint;
        private RoutePoint progressPoint;
        public bool isDriving { get; private set; }
        private bool canTurnLeft, canTurnRight, canChangeLanes;
        private float cautionAmount;
        private float desiredSpeed;
        private float routeDistanceTraveled;
        private float changeLaneCooldownTimer;
        private float waypointTimer;
        private float stuckTimer;
        private float m_AvoidOtherCarTime;        // time until which to avoid the car we recently collided with
        private float m_AvoidPathOffset;          // direction (-1 or 1) in which to offset path to avoid other car, whilst avoiding        
        private float waypointSpeedFactor = 1;
        private Vector3 relativePoint;
        private Vector3 forwardVector;
        Vector3 progressDelta;
        float steer;
        Vector3 offsetTargetPos;
        float approachingCornerAngle;
        float spinningAngle;
        float cautiousnessRequired;
        float targetAngle;
        Vector3 localTarget;
        float accelBrakeSensitivity;
        float accel;
        public bool goToPitStop { get; private set; }
        private bool canStop;
        private bool waitAtPitStopUntilTimesUpThenReturnToRoute;
        private float minCautionIfGoToPitStop;
        private float minCaution;
        private float cautionAngle;
        float lookAheadDistance;
        private float pitStopTimer;
        float waypointCaution;
        private int currentRoutePointIndex;
        private bool useSpeedLimit;
        private float speedLimit;
        private bool useBrakeTrigger;
        private bool releaseBrakeWhenStopped;
        private float brakeAmount;

        private int PossibleTargetDirection(Transform _transform)
        {
            relativePoint = transform.InverseTransformPoint(_transform.position);
            if (relativePoint.x < 0.0) return -1;
            else if (relativePoint.x > 0.0) return 1;
            else return 0;
        }

        void Awake()
        {
            driveSystem = GetComponent<IKDVC_DriveSystem>();
            _rigidbody = GetComponent<Rigidbody>();
            _transform = GetComponent<Transform>();
            routeDistanceTraveled = startingProgress;
            sensors.SetActive(AISettings.enableSensors);
            for (int i = 0; i < detectionData.Length; i++)
            {
                detectionData[i].sensor.updateInterval = AISettings.updateInterval;
                detectionData[i].sensor.layerMask = AISettings.detectionLayers;
            }
        }

        void Start()
        {
            if (startDrivingOnStart) Invoke("StartDriving", startDrivingTime);
            UpdateSensors();
        }

        void FixedUpdate()
        {
            RouteProgress();
            CheckIfLost();
            RouteLogic();
            SetMinCaution();
            offsetTargetPos = driveTarget.position; // our target position starts off as the 'real' target position
            if (isDriving)
            {
                forwardVector = (_rigidbody.velocity.magnitude > driveSystem.topSpeed * 0.1f) ? _rigidbody.velocity : _transform.forward;  // the car will brake according to the upcoming change in direction of the target.
                approachingCornerAngle = Vector3.Angle(driveTarget.forward, forwardVector); // check out the angle of our target compared to the current direction of the car                            
                spinningAngle = _rigidbody.angularVelocity.magnitude * AISettings.cautionAngularVelocityFactor; // also consider the current amount we're turning, multiplied up and then compared in the same way as an upcoming corner angle
                // Debug.Log ("sa " + spinningAngle);
                // Debug.Log ("ac " + approachingCornerAngle);
                cautionAngle = Mathf.Max(spinningAngle, approachingCornerAngle);
                cautiousnessRequired = Mathf.InverseLerp(AISettings.cautionMaxAngle, 0, cautionAngle); // if it's different to our current angle, we need to be cautious (i.e. slow down) a certain amount
                //Debug.Log (cautiousnessRequired);
                desiredSpeed = Mathf.Lerp(0.0f, driveSystem.topSpeed * (1 - cautionAmount), cautiousnessRequired);
                if (Time.time < m_AvoidOtherCarTime) offsetTargetPos += driveTarget.right * m_AvoidPathOffset; // if are we currently taking evasive action to prevent being stuck against another car veer towards the side of our path-to-target that is away from the other car
                else offsetTargetPos += driveTarget.right * (Mathf.PerlinNoise(Time.time * 0.2f, 0.2f) * 2 - 1) * 0.1f; // no need for evasive action, we can just wander across the path-to-target in a random way, // // which can help prevent AI from seeming too uniform and robotic in their driving
                desiredSpeed = desiredSpeed * waypointSpeedFactor;
                if (useSpeedLimit && desiredSpeed > speedLimit) desiredSpeed = speedLimit;
                accelBrakeSensitivity = (desiredSpeed < driveSystem.currentSpeed) ? AISettings.inputSensitivity.brake : AISettings.inputSensitivity.acceleration; // use different sensitivity depending on whether accelerating or braking:                
                accel = Mathf.Clamp((desiredSpeed - driveSystem.currentSpeed) * accelBrakeSensitivity, -1, 1); // decide the actual amount of accel/brake input to achieve desired speed.
                localTarget = transform.InverseTransformPoint(offsetTargetPos); // calculate the local-relative position of the target, to steer towards                
                targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg; // work out the local angle towards the target
                steer = Mathf.Clamp(targetAngle * AISettings.inputSensitivity.steering, -1, 1) * Mathf.Sign(driveSystem.currentSpeed);  // get the amount of steering needed to aim the car towards the target                
                if (useBrakeTrigger)
                {
                    if (releaseBrakeWhenStopped && driveSystem.currentSpeed < 1)
                    {
                        brakeAmount = 0;
                    }
                    driveSystem.Move(steer, accel, brakeAmount, 0f); // feed input to the car controller.
                }
                else
                {
                    driveSystem.Move(steer, accel, accel, 0f); // feed input to the car controller.
                }
            }
            else
            {
                if (driveSystem.currentSpeed > 2)
                {
                    if (Time.time < m_AvoidOtherCarTime) offsetTargetPos += driveTarget.right * m_AvoidPathOffset; // if are we currently taking evasive action to prevent being stuck against another car: and veer towards the side of our path-to-target that is away from the other car
                    else offsetTargetPos += driveTarget.right; // no need for evasive action, drive toward target //else offsetTargetPos += driveTarget.right* (Mathf.PerlinNoise(Time.time*m_LateralWanderSpeed, m_RandomPerlin)*2 - 1)* m_LateralWanderDistance; // no need for evasive action, we can just wander across the path-to-target in a random way, which can help prevent AI from seeming too uniform and robotic in their driving
                    localTarget = transform.InverseTransformPoint(offsetTargetPos); // calculate the local-relative position of the target, to steer towards                    
                    targetAngle = Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg; // work out the local angle towards the target                    
                    steer = Mathf.Clamp(targetAngle * AISettings.inputSensitivity.steering, -1, 1) * Mathf.Sign(driveSystem.currentSpeed); // get the amount of steering needed to aim the car towards the target                    
                    driveSystem.Move(steer, 0, -1f, 1f); // Car should not be moving, use handbrake to stop
                }
                else driveSystem.Move(0, 0, -1f, 1f); // Car should not be moving, use handbrake to stop 
            }
        }

        void RouteLogic()
        {
            if (AISettings.enableSensors && isDriving)
            {
                if (goToPitStop)
                {
                    ChangeLanesAndGotToPitArea();
                }
                else
                {
                    ChangeLaneIfObstacleIsDetected();
                }
            }
            else if (waitAtPitStopUntilTimesUpThenReturnToRoute)
            {
                WaitAtPitStopUntilTimesUpThenReturnToRoute();
            }
        }
        private Vector3 target;

        void RouteProgress()
        {
            if (AISettings.routeProgressType == RouteProgressType.Simple)
            {
                SimpleRouteProgress();
            }
            else
            {
                AdvancedRouteProgress();
            }
        }

        void SimpleRouteProgress()
        {
            if (currentRoutePointIndex < waypointRoute.waypointDataList.Count)
            {
                if (AISettings.useWaypointDistanceThreshold)
                {
                    if (Vector3.Distance(_transform.position, waypointRoute.waypointDataList[currentRoutePointIndex]._transform.position) <= 5)
                    {
                        waypointRoute.waypointDataList[currentRoutePointIndex]._waypoint.TriggerNextWaypoint(this);
                    }
                }
                driveTarget.position = waypointRoute.waypointDataList[currentRoutePointIndex]._transform.position;
            }
        }

        void AdvancedRouteProgress()
        {
            lookAheadDistance = Mathf.Clamp(driveSystem.currentSpeed + 1, AISettings.lookAheadMin, AISettings.lookAheadMax);
            target = waypointRoute.GetRoutePoint(routeDistanceTraveled + (lookAheadDistance * (1 - cautionAmount))).position;
            if (!System.Single.IsNaN(target.x)) driveTarget.position = target;
            driveTarget.rotation = Quaternion.LookRotation(waypointRoute.GetRoutePoint(routeDistanceTraveled).direction);
            progressPoint = waypointRoute.GetRoutePoint(routeDistanceTraveled); // get our current progress along the route
            progressDelta = progressPoint.position - transform.position;
            if (Vector3.Dot(progressDelta, progressPoint.direction) < 0) routeDistanceTraveled += progressDelta.magnitude * 0.5f;
            routeDistanceTraveled = routeDistanceTraveled > waypointRoute.totalRouteDistance ? routeDistanceTraveled - waypointRoute.totalRouteDistance : routeDistanceTraveled;
        }

        void OnCollisionStay(Collision col)
        {
            if (col.rigidbody != null) // detect collision against other cars, so that we can take evasive action
            {
                IKDVC_DriveSystem otherAI = col.rigidbody.GetComponent<IKDVC_DriveSystem>();
                if (otherAI != null)
                {
                    m_AvoidOtherCarTime = Time.time + 1; // we'll take evasive action for 1 second
                    //if (Vector3.Angle(_transform.forward, otherAI.transform.position - _transform.position) < 90)  m_AvoidOtherCarSlowdown = 0.5f; // the other ai is in front, so it is only good manners that we ought to brake...
                    //else m_AvoidOtherCarSlowdown = 1; // we're in front! ain't slowing down for anybody...                    
                    var otherCarLocalDelta = _transform.InverseTransformPoint(otherAI.transform.position); // both cars should take evasive action by driving along an offset from the path centre, away from the other car
                    float otherCarAngle = Mathf.Atan2(otherCarLocalDelta.x, otherCarLocalDelta.z);
                    m_AvoidPathOffset = 0.2f * -Mathf.Sign(otherCarAngle);
                    if (!goToPitStop) ChangeLane();
                }
            }
        }

        void ChangeLaneCooldownTimer()
        {
            changeLaneCooldownTimer += 1.0f * Time.deltaTime;
            if (changeLaneCooldownTimer > AISettings.changeLaneCooldown)
            {
                canChangeLanes = true;
                changeLaneCooldownTimer = 0.0f;
            }
        }

        void CheckIfCanChangeLanes()
        {
            if (!canChangeLanes) ChangeLaneCooldownTimer();
            for (int i = 0; i < detectionData.Length; i++)
            {
                detectionData[i].hit = detectionData[i].sensor.hit;
                detectionData[i].distance = detectionData[i].sensor.hitDistance;
                detectionData[i].tag = detectionData[i].sensor.hitTag;
                detectionData[i].layer = detectionData[i].sensor.hitLayer;
            }
            if (detectionData[9].hit == true || detectionData[0].hit == true || detectionData[1].hit == true)
            {
                canTurnLeft = false;
            }
            else
            {
                canTurnLeft = true;
            }
            if (detectionData[10].hit == true || detectionData[7].hit == true || detectionData[8].hit == true)
            {
                canTurnRight = false;
            }
            else
            {
                canTurnRight = true;
            }
        }

        void ChangeLaneIfObstacleIsDetected()
        {
            CheckIfCanChangeLanes();
            if (detectionData[3].hit == true && detectionData[4].hit == true && detectionData[5].hit == true)
            {
                if (detectionData[3].distance < AISettings.changeLaneDistance || detectionData[4].distance < AISettings.changeLaneDistance || detectionData[5].distance < AISettings.changeLaneDistance)
                {
                    ChangeLane();
                }
            }
        }

        public void ChangeLane()
        {
            if (canChangeLanes)
            {
                canChangeLanes = false;
                for (int i = 0; i < waypointRoute.waypointDataList.Count; i++) // get next waypoint to determine what the connecting junctions are
                {
                    if (routeDistanceTraveled <= waypointRoute.waypointDataList[i]._waypoint.onReachWaypointSettings.distanceFromStart)
                    {
                        nextWaypoint = waypointRoute.waypointDataList[i]._waypoint;
                        break;
                    }
                }
                if (nextWaypoint != null) // if there's a junction we'll use it as an alternate route
                {
                    if (nextWaypoint.junctionPoint.Length > 0)  // take the first alternate route - setup new route data
                    {
                        if (PossibleTargetDirection(nextWaypoint.junctionPoint[0].transform) == -1 && canTurnLeft) // Debug.Log (nextWaypoint.name); Debug.Log (nextWaypoint.junctionPoint [0].name);
                        {
                            waypointRoute = nextWaypoint.junctionPoint[0].waypointRoute;
                            routeDistanceTraveled = nextWaypoint.onReachWaypointSettings.distanceFromStart;
                            currentRoutePointIndex = nextWaypoint.onReachWaypointSettings.waypointIndexnumber;
                            canChangeLanes = false;
                        }
                        else if (PossibleTargetDirection(nextWaypoint.junctionPoint[0].transform) == 1 && canTurnRight) // Debug.Log (nextWaypoint.name); Debug.Log (nextWaypoint.junctionPoint [0].name);
                        {
                            waypointRoute = nextWaypoint.junctionPoint[0].waypointRoute;
                            routeDistanceTraveled = nextWaypoint.onReachWaypointSettings.distanceFromStart;
                            currentRoutePointIndex = nextWaypoint.onReachWaypointSettings.waypointIndexnumber;
                            canChangeLanes = false;
                        }
                    }
                }
            }
        }

        void ChangeLanesAndGotToPitArea()
        {
            UseCautionInPitLane();
            if (waypointRoute.pitStopLaneType == PitStopLaneType.PitLane)
            {
                // go to pit area
                UseCautionInPitLane();
                for (int i = 0; i < waypointRoute.waypointDataList.Count; i++) // get next waypoint to determine what the connecting junctions are
                {
                    if (routeDistanceTraveled <= waypointRoute.waypointDataList[i]._waypoint.onReachWaypointSettings.distanceFromStart)
                    {
                        nextWaypoint = waypointRoute.waypointDataList[i]._waypoint;
                        break;
                    }
                }
                if (nextWaypoint != null) // if there's a junction we'll use it as an alternate route
                {
                    if (nextWaypoint.pitArea != null)
                    {
                        if (nextWaypoint.pitArea.waypointRoute.pitAreaIndex == pitStopSettings.pitAreaIndex)
                        {
                            waypointRoute = nextWaypoint.pitArea.waypointRoute;
                            routeDistanceTraveled = nextWaypoint.pitArea.onReachWaypointSettings.distanceFromStart;
                            goToPitStop = false;
                            // returnToRaceTrackFromPitStop = true;
                            waitAtPitStopUntilTimesUpThenReturnToRoute = true;
                            pitStopTimer = 0.0f;
                        }
                    }
                }
            }
            else if (waypointRoute.pitStopLaneType == PitStopLaneType.HasJunctionToPitLane)
            {
                // get to the waypoint that junctions to the pit area
                UseCautionInPitLane();
                for (int i = 0; i < waypointRoute.waypointDataList.Count; i++) // get next waypoint to determine what the connecting junctions are
                {
                    if (routeDistanceTraveled <= waypointRoute.waypointDataList[i]._waypoint.onReachWaypointSettings.distanceFromStart)
                    {
                        nextWaypoint = waypointRoute.waypointDataList[i]._waypoint;
                        break;
                    }
                }
                if (nextWaypoint != null) // if there's a junction we'll use it as an alternate route
                {
                    if (nextWaypoint.pitLane != null)
                    {
                        waypointRoute = nextWaypoint.pitLane.waypointRoute;
                        routeDistanceTraveled = nextWaypoint.pitLane.onReachWaypointSettings.distanceFromStart;
                    }
                }
            }
            else
            {
                for (int i = 0; i < waypointRoute.waypointDataList.Count; i++) // get next waypoint to determine what the connecting junctions are
                {
                    if (routeDistanceTraveled <= waypointRoute.waypointDataList[i]._waypoint.onReachWaypointSettings.distanceFromStart)
                    {
                        nextWaypoint = waypointRoute.waypointDataList[i]._waypoint;
                        break;
                    }
                }
                if (nextWaypoint != null) // if there's a junction we'll use it as an alternate route
                {
                    if (nextWaypoint.junctionPoint.Length > 0)  // take the first alternate route - setup new route data
                    {
                        if (pitStopSettings.routeIndexWithPitLaneJunction < waypointRoute.routeIndex)
                        {
                            CheckIfCanChangeLanes();
                            minCautionIfGoToPitStop = Mathf.Clamp(minCautionIfGoToPitStop += 0.05f * Time.deltaTime, AISettings.minCautionIfGoToPitStopDefault, 0.8f);
                            // merge left
                            if (canTurnLeft && canChangeLanes) // Debug.Log (nextWaypoint.name); Debug.Log (nextWaypoint.junctionPoint [0].name);
                            {
                                waypointRoute = nextWaypoint.junctionPoint[0].waypointRoute;
                                routeDistanceTraveled = nextWaypoint.onReachWaypointSettings.distanceFromStart;
                                canChangeLanes = false;
                            }
                        }
                        else if (pitStopSettings.routeIndexWithPitLaneJunction > waypointRoute.routeIndex)
                        {
                            CheckIfCanChangeLanes();
                            minCautionIfGoToPitStop = Mathf.Clamp(minCautionIfGoToPitStop += 0.05f * Time.deltaTime, AISettings.minCautionIfGoToPitStopDefault, 0.8f);
                            // merge right
                            if (canTurnRight && canChangeLanes) // Debug.Log (nextWaypoint.name); Debug.Log (nextWaypoint.junctionPoint [0].name);
                            {
                                waypointRoute = nextWaypoint.junctionPoint[0].waypointRoute;
                                routeDistanceTraveled = nextWaypoint.onReachWaypointSettings.distanceFromStart;
                                canChangeLanes = false;
                            }
                        }
                    }
                }
            }
        }

        void UseCautionInPitLane()
        {
            CheckIfCanChangeLanes();
            if (detectionData[4].hit == true && detectionData[4].distance < AISettings.pitLaneCautionDistanceThreshold)
            {
                minCautionIfGoToPitStop = Mathf.Clamp(minCautionIfGoToPitStop += 0.2f * Time.deltaTime, AISettings.minCautionIfGoToPitStopDefault, 0.95f);
            }
            else
            {
                minCautionIfGoToPitStop = AISettings.minCautionIfGoToPitStopDefault;
            }
        }

        void CheckAutoCautionDistanceThreshold()
        {
            if (waypointRoute.stopForTrafficLight && routeDistanceTraveled > 0 && currentRoutePointIndex >= waypointRoute.waypointDataList.Count - 1)
            {
                if (cautionAmount != 1)
                {
                    minCaution = Mathf.Clamp(minCaution += AISettings.cautionSlowSpeed * Time.deltaTime, waypointCaution, 1.0f);
                    cautionAmount = minCaution;
                    driveSystem.inputOverride.overrideBrake = true;
                    driveSystem.inputOverride.overrideBrakePower = minCaution;
                    driveSystem.inputOverride.overrideAcceleration = true;
                    driveSystem.inputOverride.overrideAccelerationPower = 0;
                    driveSystem._rigidbody.drag = 1.5f;
                    driveSystem._rigidbody.angularDrag = 1;
                }
                else
                {
                    if (driveSystem.currentSpeed == 0)
                    {
                        driveSystem._rigidbody.drag = driveSystem._vehicleSettings.dragOverride.minDrag;
                        driveSystem._rigidbody.angularDrag = driveSystem._vehicleSettings.dragOverride.minAngularDrag;
                    }
                }
            }
            else if ((
                detectionData[3].hit == true ||
                detectionData[4].hit == true ||
                detectionData[5].hit == true
                ) && (
                detectionData[3].distance < AISettings.autoCautionDistanceThreshold ||
                detectionData[4].distance < AISettings.autoCautionDistanceThreshold ||
                detectionData[5].distance < AISettings.autoCautionDistanceThreshold))
            {
                if (cautionAmount != 1)
                {
                    minCaution = Mathf.Clamp(minCaution += AISettings.cautionSlowSpeed * Time.deltaTime, waypointCaution, 1.0f);
                    cautionAmount = minCaution;
                    driveSystem.inputOverride.overrideBrake = true;
                    driveSystem.inputOverride.overrideBrakePower = minCaution;
                    driveSystem.inputOverride.overrideAcceleration = true;
                    driveSystem.inputOverride.overrideAccelerationPower = 0;
                    driveSystem._rigidbody.drag = 1.5f;
                    driveSystem._rigidbody.angularDrag = 1;
                }
                else
                {
                    if (driveSystem.currentSpeed == 0)
                    {
                        driveSystem._rigidbody.drag = driveSystem._vehicleSettings.dragOverride.minDrag;
                        driveSystem._rigidbody.angularDrag = driveSystem._vehicleSettings.dragOverride.minAngularDrag;
                    }
                }
            }
            else
            {
                if (cautionAmount != waypointCaution)
                {
                    minCaution = waypointCaution;
                    cautionAmount = minCaution;
                    driveSystem.inputOverride.overrideBrakePower = 0;
                    driveSystem.inputOverride.overrideBrake = false;
                    driveSystem.inputOverride.overrideAcceleration = false;
                    driveSystem._rigidbody.drag = driveSystem._vehicleSettings.dragOverride.minDrag;
                    driveSystem._rigidbody.angularDrag = driveSystem._vehicleSettings.dragOverride.minAngularDrag;
                }
            }
        }

        void SetMinCaution()
        {
            if (goToPitStop)
            {
                minCaution = minCautionIfGoToPitStop;
            }
            else
            {
                CheckAutoCautionDistanceThreshold();
            }
            //minCaution = goToPitStop ? minCautionIfGoToPitStop : AISettings.minCautionDefault;
        }

        void WaitAtPitStopUntilTimesUpThenReturnToRoute()
        {
            if (pitStopTimer < pitStopSettings.pitStopDuration)
            {
                pitStopTimer += 1.0f * Time.deltaTime;
            }
            else
            {
                if (waypointRoute.pitAreaExitCheck != null)
                {
                    if (waypointRoute.pitAreaExitCheck.isEmpty)
                    {
                        nextWaypoint = waypointRoute.waypointDataList[waypointRoute.waypointDataList.Count - 1]._waypoint.pitLane;
                        routeDistanceTraveled = waypointRoute.waypointDataList[waypointRoute.waypointDataList.Count - 1]._waypoint.pitLane.onReachWaypointSettings.distanceFromStart;
                        waypointRoute = nextWaypoint.waypointRoute;
                        waitAtPitStopUntilTimesUpThenReturnToRoute = false;
                        canStop = false;
                        StartDriving();
                    }
                }
                else
                {
                    Debug.LogWarning("Pit area canExitPitCollider is null");
                    nextWaypoint = waypointRoute.waypointDataList[waypointRoute.waypointDataList.Count - 1]._waypoint.pitLane;
                    routeDistanceTraveled = waypointRoute.waypointDataList[waypointRoute.waypointDataList.Count - 1]._waypoint.pitLane.onReachWaypointSettings.distanceFromStart;
                    waypointRoute = nextWaypoint.waypointRoute;
                    waitAtPitStopUntilTimesUpThenReturnToRoute = false;
                    canStop = false;
                    StartDriving();
                }
            }
        }

        void OnDrawGizmos()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawLine(transform.position, driveTarget.position);
                Gizmos.DrawWireSphere(waypointRoute.GetRoutePosition(routeDistanceTraveled), 1);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(driveTarget.position, driveTarget.position + driveTarget.forward);
            }
            else
            {
                UpdateSensors();
            }
        }

        #region public methods
        public void OnReachedWaypoint(IKD_OnReachWaypointSettings onReachWaypointSettings) // Adjusts the AI speed factor, useful for controlling how fast vehicles should drive through an area.
        {
            if (onReachWaypointSettings.parentRoute == waypointRoute)
            {
                onReachWaypointSettings.OnReachWaypointEvent.Invoke();
                useSpeedLimit = onReachWaypointSettings.useSpeedLimit;
                speedLimit = onReachWaypointSettings.speedLimit;
                useBrakeTrigger = onReachWaypointSettings.useBrakeTrigger;
                brakeAmount = onReachWaypointSettings.brakeAmount * -1;
                releaseBrakeWhenStopped = onReachWaypointSettings.releaseBrakeWhenStopped;
                waypointTimer = stuckTimer = 0.0f;
                waypointSpeedFactor = onReachWaypointSettings.AISpeedFactor;
                cautionAmount = Mathf.Clamp(onReachWaypointSettings.caution, minCaution, 1);
                waypointCaution = onReachWaypointSettings.caution;
                routeDistanceTraveled = onReachWaypointSettings.distanceFromStart;
                if (onReachWaypointSettings.newRoutePoints.Length > 0)
                {
                    int randomIndex = Random.Range(0, onReachWaypointSettings.newRoutePoints.Length);
                    if (randomIndex == onReachWaypointSettings.newRoutePoints.Length) randomIndex -= 1;
                    waypointRoute = onReachWaypointSettings.newRoutePoints[randomIndex].waypointRoute;
                    routeDistanceTraveled = onReachWaypointSettings.newRoutePoints[randomIndex].onReachWaypointSettings.distanceFromStart;
                    currentRoutePointIndex = 0;
                }
                else
                {
                    currentRoutePointIndex = onReachWaypointSettings.waypointIndexnumber;
                }
                if (onReachWaypointSettings.stopDriving) StopDriving();
            }
        }

        public void SetTarget(Transform target)
        {
            driveTarget = target;
            isDriving = true;
        }

        public void ResetProgress() { routeDistanceTraveled = startingProgress; }
        [ContextMenu("StartDriving")] public void StartDriving() { isDriving = canStop = true; }
        [ContextMenu("StopDriving")] public void StopDriving() { if (canStop) isDriving = false; }
        [ContextMenu("GoToPitStop")] public void GoToPitStop() { goToPitStop = canStop = true; }
        #endregion

        [ContextMenu("UpdateSensors")]
        public void UpdateSensors()
        {
            for (int i = 0; i < detectionData.Length; i++)
            {
                detectionData[i].sensor.number = i;
                detectionData[i].sensor.updateInterval = AISettings.updateInterval;
                detectionData[i].sensor.layerMask = AISettings.detectionLayers;
                detectionData[i].sensor.centerWidth = AISettings.sensorFCenterWidth;
                detectionData[i].sensor.height = AISettings.sensorHeight;

                switch (i)
                {
                    case 0:
                        detectionData[i].sensor.width = AISettings.sensorFSideWidth - 0.02f;
                        detectionData[i].sensor.length = AISettings.sensorFSideLength;
                        break;
                    case 1:
                        detectionData[i].sensor.width = AISettings.sensorFSideWidth - 0.02f;
                        detectionData[i].sensor.length = AISettings.sensorFSideLength;
                        break;
                    case 2:
                        detectionData[i].sensor.width = AISettings.sensorFSideWidth - 0.02f;
                        detectionData[i].sensor.length = AISettings.sensorFSideLength;
                        break;
                    case 3:
                        detectionData[i].sensor.width = AISettings.sensorFCenterWidth - 0.02f;
                        detectionData[i].sensor.length = AISettings.sensorFCenterLength;
                        break;
                    case 4:
                        detectionData[i].sensor.width = AISettings.sensorFCenterWidth - 0.02f;
                        detectionData[i].sensor.length = AISettings.sensorFCenterLength;
                        break;
                    case 5:
                        detectionData[i].sensor.width = AISettings.sensorFCenterWidth - 0.02f;
                        detectionData[i].sensor.length = AISettings.sensorFCenterLength;
                        break;
                    case 6:
                        detectionData[i].sensor.width = AISettings.sensorFSideWidth - 0.02f;
                        detectionData[i].sensor.length = AISettings.sensorFSideLength;
                        break;
                    case 7:
                        detectionData[i].sensor.width = AISettings.sensorFSideWidth - 0.02f;
                        detectionData[i].sensor.length = AISettings.sensorFSideLength;
                        break;
                    case 8:
                        detectionData[i].sensor.width = AISettings.sensorFSideWidth - 0.02f;
                        detectionData[i].sensor.length = AISettings.sensorFSideLength;
                        break;
                    case 9:
                        detectionData[i].sensor.width = AISettings.sensorLRWidth - 0.02f;
                        detectionData[i].sensor.length = AISettings.sensorLRLength;
                        break;
                    case 10:
                        detectionData[i].sensor.width = AISettings.sensorLRWidth - 0.02f;
                        detectionData[i].sensor.length = AISettings.sensorLRLength;
                        break;
                }
            }
            for (int i = 0; i < detectionData.Length; i++) detectionData[i].sensor.UpdateSensorTransform();
        }

        void CheckIfLost()
        {
            if (isDriving)
            {
                waypointTimer += Time.deltaTime * 1;
                if (AISettings.useWaypointReset && waypointTimer >= AISettings.waypointReset) MoveVehicleToTarget();
                if (AISettings.useStuckReset && stuckTimer >= AISettings.stuckReset) MoveVehicleToTarget();
                stuckTimer = driveSystem.currentSpeed < 5.0f ? stuckTimer + (Time.deltaTime * 1) : 0;
            }
            else waypointTimer = stuckTimer = 0.0f;
        }

        void MoveVehicleToTarget()
        {
            waypointTimer = stuckTimer = 0;
            _transform.position = driveTarget.position;
            AdvancedRouteProgress();
            _transform.rotation = driveTarget.rotation;
        }

    }
}

//private float m_AvoidOtherCarSlowdown;    // how much to slow down due to colliding with another car, whilst avoiding