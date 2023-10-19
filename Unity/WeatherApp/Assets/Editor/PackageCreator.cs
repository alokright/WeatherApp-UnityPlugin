using UnityEditor;
using UnityEngine;

public class PackageCreator : MonoBehaviour
{
    [MenuItem("Plugin/Export UnityPackage")]
    public static void CreateUnityPackage()
    {
        string[] assetPaths = new string[]
        {
            "Assets/WeatherPlugin",
            "Assets/ExternalDependencyManager",
           
        };

        string outputPath = "WeatherPlugin.unitypackage";

        AssetDatabase.ExportPackage(assetPaths, outputPath, ExportPackageOptions.Recurse);
        Debug.Log("UnityPackage created at: " + outputPath);
    }
}
