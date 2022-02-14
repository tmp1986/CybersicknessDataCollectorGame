namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;
	using UnityEngine.EventSystems;

	public class IKDVC_PlayerInput : MonoBehaviour
	{		
		public IKDVC_DriveSystem vehicleController;
		public IKDVC_Nitro vehicleControllerNitro;
		public IKD_PlayerInputSettings playerInputSettings;		
		public IKD_VehicleUIManager dashboardCanvas;
		public IKD_VehicleCameraSwitch vehicleCameraSystem;
		public bool useStandardCanvas { get; private set; }
		public EventTrigger.Entry nitroON;
		public EventTrigger.Entry nitroOFF;
		public EventTrigger.Entry shiftUp;
		public EventTrigger.Entry shiftDown;		
		private float accelerationInput, footBrakeInput, horizontalInput, emergencyBrakeInput;
		private bool canShift = true;
        bool canCycleCamera;

        void Awake ()
		{
			if (!vehicleControllerNitro) vehicleControllerNitro = GetComponent<IKDVC_Nitro>();
			if (!vehicleController)	vehicleController = GetComponent<IKDVC_DriveSystem>();
			useStandardCanvas = vehicleController.vehicleInput.playerInputSettings.uIType == UIType.Standalone;
			if (playerInputSettings.uIType == UIType.Mobile)
			{
				IKD_StaticUtility.m_IKD_UtilitySettings.useMobileController = true;				
				if (playerInputSettings.mobileCanvas != null) //Spwan UI Mobile Input
				{
					vehicleController.vehicleUI = Instantiate (playerInputSettings.mobileCanvas);
					vehicleController.vehicleUI.vehicleController = vehicleController;
					IKD_MobileControlRig mobileRig = vehicleController.vehicleUI.GetComponentInChildren<IKD_MobileControlRig> ();
					mobileRig.vehicleController = (IKDVC_DriveSystem) vehicleController as IKDVC_DriveSystem;
					
					EventTrigger mobileButton = GameObject.Find("Nitro Button").GetComponent<EventTrigger>(); //Setup Nitro UI Button
					EventTrigger.Entry entry = nitroON;
					mobileButton.triggers.Add(entry);
					entry = nitroOFF;
					mobileButton.triggers.Add(entry);

					if (vehicleController.vehicleSettings.manual) 
					{						
						mobileButton = GameObject.Find ("Shift Up Button").GetComponent<EventTrigger> (); //Setup Shift Up UI Button
						entry = shiftUp;
						mobileButton.triggers.Add (entry);
						
						mobileButton = GameObject.Find ("Shift Down Button").GetComponent<EventTrigger> (); //Setup Shift Down UI Button
						entry = shiftDown;
						mobileButton.triggers.Add (entry);
					}
				}
			} 
			else 
			{
				IKD_StaticUtility.m_IKD_UtilitySettings.useMobileController = false;
				if (useStandardCanvas && playerInputSettings.defaultCanvas != null) //Spawn UI
				{
					vehicleController.vehicleUI = Instantiate (playerInputSettings.defaultCanvas);
				}
				else
				{
					vehicleController.vehicleUI = dashboardCanvas;
					vehicleController.vehicleUI.primaryUIController = true;
				}
				vehicleController.vehicleUI.vehicleController = vehicleController;
			}
		}

		void Update ()
		{
			if (vehicleControllerNitro)
			{
				if ( Input.GetAxisRaw(playerInputSettings.inputAxes.nitro) == 1 ) vehicleControllerNitro.NitroOn();
				if ( Input.GetAxisRaw(playerInputSettings.inputAxes.nitro) == 0 ) vehicleControllerNitro.NitroOff();
			}			
			if (vehicleController.vehicleSettings.manual && canShift)
			{ 
				if (Input.GetAxisRaw(playerInputSettings.inputAxes.shiftUp) == 1)
				{
					canShift = false;
					vehicleController.ShiftUp ();			
				}
				if (Input.GetAxisRaw(playerInputSettings.inputAxes.shiftDown) == 1)
				{
					canShift = false;
					vehicleController.ShiftDown ();
				}
			} 
			else
			{
				if (Input.GetAxisRaw(playerInputSettings.inputAxes.shiftUp) == 0 && Input.GetAxisRaw(playerInputSettings.inputAxes.shiftDown) == 0) canShift = true;
				//if (Input.GetAxisRaw(playerInputSettings.shiftDownInput) == 0) canShift = true;
			}

            if (vehicleCameraSystem.isLookingBack)
            {
                if (Input.GetAxisRaw(playerInputSettings.inputAxes.lookBack) == 0) vehicleCameraSystem.OnLookBackKeyUp();
            }
            else
            {
                if (Input.GetAxisRaw(playerInputSettings.inputAxes.lookBack) == 1) vehicleCameraSystem.LookBackCamera();
            }
		}

		void LateUpdate ()
		{
            //if (canCycleCamera)
            //{
            //    if (Input.GetAxisRaw(playerInputSettings.inputAxes.cycleCamera) == 1)
            //    {
            //        canCycleCamera = false;
            //        vehicleCameraSystem.CycleCamera();
            //    }
            //}
            //else if (Input.GetAxisRaw(playerInputSettings.inputAxes.cycleCamera) == 0)
            //{
            //    canCycleCamera = true;
            //}
		}

		void FixedUpdate ()
		{
			horizontalInput = IKD_CrossPlatformInputManager.GetAxis(playerInputSettings.inputAxes.steering);
			accelerationInput = IKD_CrossPlatformInputManager.GetAxis(playerInputSettings.inputAxes.throttle);
            footBrakeInput = playerInputSettings.inputAxes.invertFootBrake ? -1 * IKD_CrossPlatformInputManager.GetAxis(playerInputSettings.inputAxes.brake) : IKD_CrossPlatformInputManager.GetAxis(playerInputSettings.inputAxes.brake);
            emergencyBrakeInput = IKD_CrossPlatformInputManager.GetAxis(playerInputSettings.inputAxes.handBrake);
			vehicleController.Move(horizontalInput, accelerationInput, footBrakeInput, emergencyBrakeInput);
		}
		
	}
}