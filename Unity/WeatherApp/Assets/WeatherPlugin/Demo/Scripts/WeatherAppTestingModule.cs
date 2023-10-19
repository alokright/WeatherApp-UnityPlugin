using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public class WeatherAppTestingModule : MonoBehaviour
{
    #region Constants
    const string VERIFIED_TEMPLATE = "<color=#13FF1C>Verified</color>";
    const string UNVERIFIED_TEMPLATE = "<color=#FF0000>Verified</color>";
    #endregion

    #region UI Elements
    [SerializeField] private Text lattitudeText;
    [SerializeField] private Text longitudeText;
    [SerializeField] private Toggle locationPermissionToggle;
    [SerializeField] private Text currentTemperature;
    [SerializeField] private Text ManifestPermission;
    [SerializeField] private Text WeatherDetailsGameObject;
    [SerializeField] private Text activityTest;
    [SerializeField] private Text classDependency;
    #endregion

    #region Test Status Text
    public string activityTestText;
    public string classDependencyText;
    public string ManifestPermissionText;
    public string WeatherDetailsGameObjectText;
    #endregion

    #region Test Variables
    [SerializeField] private bool IsValidatingIntegration = false;
    [SerializeField] private bool IsValidatingPermissions = false;
    [SerializeField] private List<string> RequiredPermissions = new List<string>();
    [SerializeField] private bool IsValidatingLocation = false;
    [SerializeField] private bool IsValidatingTemperature = false;
    [SerializeField] private double knownTemperature = int.MinValue;
    [SerializeField] private double Lattitude = int.MinValue;
    [SerializeField] private double Longitute = int.MinValue;
    [SerializeField] private bool testNativeMessage = false;
    [SerializeField] private string nativeMessageJson = null;
    [SerializeField] private GameObject TestModuleParent;
    #endregion

    #region Unity Callbacks
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
    #endregion

    #region Public Methods
    public void ToggleTestModuleButtonTapped()
    {
        TestModuleParent.SetActive(!TestModuleParent.activeInHierarchy);
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
    #endregion

    #region Validation Methods
    private void setTestPartialStatus(Text text, bool status, string test)
    {
        text.text = string.Format("{0}-{1}", test, status ? VERIFIED_TEMPLATE : UNVERIFIED_TEMPLATE);
    }

    public void ValidateIntegration()
    {
        // Check if permissions are requested by the app
        string[] requiredPermissions = { "android.permission.ACCESS_COARSE_LOCATION", "android.permission.ACCESS_FINE_LOCATION" };
        foreach (string permission in requiredPermissions)
        {
            if (!Permission.HasUserAuthorizedPermission(permission))
            {
                Debug.LogError($"Permission not requested: {permission}");
                setTestPartialStatus(ManifestPermission, false, ManifestPermissionText);
            }
            else
            {
                ShowUserPrompt("All Permissions are present!");
                setTestPartialStatus(ManifestPermission, true, ManifestPermissionText);
            }
        }

        // Check if the Plugin MessageReceiver GameObject is in the scene with the name "WeatherDetails"
        GameObject messageReceiver = GameObject.Find("WeatherDetails");
        if (messageReceiver == null)
        {
            Debug.LogError("Plugin MessageReceiver GameObject 'WeatherDetails' is not found in the scene.");
            setTestPartialStatus(WeatherDetailsGameObject, false, WeatherDetailsGameObjectText);
        }
        else
        {
            setTestPartialStatus(WeatherDetailsGameObject, true, WeatherDetailsGameObjectText);
            ShowUserPrompt("Plugin MessageReceiver GameObject 'WeatherDetails' is found in the scene.");
        }

        // Check if the PermissionActivity is accessible
        if (!IsActivityAccessible("com.example.PermissionActivity"))
        {
            Debug.LogError("PermissionActivity is not accessible");
            setTestPartialStatus(activityTest, false, activityTestText);
        }
        else
        {
            setTestPartialStatus(activityTest, true, activityTestText);
        }

        // Check if required classes like LocationListener, AppCompatActivity are in the build path
        if (!IsTypeAvailable("android.location.LocationListener"))
        {
            Debug.LogError("Required class 'LocationListener' is not in the build path");
            setTestPartialStatus(classDependency, false, classDependencyText);
        }
        else
        {
            ShowUserPrompt("Required class 'LocationListener' is found in the build path");
            setTestPartialStatus(classDependency, true, classDependencyText);
        }

        if (!IsTypeAvailable("android.support.v7.app.AppCompatActivity"))
        {
            Debug.LogError("Required class 'AppCompatActivity' is not in the build path");
            setTestPartialStatus(classDependency, false, classDependencyText);
        }
        else
        {
            ShowUserPrompt("Required class 'AppCompatActivity' is found in the build path");
            setTestPartialStatus(classDependency, true, classDependencyText);
        }
    }

    public void ValidatePermissionFlow(List<string> permissions)
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
                    bool[] st = weatherAppBridge.CallStatic<bool[]>("checkPermissionsWithStatus", currentActivity, JsonConvert.SerializeObject(permissions));
                    nativePermissionStatus = new List<bool>(st);
                    Debug.Log("JsonUtility.ToJson(nativePermissionStatus)" + JsonConvert.SerializeObject(nativePermissionStatus));
                }
            }
        }

        //Compare both status
        bool result = true;
        for (int i = 0; i < permissions.Count; i++)
        {
            if (unityPermissionStatus[i] != nativePermissionStatus[i])
                result = false;
        }
        if (result)
            ShowUserPrompt("Unity and Native permissions check are different!");
        else
            ShowUserPrompt("Both Unity and Native permissions check successful!");
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

        //Use this method to validate Positive, Negative & triggering Permissions Rationale flow
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
    #endregion

    #region Location and Temperature Validation
    public void ValidateLocation()
    {
        InitializeParameters();
        //Set device current Lattitude and Longitute
        if (Lattitude == int.MinValue || Longitute == int.MinValue)
        {
            ShowUserPrompt("Please update Lattitude & Longitute for the test location");
        }
        //check permissions 
        if (locationPermissionToggle.isOn)
        {
            // Handle location permission logic here
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
            ShowUserPrompt(String.Format("Difference between known and fetched temperature is Latitude: {0}, Longitute: {1}", (Lattitude - lat), (Longitute - lo)));
        });
    }

    public void ValidateTemperature()
    {
        InitializeParameters();
        //Set local temperature

        if (string.IsNullOrEmpty(currentTemperature.text) || knownTemperature == int.MinValue)
        {
            ShowUserPrompt("Please update known temperature for the test location");
        }

        if (Lattitude == int.MinValue || Longitute == int.MinValue)
        {
            ShowUserPrompt("Please update Lattitude & Longitute for the test location");
        }
        //set location
        //Fetch temperature for the player location
#if UNITY_ANDROID && !UNITY_EDITOR

        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass weatherAppBridge = new AndroidJavaClass("com.clevertap.demo.weatherapp.WeatherAppBridge"))
                {
                    weatherAppBridge.CallStatic("fetchCurrentTemperatureAtLocation", currentActivity, Lattitude, Longitute);
                }
            }
        }
        //Compare fetched temperature with known local temperature
        FindObjectOfType<WeatherAppManager>().AddCurrentTemperatureCallback(temperature => {
            ShowUserPrompt("Difference between known and fetched temperature is " + (knownTemperature - temperature));
        });

#endif
    }
    #endregion

    private void ShowUserPrompt(string message)
    {
        Debug.Log("User Prompt: " + message);
    }

    private void InitializeParameters()
    {
        double.TryParse(lattitudeText.text, out Lattitude);
        double.TryParse(longitudeText.text, out Longitute);
        double.TryParse(currentTemperature.text, out knownTemperature);
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
}
