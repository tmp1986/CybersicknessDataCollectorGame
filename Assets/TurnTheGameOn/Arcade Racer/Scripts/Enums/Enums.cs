using UnityEngine;

namespace TurnTheGameOn.IKDriver
{
	public enum CameraType
	{
		ChaseCamera,
		HelmetCamera
	}

	public enum CarDriveType 
	{
		FrontWheelDrive,
		RearWheelDrive,
		FourWheelDrive
	}

	public enum SpeedType 
	{
		MPH,
		KPH
	}

	public enum MobileSteeringType 
	{
		UIButtons,
		Tilt,
		UIJoystick,
		UISteeringWheel
	}

	public enum DemoVehicles
	{
		LeftSteering,
		RightSteering
	}

	public enum AvatarInputType
	{
		Player,
		AI,
		RCC
	}

	public enum PitStopLaneType
	{
		None,
		HasJunctionToPitLane,
		PitArea,
		PitLane
	}

    public enum UIType
    {
        None,
        Standalone,
        Mobile
    }

    public enum TargetSide
    {
        Left,
        Right,
        None
    }

    public enum SteeringTargets
    {
        Two,
        All
    }

    public enum RouteProgressType
    {
        Simple,
        Advanced
    }

}