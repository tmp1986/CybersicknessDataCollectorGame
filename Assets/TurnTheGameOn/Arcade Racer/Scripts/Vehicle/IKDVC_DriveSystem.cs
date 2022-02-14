namespace TurnTheGameOn.IKDriver
{
    using UnityEngine;
    [RequireComponent(typeof(Rigidbody))] public class IKDVC_DriveSystem : MonoBehaviour
    {        
        public IKDVC_PlayerInput vehicleInput;
        public TurnTheGameOn.IKDriver.IKD_VehicleUIManager vehicleUI;
        public IKD_VehicleSettings vehicleSettings;
        public IKD_VehicleSettings _vehicleSettings { get; private set; }
        public IKD_WheelSettings wheelSettings;
        public Rigidbody _rigidbody { get; private set; }
        private Transform _transform;         
        public IKD_VehicleWheel[] wheels;
        public IKD_VehicleInputOverride inputOverride;
        private Vector3 previousPosition;
        private float previousRotationY;
        private float previousSteerInput;
        private float distanceTraveledFromPreviousPosition;
        private float velocityMagnitude;
        public float Revs { get; private set; }
        public float RevsAudio { get; private set; }
        public float gearFactor { get; private set; }
        public float topSpeed;
        public float currentSpeed;
        private float speedMultiplier;
        public float AccelInput { get; private set; }
        public float BrakeInput { get; private set; }
        public float CurrentSteerAngle{ get { return m_SteerAngle; }}
        public int currentGear { get; private set; }
        public bool isBraking;
        public bool isReversing;        
        public bool reversing { get; private set; }
        public bool canReverse { get; private set; }        
        public int gearNum { get; private set; }
        private float downGearLimit;
        private float upGearLimit;
        private float targetGearFactor;
        private float amountOfGearsFactor;
        private float gearSpeedRange;
        private float currentGearSpeedLimit;
        float gearSpeedLimitCurvePoint;
        private float m_SteerAngle;        
        private float thrustTorque;
        private float m_CurrentTorque;                    
        private float calculatedSteerSensitivity;
        float hbTorque;
        WheelHit wheelHit_Cached;
        Vector3 wheelPosition_Cached;
        float steerHelper_turnadjust;
        Quaternion steerHelper__velRotation;        
        Quaternion wheelQuaternion_Cached;
        public delegate void ChangedGear();
        public event ChangedGear OnChangedGear;        
        public delegate void SetIsBraking();
        public event SetIsBraking OnSetIsBraking;
        public delegate void SetIsNotBraking();
        public event SetIsNotBraking OnSetIsNotBraking;
        public delegate void SetIsReversing();
        public event SetIsBraking OnSetIsReversing;
        public delegate void SetIsNotReversing();
        public event SetIsNotBraking OnSetIsNotReversing;        
        public delegate void UpdateInput();
        public event UpdateInput OnUpdateInput;
        private float currentThrottleInput;

        private void Awake()
        {            
            _transform = transform;
            _vehicleSettings = vehicleSettings;
            speedMultiplier = _vehicleSettings.speedType == SpeedType.MPH ? 2.23693629f : 3.6f;
            amountOfGearsFactor = (1 / (float)_vehicleSettings.noOfGears);
            vehicleInput = GetComponent <IKDVC_PlayerInput>();
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.centerOfMass = _vehicleSettings.centerOfMass;
            topSpeed = _vehicleSettings.topSpeed;            
            m_CurrentTorque = _vehicleSettings.fullTorqueOverAllWheels - (_vehicleSettings.tractionControl * _vehicleSettings.fullTorqueOverAllWheels);
        }        

        public void Move(float steering, float accel, float footbrake, float handbrake)
        {
            if (inputOverride.overrideSteering) steering = inputOverride.overrideSteeringPower;
			previousSteerInput = steering;
			if (steering > 0 && previousSteerInput <= steering || steering < 0 && previousSteerInput >= steering) {
				if (calculatedSteerSensitivity < 1) calculatedSteerSensitivity += vehicleSettings.steerSensitivity * Time.deltaTime;
			}
            else calculatedSteerSensitivity = 0;
			steering = calculatedSteerSensitivity * steering;
            //--
            steering = Mathf.Clamp(steering, -1, 1);
            AccelInput = accel = Mathf.Clamp(accel, 0, 1);


      

            BrakeInput = footbrake = -1*Mathf.Clamp(footbrake, -1, 0);
            handbrake = Mathf.Clamp(handbrake, 0, 1);            
            m_SteerAngle = steering* _vehicleSettings.maxSteerAngle; //Set the steer on the front wheels assuming that wheels 0 and 1 are the front wheels.
            wheels [0].collider.steerAngle = m_SteerAngle;
            wheels [1].collider.steerAngle = m_SteerAngle;
            //--
            if(inputOverride.overrideBrake) footbrake = inputOverride.overrideBrakePower;
			if(inputOverride.overrideAcceleration){
				accel = inputOverride.overrideAccelerationPower;
				ApplyDrive(accel, footbrake);
				return;
			}
            //--
            currentThrottleInput = accel;
            UpdateWheels(steering, handbrake);
            ApplyDrive(accel, footbrake);
            if (vehicleSettings.dragOverride.enableDragOverride) DragOverride(accel);
            CapSpeed();            
            if (handbrake > 0f) hbTorque = handbrake * float.MaxValue; //Set the handbrake assuming that wheels 2 and 3 are the rear wheels.
            wheels [2].collider.brakeTorque = handbrake > 0.0f ? hbTorque : 0.0f;
            wheels [3].collider.brakeTorque = handbrake > 0.0f ? hbTorque : 0.0f;
            UpdateGear();
            if (vehicleSettings.enableDownForce) _rigidbody.AddForce(-_transform.up * _vehicleSettings.downforce * _rigidbody.velocity.magnitude); // this is used to add more grip in relation to speed
            TractionControl();
            if (OnUpdateInput != null) OnUpdateInput();
            distanceTraveledFromPreviousPosition = Vector3.Distance (previousPosition, _transform.position);
            previousPosition = _transform.position;
            velocityMagnitude = distanceTraveledFromPreviousPosition == 0 ? 0 : _rigidbody.velocity.magnitude;
            currentSpeed = distanceTraveledFromPreviousPosition == 0 ? 0 : velocityMagnitude * speedMultiplier;


            //string[] paramNames = { "Accel", "AccelInput", "BrakeInput","overrideAcceleration" };
            //string[] param = { accel.ToString(), AccelInput.ToString(), BrakeInput.ToString() };

           // DATA_Manager.setCarParameters(paramNames,param);

        }

        [ContextMenu("Shift Up")] public void ShiftUp()
        {
			if (!_vehicleSettings.manual) return;
			if ((currentGear < (_vehicleSettings.noOfGears - 1))) {
				currentGear++;
				gearFactor = _vehicleSettings.minRPM;
			}
		}

		[ContextMenu("Shift Down")] public void ShiftDown ()
		{
			if (!_vehicleSettings.manual) return;
			if (gearSpeedRange < downGearLimit || currentGear == 0)	currentGear--;
			gearFactor = _vehicleSettings.minRPM;
		}

        private void UpdateGear()
        {
            if (_vehicleSettings.manual) targetGearFactor = currentGear == 0 ? _vehicleSettings.minRPM : currentGear == -1 ? targetGearFactor = ((currentSpeed / currentGearSpeedLimit) * BrakeInput) - (1 - _vehicleSettings.maxRPM) : targetGearFactor = ((currentSpeed / currentGearSpeedLimit) * AccelInput) - (1 - _vehicleSettings.maxRPM);
            else targetGearFactor = Mathf.InverseLerp (amountOfGearsFactor * currentGear, amountOfGearsFactor * (currentGear + 1), Mathf.Abs (currentSpeed / topSpeed));
            gearFactor = _vehicleSettings.manual ? Mathf.Lerp (gearFactor, targetGearFactor, Time.deltaTime * 1.0f) : gearFactor = Mathf.Lerp (gearFactor, targetGearFactor, Time.deltaTime * 5f);
            if (System.Single.IsNaN (gearFactor)) gearFactor = _vehicleSettings.minRPM;

			if (vehicleUI) gearSpeedRange = vehicleUI.gearText.text == "N" ? 0.0f : vehicleSettings.gearSpeedLimitCurve.Evaluate (Mathf.Abs ( velocityMagnitude * speedMultiplier / topSpeed)); 
			upGearLimit = amountOfGearsFactor * (currentGear + 1);
			downGearLimit = amountOfGearsFactor * currentGear;
			if (!_vehicleSettings.manual)
			{ 
				if (reversing && canReverse) currentGear = -1;
				else
				{
					if (currentGear > 0 && gearSpeedRange < downGearLimit) currentGear--;
					if (gearSpeedRange > upGearLimit && (currentGear < _vehicleSettings.noOfGears)) currentGear++;
				}
			}            
            gearSpeedLimitCurvePoint = currentGear / (float) _vehicleSettings.noOfGears;
            var gearNumFactor = gearNum/(float) _vehicleSettings.noOfGears;
            var revsRangeMin = IKD_StaticUtility.ULerp(0f, 1.0f, 1 - (1 - gearNumFactor)*(1 - gearNumFactor));  //1 - (1 - gearNumFactor)*(1 - gearNumFactor); // simple function to add a curved bias towards 1 for a value in the 0-1 range
            var revsRangeMax = IKD_StaticUtility.ULerp(1.0f, 1f, gearNumFactor);
            Revs = IKD_StaticUtility.ULerp(revsRangeMin, revsRangeMax, 0.0f);
            float f = (1/(float) _vehicleSettings.noOfGears);
            // gear factor is a normalised representation of the current speed within the current gear's range of speeds. We smooth towards the 'target' gear factor, so that revs don't instantly snap up or down when changing gear.
            var targetGearFactorA = Mathf.InverseLerp(f*gearNum, f*(gearNum + 1), Mathf.Abs(currentSpeed/topSpeed));
            gearFactor = Mathf.Lerp(gearFactor, targetGearFactorA, Time.deltaTime*5f);
            var gearNumFactorA = gearNum/(float) _vehicleSettings.noOfGears;
            var revsRangeMinA = IKD_StaticUtility.ULerp(0f, 1.0f, 1 - (1 - gearNumFactorA)*(1 - gearNumFactorA));  //1 - (1 - gearNumFactor)*(1 - gearNumFactor); // simple function to add a curved bias towards 1 for a value in the 0-1 range
            var revsRangeMaxA = IKD_StaticUtility.ULerp(1.0f, 1f, gearNumFactor);
            RevsAudio = IKD_StaticUtility.ULerp(revsRangeMinA, revsRangeMaxA, gearFactor);
            if (!_vehicleSettings.manual)
            {
                float fl = Mathf.Abs(currentSpeed/_vehicleSettings.topSpeed);
                float upgearlimit = (1/(float) _vehicleSettings.noOfGears)*(gearNum + 1);
                float downgearlimit = (1/(float) _vehicleSettings.noOfGears)*gearNum;
                if (gearNum > 0 && fl < downgearlimit) gearNum--;
                if (fl > upgearlimit && (gearNum < _vehicleSettings.noOfGears)) gearNum++;
            }
        }

        private void CapSpeed()
        {
            if (currentSpeed > _vehicleSettings.topSpeedReverse) Reverse (0);
            if (currentSpeed > _vehicleSettings.topSpeed) _rigidbody.velocity = (_vehicleSettings.topSpeed/speedMultiplier) * _rigidbody.velocity.normalized;
        }
        
        private void ApplyDrive(float accel, float footbrake)
        {
            if (inputOverride.overrideBrake) footbrake = inputOverride.overrideBrakePower;
			if (inputOverride.overrideAcceleration) accel = inputOverride.overrideAccelerationPower;
            if (!_vehicleSettings.manual || currentSpeed < currentGearSpeedLimit) thrustTorque = _vehicleSettings.carDriveType == CarDriveType.FourWheelDrive ? accel * (m_CurrentTorque / 4f) : accel * (m_CurrentTorque / 2f);
            switch (_vehicleSettings.carDriveType)
            {
                case CarDriveType.FourWheelDrive:
                    for (int i = 0; i < 4; i++) 
                    {
                        if (_vehicleSettings.manual) wheels [i].collider.motorTorque = currentSpeed < currentGearSpeedLimit ? thrustTorque : 0;
                        else wheels [i].collider.motorTorque = thrustTorque;
                    }
                    break;
                case CarDriveType.FrontWheelDrive:
                    if (_vehicleSettings.manual) wheels [0].collider.motorTorque = wheels [1].collider.motorTorque = currentSpeed < currentGearSpeedLimit ? thrustTorque : 0;
			        else wheels [0].collider.motorTorque = wheels [1].collider.motorTorque = thrustTorque;
                    break;
                case CarDriveType.RearWheelDrive:
                    if (_vehicleSettings.manual) wheels [2].collider.motorTorque = wheels [3].collider.motorTorque = currentSpeed < currentGearSpeedLimit ? thrustTorque : 0;
			        else wheels [2].collider.motorTorque = wheels [3].collider.motorTorque = thrustTorque;
                    break;
            }
            if (currentGear == -1) // reverse
            {
                if (currentSpeed < _vehicleSettings.topSpeedReverse) Reverse (footbrake);
                if (accel > 0) ApplyBrake (accel);
            }
            if (_vehicleSettings.manual) 
			{				 
				if (currentGear == 0) ApplyBrake (footbrake); // neutral
				else if (currentGear > 0) ApplyBrake (footbrake); // drive
			} 
			else 
			{
				if (currentGear == -1) CancelInvoke ("ResetGearCheck"); // reverse
				else if (currentGear == 0) // neutral
				{
					if (currentSpeed > 0 && Vector3.Angle (_transform.forward, _rigidbody.velocity) < 50f) ApplyBrake (footbrake);
					else if (footbrake > 0) 
					{
						if (currentSpeed < _vehicleSettings.topSpeedReverse) Reverse (footbrake);
					} 
					else if (footbrake == 0 && accel == 0) 
					{
						canReverse = false;
						Invoke ("ResetGearCheck", 0.1f);
					}
                    else if (AccelInput > 0 && BrakeInput == 0 && (isBraking || isReversing))
                    {                        
                        Reverse (0.0f);
                        ApplyBrake (0.0f);
                    }
				} 
				else if (currentGear > 0) // drive
				{
					CancelInvoke ("ResetGearCheck");
					ApplyBrake (footbrake);
				}
			}
        }

		void ApplyBrake (float _brakeAmount)
		{
            if (!isBraking && _brakeAmount > 0.0f)
            {
                isBraking = true;
                if (OnSetIsBraking != null) OnSetIsBraking();
            }
            else if (isBraking && _brakeAmount == 0.0f)
            {
                isBraking = false;
                if (OnSetIsNotBraking != null) OnSetIsNotBraking();
            }
			canReverse = false;
			for (int i = 0; i < 4; i++) 
			{
				if (_brakeAmount > 0) wheels [i].collider.brakeTorque = _vehicleSettings.brakeTorque * _brakeAmount;
				else wheels [i].collider.brakeTorque = 0f;
			}
		}

        void Reverse (float _reverseAmount) 
		{
            if (!isReversing && _reverseAmount > 0.0f) // vehicleState = VehicleState.Reversing;
            {                
                isReversing = true;
                if (OnSetIsReversing != null) OnSetIsReversing();
            }
            else if (isReversing && _reverseAmount == 0.0f) // vehicleState = VehicleState.Driving;
            {                
                isReversing = false;
                if (OnSetIsNotReversing != null) OnSetIsNotReversing();
            }
            CheckCanReverse();
            if (canReverse)
            {
                for (int i = 0; i < 4; i++)
                {
                    wheels[i].collider.brakeTorque = 0f;
                    wheels[i].collider.motorTorque = -_vehicleSettings.reverseTorque * _reverseAmount;
                }
            }
        }

		void ResetGearCheck () 
		{
			currentGear = 1;
		}

		void CheckCanReverse () 
		{
			if (vehicleInput)
            {
                if (vehicleInput.playerInputSettings.uIType == UIType.Mobile)
                {
                    canReverse = true;
                }
            }
            if (vehicleInput)
            {
                if (vehicleInput.playerInputSettings.inputAxes.releaseBrakeToReverse)
                {
                    if (Input.GetKeyUp("down"))
                    {
                        Invoke("SetCanReverse", 0.1f);
                    }
                }
                else if (currentThrottleInput == 0)
                {
                    Invoke("SetCanReverse", 0.1f);
                }
            }
            else if (currentThrottleInput == 0)
            {
                Invoke("SetCanReverse", 0.1f);
            }
        }

		void SetCanReverse ()
		{
			canReverse = true;
		}

        private void UpdateWheels (float _steerInput, float _handBrake)
        {
            for (int i = 0; i < 4; i++)
            {                
                wheels [i].collider.GetWorldPose(out wheelPosition_Cached, out wheelQuaternion_Cached);
                wheels [i].meshTransform.position = wheelPosition_Cached;
                wheels [i].meshTransform.rotation = wheelQuaternion_Cached;
            }            
            if (inputOverride.overrideSteering) _steerInput = inputOverride.overrideSteeringPower;
			previousSteerInput = _steerInput;
			if (_steerInput > 0 && previousSteerInput <= _steerInput || _steerInput < 0 && previousSteerInput >= _steerInput) {
				if (calculatedSteerSensitivity < 1) calculatedSteerSensitivity += vehicleSettings.steerSensitivity * Time.deltaTime;
			}else calculatedSteerSensitivity = 0;
			_steerInput = calculatedSteerSensitivity * _steerInput;
            _steerInput = Mathf.Clamp(_steerInput, -1, 1);
            m_SteerAngle = _steerInput * _vehicleSettings.maxSteerAngle; //Set the steer on the front wheels assuming that wheels 0 and 1 are the front wheels.
            wheels [0].collider.steerAngle = wheels [1].collider.steerAngle = m_SteerAngle;
            for (int i = 0; i < 4; i++)
            {
                wheels [i].collider.GetGroundHit(out wheelHit_Cached);
                if (wheelHit_Cached.normal == Vector3.zero) return; // wheels arent on the ground so dont realign the rigidbody velocity                    
            }            
            if (Mathf.Abs(previousRotationY - _transform.eulerAngles.y) < 10f) // this if is needed to avoid gimbal lock problems that will make the car suddenly shift direction
            {
                steerHelper_turnadjust = (_transform.eulerAngles.y - previousRotationY) * _vehicleSettings.steerHelper;
                steerHelper__velRotation = Quaternion.AngleAxis(steerHelper_turnadjust, Vector3.up);
                _rigidbody.velocity = steerHelper__velRotation * _rigidbody.velocity;
            }
            previousRotationY = _transform.eulerAngles.y;
            hbTorque = _handBrake > 0f ? _handBrake * float.MaxValue : 0.0f; //Set the handbrake assuming that wheels 2 and 3 are the rear wheels.
            wheels [2].collider.brakeTorque = _handBrake > 0.0f ? hbTorque : 0.0f;
            wheels [3].collider.brakeTorque = _handBrake > 0.0f ? hbTorque : 0.0f;
        }
        
        private void TractionControl() // crude traction control that reduces the power to wheel if the car is wheel spinning too much
        {
            switch (_vehicleSettings.carDriveType)
            {
                case CarDriveType.FourWheelDrive:                    
                    for (int i = 0; i < 4; i++)// loop through all wheels
                    {
                        wheels [i].collider.GetGroundHit(out wheelHit_Cached);
                        AdjustTorque(wheelHit_Cached.forwardSlip);
                    }
                    break;
                case CarDriveType.RearWheelDrive:
                    wheels [2].collider.GetGroundHit(out wheelHit_Cached);
                    AdjustTorque(wheelHit_Cached.forwardSlip);
                    wheels [3].collider.GetGroundHit(out wheelHit_Cached);
                    AdjustTorque(wheelHit_Cached.forwardSlip);
                    break;
                case CarDriveType.FrontWheelDrive:
                    wheels [0].collider.GetGroundHit(out wheelHit_Cached);
                    AdjustTorque(wheelHit_Cached.forwardSlip);
                    wheels [1].collider.GetGroundHit(out wheelHit_Cached);
                    AdjustTorque(wheelHit_Cached.forwardSlip);
                    break;
            }
        }

        private void AdjustTorque(float forwardSlip)
        {
            if (forwardSlip >= wheelSettings.slipLimit && m_CurrentTorque >= 0) m_CurrentTorque -= 10 * _vehicleSettings.tractionControl;
            else
            {
                m_CurrentTorque += 10 * _vehicleSettings.tractionControl;
                if (m_CurrentTorque > _vehicleSettings.fullTorqueOverAllWheels) m_CurrentTorque = _vehicleSettings.fullTorqueOverAllWheels;
            }
			m_CurrentTorque = _vehicleSettings.fullTorqueOverAllWheels * _vehicleSettings.torqueCurve.Evaluate (gearSpeedLimitCurvePoint);
			if (currentGear >= 0) currentGearSpeedLimit = topSpeed * _vehicleSettings.gearSpeedLimitCurve.Evaluate (gearSpeedLimitCurvePoint);
			else currentGearSpeedLimit = _vehicleSettings.topSpeedReverse;
        }

        private void DragOverride(float _throttleInput)
        {
            if (_throttleInput > 0) // apply min drag when acceleration is used
            {
                _rigidbody.drag = vehicleSettings.dragOverride.minDrag;
                _rigidbody.angularDrag = vehicleSettings.dragOverride.minAngularDrag;
            }
            else // apply max drag when not accelerating
            {
                _rigidbody.drag = vehicleSettings.dragOverride.maxDrag;
                _rigidbody.angularDrag = vehicleSettings.dragOverride.maxAngularDrag;
            }
        }

    }
}