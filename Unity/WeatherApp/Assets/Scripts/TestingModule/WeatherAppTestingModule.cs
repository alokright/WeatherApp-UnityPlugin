using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class WeatherAppTestingModule : MonoBehaviour
{
   //public void ValidateIntegration()
   // {
        //Check Permissions entries in Manifest
        //Check if Plugin MessageReceiver gameobject is in the scene
        //Check if activity is there in Manifest or not
        //Check required classes like LocationListener, AppCompatActivity
  //  }
    public void ValidateIntegration()
    {
        // Check if permissions are requested by the app
        string[] requiredPermissions = { "android.permission.ACCESS_COARSE_LOCATION", "android.permission.ACCESS_FINE_LOCATION" };
        foreach (string permission in requiredPermissions)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Debug.LogError($"Permission not requested: {permission}");
            }
            else
                ShowUserPrompt("All Permissions are present!");
        }

        // Check if the Plugin MessageReceiver GameObject is in the scene with the name "WeatherDetails"
        GameObject messageReceiver = GameObject.Find("WeatherDetails");
        if (messageReceiver == null)
        {
            Debug.LogError("Plugin MessageReceiver GameObject 'WeatherDetails' is not found in the scene.");
        }
        else
            ShowUserPrompt("Plugin MessageReceiver GameObject 'WeatherDetails' is found in the scene.");

        // Check if the PermissionActivity is accessible
        if (!IsActivityAccessible("com.example.PermissionActivity"))
        {
            Debug.LogError("PermissionActivity is not accessible.");
        }

        // Check if required classes like LocationListener, AppCompatActivity are in the build path
        if (!IsTypeAvailable("android.location.LocationListener"))
        {
            Debug.LogError("Required class 'LocationListener' is not in the build path.");
        }
        else
            ShowUserPrompt("Required class 'LocationListener' is found in the build path.");

        if (!IsTypeAvailable("android.support.v7.app.AppCompatActivity"))
        {
            Debug.LogError("Required class 'AppCompatActivity' is not in the build path.");
        }
        else
            ShowUserPrompt("Required class 'AppCompatActivity' is found in the build path.");
    }

    private bool IsActivityAccessible(string activityName)
    {
        try
        {
            AndroidJavaClass jc = new AndroidJavaClass(activityName);
            return jc != null;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    private bool IsTypeAvailable(string typeName)
    {
        try
        {
            System.Type type = System.Type.GetType(typeName);
            return type != null;
        }
        catch (System.Exception)
        {
            return false;
        }
    }

    public void ValidatePermissionFlow(List<String>  permissions)
    {
        InitializeParameters();
        //Check Permission using Unity APIS
        List<bool> unityPermissionStatus = CheckPermissionsUnityAPI(permissions);
        //Check Permission using Native API
        List<bool> nativePermissionStatus;
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass weatherAppBridge = new AndroidJavaClass("com.clevertap.demo.weatherapp.WeatherAppBridge"))
                {
                    bool[] st = weatherAppBridge.CallStatic<bool[]>("checkPermissionsWithStatus", currentActivity,permissions);
                    nativePermissionStatus = new List<bool>(st);
                }
            }
        }

        //Compare both status
        bool result = true;
        for(int i = 0;i < permissions.Count; i++)
        {
            if (unityPermissionStatus[i] != nativePermissionStatus[i])
                result = false;
        }
        if (result)
            ShowUserPrompt("Unity and Native permissions check are different!");
        else
            ShowUserPrompt("Bothe Unity and Native permissions check successful!");
        //Trigger Request
        for (int i = 0; i < permissions.Count; i++)
        {
            if (!unityPermissionStatus[i])
            {
                //Request permissions
                using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
                {
                    using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
                    {
                        using (AndroidJavaClass weatherAppBridge = new AndroidJavaClass("com.clevertap.demo.weatherapp.Utils"))
                        {
                            weatherAppBridge.CallStatic("requestLocationPermission", currentActivity);
                        }
                    }
                }
            }
                    
        }

        //Use the this method to validate Positive, Negative & triggering Permissions Rationale flow 
    }

    public List<bool> CheckPermissionsUnityAPI(List<string> permissionsToCheck)
    {
        List<bool> result = new List<bool>();
        for (int i = 0; i < permissionsToCheck.Count; i++)
        {
            string permission = permissionsToCheck[i];
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                // Handle permission not granted
                Debug.Log("Permission not granted: " + permission);
                }
            else
            {
                // Permission is granted
                Debug.Log("Permission granted: " + permission);
            }

            result.Add(Permission.HasUserAuthorizedPermission(permission));
        }
        return result;
    }

    [SerializeField]
    Text lattitudeText;
    [SerializeField]
    Text longitudeText;

    [SerializeField]
    Toggle locationPermissionToggle;
    public void ValidateLocation()
    {
        InitializeParameters();
        //Set device current Lattitude and Longitute
        if (Lattitude == int.MinValue || Longitute == int.MinValue)
        {
            ShowUserPrompt("Please update Lattitude & Longitute for test location");
        }
        //check permissions 
        if (locationPermissionToggle.isOn)
        {

        }
        //Fetch Location 
#if UNITY_ANDROID && !UNITY_EDITOR

        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass weatherAppBridge = new AndroidJavaClass("com.clevertap.demo.weatherapp.WeatherAppBridge"))
                {
                    weatherAppBridge.CallStatic("fetchUserLocation", currentActivity);
                }
            }
        }

