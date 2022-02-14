namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;
	using System.Collections;
	public static class IKD_StaticUtility
	{				
		private static IKD_UtilitySettings IKD_UtilitySettings;
		public static IKD_UtilitySettings m_IKD_UtilitySettings{
			get{ 
				if (IKD_UtilitySettings == null)
					IKD_UtilitySettings = Resources.Load ("IKD_UtilitySettings") as IKD_UtilitySettings;
				return IKD_UtilitySettings;
			}
		}
		public static float ULerp(float from, float to, float value) // unclamped versions of Lerp and Inverse Lerp, to allow value to exceed the from-to range
        {
            return (1.0f - value)*from + value*to;
        }

	}
}