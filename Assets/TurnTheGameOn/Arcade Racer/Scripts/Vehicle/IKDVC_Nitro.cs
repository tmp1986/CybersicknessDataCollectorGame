namespace TurnTheGameOn.IKDriver
{
    using System.Collections;
    using UnityEngine;
    [RequireComponent(typeof(IKDVC_DriveSystem))]
    public class IKDVC_Nitro : MonoBehaviour
    {
        public GameObject nitroEffectObject;
        private IKDVC_DriveSystem vehicleController;
        private bool canUpdateNitroAmount;
        public bool nitroOn { get; private set; }
        public float nitroAmount { get; private set; }

        [ContextMenu("NiroON")]
        public void NitroOn()
        {
            if (vehicleController._vehicleSettings.enableNitro && !nitroOn && nitroAmount > 2.0f)
            {
                nitroEffectObject.SetActive(true);
                vehicleController.topSpeed = vehicleController.vehicleSettings.topSpeed + vehicleController._vehicleSettings.nitroTopSpeed;
                vehicleController._vehicleSettings.fullTorqueOverAllWheels = vehicleController._vehicleSettings.fullTorqueOverAllWheels + vehicleController._vehicleSettings.nitroFullTorque;
                nitroOn = true;
            }
        }

        [ContextMenu("NiroOFF")]
        public void NitroOff()
        {
            if (nitroOn && vehicleController._vehicleSettings.enableNitro)
            {
                nitroEffectObject.SetActive(false);
                vehicleController.topSpeed = vehicleController.vehicleSettings.topSpeed - vehicleController._vehicleSettings.nitroTopSpeed;
                vehicleController._vehicleSettings.fullTorqueOverAllWheels = vehicleController._vehicleSettings.fullTorqueOverAllWheels - vehicleController._vehicleSettings.nitroFullTorque;
                nitroOn = false;
            }
        }

        void Start()
        {
            if (!vehicleController) vehicleController = GetComponent<IKDVC_DriveSystem>();
            if (vehicleController.vehicleSettings.enableNitro)
            {
                nitroAmount = vehicleController._vehicleSettings.nitroDuration;
                StartCoroutine("UpdateNitroAmount");
            }
        }

        void OnDisable()
        {
            canUpdateNitroAmount = false;
            StopAllCoroutines();
        }

        IEnumerator UpdateNitroAmount()
        {
            canUpdateNitroAmount = true;
            while (canUpdateNitroAmount)
            {
                if (!nitroOn && nitroAmount < vehicleController._vehicleSettings.nitroDuration)
                {
                    nitroAmount += vehicleController._vehicleSettings.nitroRefillRate * Time.deltaTime;
                    if (nitroAmount > vehicleController._vehicleSettings.nitroDuration) nitroAmount = vehicleController._vehicleSettings.nitroDuration;
                }
                else
                {
                    nitroAmount -= vehicleController._vehicleSettings.nitroSpendRate * Time.deltaTime;
                    if (nitroAmount < 0)
                    {
                        nitroAmount = 0;
                        NitroOff();
                    }
                }
                yield return null;
            }
        }
    }
}