#endif
        //Compare fetched location and known location
        FindObjectOfType<WeatherAppManager>().AddLocationCallback((lat, lo) => {
            ShowUserPrompt(String.Format("Difference between known and fetched temperature is Latitude :{0}, Longitute : {1}", (Lattitude - lat),(Longitute - lo)));
        });
        
     }

    [SerializeField]
    private Text currentTemperature;
    public void ValidateTemperature()
    {
        InitializeParameters();
        //Set local temperature

        if (string.IsNullOrEmpty(currentTemperature.text) || knownTemperature == int.MinValue)
        {
            ShowUserPrompt("Please update known temperature for test location");
        }

        if(Lattitude == int.MinValue ||Longitute == int.MinValue)
        {
            ShowUserPrompt("Please update Lattitude & Longitute for test location");
        }
        //set location
        //Fetch temperature for player location
        #if UNITY_ANDROID && !UNITY_EDITOR

        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass weatherAppBridge = new AndroidJavaClass("com.clevertap.demo.weatherapp.WeatherAppBridge"))
                {
                    weatherAppBridge.CallStatic("fetchCurrentTemperatureAtLocation", currentActivity,Lattitude,Longitute);
                }
            }
        }
        //Compare fetched temperature with known local temperature
        FindObjectOfType<WeatherAppManager>().AddCurrentTemperatureCallback(temperature => {
            ShowUserPrompt("Difference between known and fetched temperature is " + (knownTemperature - temperature));
        });

    #endif
    }

    private void ShowUserPrompt(string message)
    {
        Debug.Log("USer Prompt::"+message);
    }

    private void SetupScene()
    {

    }

    private void InitializeParameters()
    {
        
        double.TryParse(lattitudeText.text, out Lattitude);

        double.TryParse(longitudeText.text, out Longitute);

        double.TryParse(currentTemperature.text, out knownTemperature);
    }



    [SerializeField]
    private bool IsValidatingIntegration = false;
    [SerializeField]
    private bool IsValidatingPermissions = false;
    [SerializeField]
    private List<string> RequiredPermissions = new List<string>();
    [SerializeField]
    private bool IsValidatingLocation = false;
    [Header("=======Current Temperature tests=======")]
    [SerializeField]
    private bool IsValidatingTemperature = false;
    [SerializeField]
    private double knownTemperature = int.MinValue;
    [SerializeField]
    private double Lattitude = int.MinValue;
    [SerializeField]
    private double Longitute = int.MinValue;

    [Header("=======Native Unity Callback tests=======")]
    [SerializeField]
    private bool testNativeMessage = false;
    [SerializeField]
    private string nativeMessageJson = null;

    [SerializeField]
    private GameObject TestModuleParent;
    public void ToggleTestModuleButtonTapped()
    {
        TestModuleParent.SetActive(!TestModuleParent.activeInHierarchy);
    }

    private void LateUpdate()
    {
        if (IsValidatingIntegration)
        {
            IsValidatingIntegration = false;
            ValidateIntegration();
        }

        if (IsValidatingPermissions)
        {
            IsValidatingPermissions = false;
            List<string> permissionList = new List<string>
        {
            "android.permission.ACCESS_COARSE_LOCATION",
            "android.permission.ACCESS_FINE_LOCATION"
        };
            ValidatePermissionFlow(permissionList);
        }

        if (IsValidatingLocation)
        {
            IsValidatingLocation = false;
            ValidateLocation();
        }

        if (IsValidatingTemperature)
        {
            IsValidatingTemperature = false;
            ValidateTemperature();
        }

        if (testNativeMessage)
        {
            testNativeMessage = false;
            new WeatherAppManager().ReceiveData(nativeMessageJson);
        }
    }

    public void executeTemperatureTest()
    {
        IsValidatingTemperature = true;
    }

    public void executeLocationTest()
    {
        IsValidatingLocation = true;
    }

    public void executePermissionFlowTest()
    {
        IsValidatingPermissions = true;
    }

    public void executeIntergrationTests()
    {
        IsValidatingIntegration = true;
    }

    public void openAppSettings()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass weatherAppBridge = new AndroidJavaClass("com.clevertap.demo.weatherapp.Utils"))
                {
                    weatherAppBridge.CallStatic("openAppSettings", currentActivity);
                }
            }
        }
    }
}
