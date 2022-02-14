namespace TurnTheGameOn.IKDriver
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;

    public class IKD_DemoController_AI_RaceTrackWithPitLane : MonoBehaviour
    {
        public IKDVC_AI[] vehicles;
        public Image[] vehicleButtonImages;
        public Color goingToPitstopColor;
        public void SetVehicleGoToPitstop (int _vehicleNumber)
        {
            vehicleButtonImages [_vehicleNumber - 1].color = Color.magenta;;
            vehicles [_vehicleNumber - 1].GoToPitStop();
        }

        void Update ()
        {
            for (int i = 0; i < vehicles.Length; i++)
            {
                if (vehicles [i].goToPitStop == false) vehicleButtonImages [i].color = Color.white;
            }
        }
    }
}