using UnityEditor;

public class ExportOrImportPackages : Editor
{
    static string InputManager_Asset = "ProjectSettings/InputManager.asset";
    static string InputManager_XboxOneController_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/Input/InputManager_XboxOneController.unitypackage";
    static string InputManager_Keyboard_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/Input/InputManager_Keyboard.unitypackage";
    static string InputManager_Keyboard_And_XboxOneController_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/Input/InputManager_Keyboard_And_XboxOneController.unitypackage";
    static string TagManager_Asset = "ProjectSettings/TagManager.asset";
    static string TagManager_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/ProjectSettings/TagsAndLayers/TagManager.unitypackage";
    static string RCCSupport_ExportPath = "Assets/TurnTheGameOn/Arcade Racer/Support/RCC/RCC_Support.unitypackage";
    static ExportPackageOptions flags = ExportPackageOptions.Interactive;

    #region Export InputManager

    [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Export/InputManager_XboxOneController")]
    static void Export_InputManager_XboxOneController()
    {
        AssetDatabase.ExportPackage(InputManager_Asset, InputManager_XboxOneController_ExportPath, flags);
    }
    
    [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Export/InputManager_Keyboard")]
    static void Export_InputManager_Keyboard()
    {
        AssetDatabase.ExportPackage(InputManager_Asset, InputManager_Keyboard_ExportPath, flags);
    }
    
    [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Export/InputManager_Keyboard_And_XboxOneController")]
    static void Export_InputManager_Keyboard_And_XboxOneController()
    {
        AssetDatabase.ExportPackage(InputManager_Asset, InputManager_Keyboard_And_XboxOneController_ExportPath, flags);
    }

    #endregion

    #region Export TagManager

    [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Export/TagManager")]
    static void Export_TagManager()
    {
        AssetDatabase.ExportPackage(TagManager_Asset, TagManager_ExportPath, flags);
    }

    #endregion

    #region Import InputManager

    [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Import/InputManager/Keyboard")]
    static void Import_InputManager_Keyboard()
    {
        AssetDatabase.ImportPackage(InputManager_Keyboard_ExportPath, true);
    }

    [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Import/InputManager/Keyboard_And_XboxOneController")]
    static void Import_InputManager_Keyboard_And_XboxOneController()
    {
        AssetDatabase.ImportPackage(InputManager_Keyboard_And_XboxOneController_ExportPath, true);
    }

    [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Import/InputManager/XboxOneController")]
    static void Import_InputManager_XboxOneController()
    {
        AssetDatabase.ImportPackage(InputManager_XboxOneController_ExportPath, true);
    }

    #endregion

    #region Import TagManager

    [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Import/TagManager")]
    static void Import_TagManager()
    {
        AssetDatabase.ImportPackage(TagManager_ExportPath, true);
    }

    #endregion

    #region Import RCC Support

    [MenuItem("Tools/TurnTheGameOn/Arcade Racer/Import/RCC Support")]
    static void Import_RCC_Support()
    {
        AssetDatabase.ImportPackage(RCCSupport_ExportPath, true);
    }

    #endregion

}