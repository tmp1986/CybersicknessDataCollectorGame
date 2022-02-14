namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;
	[CreateAssetMenu(fileName = "IKD_PlayerInputSettings", menuName = "TurnTheGameOn/IK Driver/IKD_PlayerInputSettings")]
	public class IKD_PlayerInputSettings : ScriptableObject
	{
		public IKD_VehicleUIManager mobileCanvas;
		public IKD_VehicleUIManager defaultCanvas;
        public UIType uIType;
        public MobileSteeringType mobileSteeringType;
        public IKD_InputAxesSettings inputAxes;
	}
}