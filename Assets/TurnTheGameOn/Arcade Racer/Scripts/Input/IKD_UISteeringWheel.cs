using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

namespace TurnTheGameOn.IKDriver {
	public class IKD_UISteeringWheel : MonoBehaviour {
		public Image steeringWheel;
		public float maximumAngle = 180.0f;

		public Vector2 deltaPivot = Vector2.zero; // If wheel not rotates around its center, this variable allows tweaking the pivot point
		public float wheelFreeSpeed = 200f; // Degrees per second the wheel rotates when released
		public float wheelAngle; // Wheel's angle in degrees
		public bool wheelBeingHeld; // Whether or not the steering wheel is being held
		public Rect wheelPosition; // Wheel's position on screen
		public Vector2 wheelCenter; // Wheel's center on screen coordinates (not Rect coordinates)
		public float wheelTempAngle;
		public Vector2 mousePosition;
		public string axisName = "Horizontal";
		float currentInput;
		IKD_CrossPlatformInputManager.VirtualAxis m_HorizontalVirtualAxis; // Reference to the joystick in the cross platform input

		void Start(){
			wheelBeingHeld = false;
			wheelAngle = 0f;
			wheelCenter = GetCenter (steeringWheel.rectTransform);
			wheelPosition = new Rect (wheelCenter.x - (steeringWheel.rectTransform.sizeDelta.x/2), wheelCenter.y , steeringWheel.rectTransform.sizeDelta.x, steeringWheel.rectTransform.sizeDelta.y);
			if (!IKD_CrossPlatformInputManager.AxisExists(axisName)){
				// if the axis doesnt exist create a new one in cross platform input
				m_HorizontalVirtualAxis = new IKD_CrossPlatformInputManager.VirtualAxis(axisName);
				IKD_CrossPlatformInputManager.RegisterVirtualAxis(m_HorizontalVirtualAxis);
			} else {
				m_HorizontalVirtualAxis = IKD_CrossPlatformInputManager.VirtualAxisReference(axisName);
			}
		}

		private Vector3 GetCenter(RectTransform rect){
			Vector3[] array = new Vector3[4];
			rect.GetWorldCorners(array);
			Vector3 ret = Vector3.zero;
			for ( int idx = 0; idx < array.Length; ++idx ){
				ret += array[idx]; 
			}
			return ret / 4.0f;
		}

		void GetMouseInput (){
			mousePosition = Input.mousePosition;
			wheelPosition = new Rect (wheelCenter.x - steeringWheel.rectTransform.sizeDelta.x/2, 
				wheelCenter.y - steeringWheel.rectTransform.sizeDelta.y/2, steeringWheel.rectTransform.sizeDelta.x, 
				steeringWheel.rectTransform.sizeDelta.y);

			if (Input.GetMouseButton (0) || Input.GetMouseButtonDown (0))
			if( wheelPosition.Contains ( new Vector2( mousePosition.x, mousePosition.y ) ) ){
				wheelBeingHeld = true;
				//wheelTempAngle = Vector2.Angle( Vector2.up, new Vector2( mousePosition.x - wheelCenter.x, mousePosition.y - wheelCenter.y) );
			}
		}

		void Update(){
			//GetMouseInput ();
			wheelCenter = GetCenter (steeringWheel.rectTransform);
			wheelPosition = new Rect (wheelCenter.x - steeringWheel.rectTransform.sizeDelta.x/2, 
				wheelCenter.y - steeringWheel.rectTransform.sizeDelta.y/2, steeringWheel.rectTransform.sizeDelta.x, 
				steeringWheel.rectTransform.sizeDelta.y);
			//wheelPosition = new Rect (wheelCenter.x - (steeringWheel.rectTransform.sizeDelta.x/2), Screen.height - wheelCenter.y - (steeringWheel.rectTransform.sizeDelta.y/2), steeringWheel.rectTransform.sizeDelta.x, steeringWheel.rectTransform.sizeDelta.y);

			if( wheelBeingHeld ){
				// Find the mouse position on screen
				Vector2 checkInput = Input.mousePosition;
				if (wheelPosition.Contains (new Vector2 (checkInput.x, checkInput.y))) {
					mousePosition = checkInput;
				}

				float wheelNewAngle = Vector2.Angle( Vector2.up, mousePosition - wheelCenter );
				// If mouse is very close to the steering wheel's center, do nothing
				if( Vector2.Distance( mousePosition, wheelCenter ) > 20f ){
					if( mousePosition.x > wheelCenter.x )	wheelAngle -= wheelNewAngle - wheelTempAngle;
					else wheelAngle += wheelNewAngle - wheelTempAngle;
				}
				// Make sure that the wheelAngle does not exceed the maximumAngle
				if( wheelAngle > maximumAngle )	wheelAngle = maximumAngle;
				else if( wheelAngle < -maximumAngle )	wheelAngle = -maximumAngle;
				wheelTempAngle = wheelNewAngle;
				// If user releases the mouse, release the wheel
				if( Input.GetMouseButtonUp( 0 ) )	wheelBeingHeld = false;
			}
			else{
				//Mobile input
				if (TurnTheGameOn.IKDriver.IKD_StaticUtility.m_IKD_UtilitySettings.useMobileController) {
					if( Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved){
						for(int i = 0; i < Input.touchCount; i++){
							Vector2 touchPosition;
							touchPosition.x = Input.touches[i].position.x;
							touchPosition.y = Input.touches[i].position.y;
							Debug.Log (touchPosition);
							if( wheelPosition.Contains ( new Vector2( touchPosition.x, touchPosition.y ) ) ){
								wheelBeingHeld = true;
								wheelTempAngle = Vector2.Angle( Vector2.up, new Vector2( touchPosition.x - wheelCenter.x, Input.touches[i].position.y - wheelCenter.y) );
							}
						}
					}
				}
				//Mouse input
				else {
					Vector2 touchPosition;
					touchPosition.x = Input.mousePosition.x;
					touchPosition.y = Screen.height - Input.mousePosition.y;
					if( Input.GetMouseButtonDown( 0 ) && wheelPosition.Contains ( new Vector2( Input.mousePosition.x, Screen.height - Input.mousePosition.y ) ) ){
						wheelBeingHeld = true;
						wheelTempAngle = Vector2.Angle( Vector2.up, new Vector2( Input.mousePosition.x - wheelCenter.x, Input.mousePosition.y - wheelCenter.y) );
					}
				}

				// If the wheel is rotated and not being held, rotate it to its default angle (zero)
				if( !Mathf.Approximately( 0f, wheelAngle ) ){
					float deltaAngle = wheelFreeSpeed * Time.deltaTime;
					if( Mathf.Abs( deltaAngle ) > Mathf.Abs( wheelAngle ) )	{
						wheelAngle = 0f;
						return;
					}

					if( wheelAngle > 0f )	wheelAngle -= deltaAngle;
					else wheelAngle += deltaAngle;
				}
			}
			currentInput = (wheelAngle / maximumAngle) * -1;
			m_HorizontalVirtualAxis.Update (currentInput);
			float rotateAroundZ = wheelAngle;// * Input.GetAxis ("Horizontal");
			Quaternion targetRotation = Quaternion.Euler (0, 0, rotateAroundZ);
			steeringWheel.rectTransform.localRotation = targetRotation;

		}

	}
}