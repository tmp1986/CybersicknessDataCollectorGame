using UnityEngine;
using System.Collections;

namespace TurnTheGameOn.IKDriver {
	public class IKD_UISetSizeDelta : MonoBehaviour {
		
		#region Public Variables
		public RectTransform rectTrans;
		public Vector2 newSizeDelta;
		#endregion
		
		#region Private Variables
		
		#endregion
		
		#region Main Methods	
		void Start () {
			if (!rectTrans)	rectTrans = GetComponent<RectTransform> ();
			rectTrans.sizeDelta = newSizeDelta;
			Destroy (this);
		}
		#endregion

		
	}
}