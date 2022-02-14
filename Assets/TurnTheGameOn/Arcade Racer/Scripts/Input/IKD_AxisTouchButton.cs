using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace TurnTheGameOn.IKDriver{
	public class IKD_AxisTouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
		
		#region Public Variables
		// designed to work in a pair with another axis touch button
		// (typically with one having -1 and one having 1 axisValues)
		public string axisName = "Horizontal"; // The name of the axis
		public float axisValue = 1; // The axis that the value has
		public float responseSpeed = 3; // The speed at which the axis touch button responds
		public float returnToCentreSpeed = 3; // The speed at which the button will return to its centre
//		IKD_AxisTouchButton m_PairedWith; // Which button this one is paired with
		IKD_CrossPlatformInputManager.VirtualAxis m_Axis; // A reference to the virtual axis as it is in the cross platform input
		bool buttonPressed;

		#endregion
		
		#region Private Variables
		
		#endregion
		
		#region Main Methods
		void OnEnable(){
			if (!IKD_CrossPlatformInputManager.AxisExists(axisName)){
				// if the axis doesnt exist create a new one in cross platform input
				m_Axis = new IKD_CrossPlatformInputManager.VirtualAxis(axisName);
				IKD_CrossPlatformInputManager.RegisterVirtualAxis(m_Axis);
			}
			else{
				m_Axis = IKD_CrossPlatformInputManager.VirtualAxisReference(axisName);
			}
			FindPairedButton();
		}

		void FindPairedButton(){
			// find the other button witch which this button should be paired
			// (it should have the same axisName)
			var otherAxisButtons = FindObjectsOfType(typeof(IKD_AxisTouchButton)) as IKD_AxisTouchButton [];
			//Debug.Log (otherAxisButtons);
			if (otherAxisButtons != null){
				for (int i = 0; i < otherAxisButtons.Length; i++){
					if (otherAxisButtons[i].axisName == axisName && otherAxisButtons[i] != this){
						//m_PairedWith = otherAxisButtons[i];
					}
				}
			}
		}

		void Update(){
			if (buttonPressed) {
				 m_Axis.Update (Mathf.MoveTowards (m_Axis.GetValue, axisValue, responseSpeed * Time.deltaTime));
			}
		}
		#endregion
		
		#region Utility Methods
		public void OnPointerDown(PointerEventData data){
			buttonPressed = true;
		}

		public void OnPointerUp(PointerEventData data){
			buttonPressed = false;
			m_Axis.Update(Mathf.MoveTowards(m_Axis.GetValue, 0, responseSpeed * Time.deltaTime));
		}
		#endregion
		
	}
}