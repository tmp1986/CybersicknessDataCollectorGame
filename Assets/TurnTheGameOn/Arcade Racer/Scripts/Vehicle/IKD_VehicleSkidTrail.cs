using UnityEngine;
using System.Collections;

namespace TurnTheGameOn.IKDriver{
	public class IKD_VehicleSkidTrail : MonoBehaviour {
		
		#region Public Variables
		public float persistTime;
		#endregion
		
		#region Main Methods	
		private IEnumerator Start()	{
			while (true){
				yield return null;
				if (transform.parent.parent == null){
					Destroy(gameObject, persistTime);
				}
			}
		}
		#endregion
		
	}
}