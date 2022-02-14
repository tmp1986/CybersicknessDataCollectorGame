using UnityEditor;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace ThiagoMalheiros.SceneTransition
{
    // Custom Editor using SerializedProperties.
    // Automatic handling of multi-object editing, undo, and prefab overrides.
    [CustomEditor(typeof(SceneLoader))]
    [CanEditMultipleObjects]
    public class SceneLoaderEditor : Editor
    {
        SerializedProperty FadeImage;
        SerializedProperty FadeMaterial;
        SerializedProperty VRMode;
        SerializedProperty UseSceneForLoadingScreen;
        SerializedProperty LoadingScreen;
        SerializedProperty LoadingSceneName;
        SerializedProperty FadeColor;
        SerializedProperty FadeSeconds;
        SerializedProperty MinimumLoadingScreenSeconds;
        SerializedProperty FadeInLoadingScreen;
        SerializedProperty FadeOutLoadingScreen;
        SerializedProperty IgnoreTimeScale;

        void OnEnable ()
        {
            // Setup the SerializedProperties.
            FadeImage = serializedObject.FindProperty ("FadeImage");
            FadeMaterial = serializedObject.FindProperty ("FadeMaterial");
            VRMode = serializedObject.FindProperty ("VRMode");
            UseSceneForLoadingScreen = serializedObject.FindProperty ("UseSceneForLoadingScreen");
            LoadingScreen = serializedObject.FindProperty ("LoadingScreen");
            LoadingSceneName = serializedObject.FindProperty ("LoadingSceneName");
            FadeColor = serializedObject.FindProperty ("FadeColor");
            FadeSeconds = serializedObject.FindProperty ("FadeSeconds");
            MinimumLoadingScreenSeconds = serializedObject.FindProperty ("MinimumLoadingScreenSeconds");
            FadeInLoadingScreen = serializedObject.FindProperty ("FadeInLoadingScreen");
            FadeOutLoadingScreen = serializedObject.FindProperty ("FadeOutLoadingScreen");
            IgnoreTimeScale = serializedObject.FindProperty("IgnoreTimeScale");
        }

        public override void OnInspectorGUI()
        {
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            serializedObject.Update ();

            if (!VRMode.boolValue)
            {
                EditorGUILayout.PropertyField (FadeImage, new GUIContent ("Fade Image"));
            }
            else
            {
                EditorGUILayout.PropertyField (FadeMaterial, new GUIContent ("Fade Material"));
            }
            EditorGUILayout.PropertyField(VRMode, new GUIContent ("VR Support"));
            EditorGUILayout.PropertyField(UseSceneForLoadingScreen, new GUIContent ("Use Scene For Loading Screen"));
            if (UseSceneForLoadingScreen.boolValue)
            {
                EditorGUILayout.PropertyField(LoadingSceneName, new GUIContent ("Loading Scene Name"));
            }
            else if(!VRMode.boolValue)
            {
                EditorGUILayout.PropertyField(LoadingScreen, new GUIContent ("Loading Screen"));
            }
            EditorGUILayout.PropertyField(FadeColor, new GUIContent ("Fade Color"));
            EditorGUILayout.PropertyField(FadeSeconds, new GUIContent ("Fade Seconds"));
            if (!VRMode.boolValue || UseSceneForLoadingScreen.boolValue)
            {
                EditorGUILayout.PropertyField (MinimumLoadingScreenSeconds, new GUIContent ("Minimum Loading Screen Seconds"));
                EditorGUILayout.PropertyField (FadeInLoadingScreen, new GUIContent ("Fade In Loading Screen"));
                EditorGUILayout.PropertyField (FadeOutLoadingScreen, new GUIContent ("Fade Out Loading Screen"));
            }
            EditorGUILayout.PropertyField(IgnoreTimeScale, new GUIContent("Ignore Time Scale"));

            // Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
            serializedObject.ApplyModifiedProperties ();
        }
    }
}
