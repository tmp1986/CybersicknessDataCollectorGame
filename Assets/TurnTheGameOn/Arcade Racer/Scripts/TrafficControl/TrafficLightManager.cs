namespace TurnTheGameOn.IKDriver
{
    using System.Collections;
    using UnityEngine;

    public class TrafficLightManager : MonoBehaviour
    {
        public TrafficLightCycle[] trafficLightCycles;

        private void Start()
        {
            EnableRedLights();
            StartCoroutine(StartTrafficLightCycles());
        }

        private void EnableRedLights()
        {
            for (int i = 0; i < trafficLightCycles.Length; i++)
            {
                for (int j = 0; j < trafficLightCycles[i].trafficLights.Length; j++)
                {
                    trafficLightCycles[i].trafficLights[j].EnableRedLight();
                }
            }
        }

        IEnumerator StartTrafficLightCycles()
        {
            while (true)
            {
                for (int i = 0; i < trafficLightCycles.Length; i++)
                {
                    for (int j = 0; j < trafficLightCycles[i].trafficLights.Length; j++)
                    {
                        trafficLightCycles[i].trafficLights[j].EnableGreenLight();
                    }
                    yield return new WaitForSeconds(trafficLightCycles[i].onDuration - 2);
                    for (int j = 0; j < trafficLightCycles[i].trafficLights.Length; j++)
                    {
                        trafficLightCycles[i].trafficLights[j].EnableYellowLight();
                    }
                    yield return new WaitForSeconds(2.5f);
                    for (int j = 0; j < trafficLightCycles[i].trafficLights.Length; j++)
                    {
                        trafficLightCycles[i].trafficLights[j].EnableRedLight();
                    }
                    yield return new WaitForSeconds(3);
                }
            }
        }
    }
}