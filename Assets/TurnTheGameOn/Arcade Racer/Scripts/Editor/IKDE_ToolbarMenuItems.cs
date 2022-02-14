namespace TurnTheGameOn.IKDriver
{
	using UnityEditor;
	using UnityEngine;

	public class IKDE_ToolbarMenuItems : Editor
	{
		[MenuItem("Tools/TurnTheGameOn/Arcade Racer/Create/AI_WaypointRoute %&r")] private static void SpawnWaypointRoute ()
		{
			GameObject _AI_WaypointRoute = Instantiate (Resources.Load ("IKD_AI_WaypointRoute")) as GameObject;
			_AI_WaypointRoute.name = "IKD_AI_WaypointRoute";			
			GameObject [] newSelection = new GameObject [1];
			newSelection [0] = _AI_WaypointRoute;
			Selection.objects = newSelection;
		}

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Create/TrafficLightManager %&l")]
        private static void SpawnTrafficLightManager()
        {
            GameObject _TrafficLightManager = Instantiate(Resources.Load("IKD_TrafficLightManager")) as GameObject;
            _TrafficLightManager.name = "IKD_TrafficLightManager";
            GameObject[] newSelection = new GameObject[1];
            newSelection[0] = _TrafficLightManager;
            Selection.objects = newSelection;
        }

        [MenuItem("Tools/TurnTheGameOn/Arcade Racer/PlayerPrefs/DeleteAll")]
        private static void DeletePlayerPrefs()
        {
            PlayerPrefs.DeleteAll();
        }

    }
}