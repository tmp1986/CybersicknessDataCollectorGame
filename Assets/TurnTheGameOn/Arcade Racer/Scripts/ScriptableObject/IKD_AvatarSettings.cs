namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;
	[CreateAssetMenu(fileName = "IKD_AvatarSettings", menuName = "TurnTheGameOn/IK Driver/IKD_AvatarSettings")]
	public class IKD_AvatarSettings : ScriptableObject
	{
		[Header("Avatar")]
        public bool ikActive = false; //enable/disable IK control of the avatar
        public Vector3 avatarPosition;
        public TargetSide shiftHand = TargetSide.Right;
        public TargetSide clutchFoot = TargetSide.Left;
        public TargetSide brakeFoot = TargetSide.Right;
        public TargetSide gasFoot = TargetSide.Right;
        public float defaultTorsoLeanIn;
        public AnimationCurve torsoCurve;
        public float maxRotateLeft;
        public float maxRotateRight;
        public float maxLeanLeft;
        public float maxLeanRight;
        [Header("Head")]
        public float defaultLookXPos;
        public float maxLookLeft; //the maximum distance the look target can move left or right of the default position
        public float maxLookRight;
        public float minLookSpeed;
        public float maxLookSpeed;
        [Header("Input")]
        public AvatarInputType avatarInputType = AvatarInputType.Player;
        public float aISteerMultiplier = 1.0f;
        public string steeringAxis = "Horizontal";
        public string throttleAxis = "Vertical";
        [Header("Shifting")]
        public bool enableShifting = true;
        public float shiftAnimSpeed = 2.5f;
        public AnimationCurve shiftCurve;
        [Header("Steering")]
        public SteeringTargets steeringTargets = SteeringTargets.All;
        public bool controlSteeringWheel = true;
        [Range(0, 1)] public float wheelShake = 0.2f;
        [Range(0, 360)] public float steeringWheelRotation = 360; //maximum rotation of steering wheel transform on x axis
        [Range(0, 90)] public float steeringWheelRotationTwoTargets = 85f;
        public Vector3 defaultSteeringWheelRotation = new Vector3(8.75f, 0, 0);
        public float steeringRotationSpeed = 0.2f;
        [Header("IK Timing Control")]
        public SpeedType speedType;
        public float rotationSpeed = 0.19f;
        public float snapBackRotationSpeed = 0.1f;
        public float holdShiftTime = 0.2f;
        public float shiftBackTime = 0.2f;
        public float IKSpeed = 0.15f;
        public float maxrotateSpeeds = 0.3f;
    }
}