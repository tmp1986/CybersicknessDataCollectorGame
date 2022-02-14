using UnityEngine;
using System.Collections;

namespace TurnTheGameOn.IKDriver
{
	[CreateAssetMenu(fileName = "IKD_UtilitySettings", menuName = "TurnTheGameOn/IK Driver/IKD_UtilitySettings")]
	public class IKD_UtilitySettings : ScriptableObject 
	{		
		//Toggle mobile controls on or off.
		public bool useMobileController;	
	}
}