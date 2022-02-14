using UnityEngine;
using System.Collections;

namespace TurnTheGameOn.IKDriver{
	public class IKD_DrawGizmos : MonoBehaviour {

		#region Public Variables
		public enum GizmoType {
			Cube, WireCube,
		}
		public GizmoType gizmoType;
		public Color gizmoColor = Color.green;
		public Vector3 gizmoSize = new Vector3 (0.14f, 0.14f, 0.14f);
		#endregion
		
		#region Main Methods	
		void OnDrawGizmos(){
			
			Gizmos.color = gizmoColor;
			Gizmos.matrix = transform.localToWorldMatrix;
			switch (gizmoType)
			{
			case GizmoType.Cube:
				Gizmos.DrawCube (Vector3.zero, gizmoSize);
				break;
			case GizmoType.WireCube:
				Gizmos.DrawWireCube (Vector3.zero, gizmoSize);
				break;
			default:
				Gizmos.DrawWireCube (Vector3.zero, gizmoSize);
				break;
			}

		}
		#endregion
		
	}
}