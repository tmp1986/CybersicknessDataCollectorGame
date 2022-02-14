namespace TurnTheGameOn.IKDriver
{
	using UnityEngine;
	public class IKDVC_StayUpright : MonoBehaviour
	{
		
		#region Public Variables
		public float waitTime = 3f;
		public float velocityThreshold = 1f;
		#endregion
		
		#region Private Variables
		private float lastOkTime;
		private Rigidbody rigidbodyRef;
		#endregion
		
		#region Main Methods	
		void Start(){
			rigidbodyRef = GetComponent<Rigidbody>();
		}

		void Update(){
			if (transform.up.y > 0f || rigidbodyRef.velocity.magnitude > velocityThreshold){
				lastOkTime = Time.time;
			}
			if (Time.time > lastOkTime + waitTime){
				RightCar();
			}
		}
			
		void RightCar()	{
			transform.position += Vector3.up;
			transform.rotation = Quaternion.LookRotation(transform.forward);
		}
		#endregion
		
	}
}