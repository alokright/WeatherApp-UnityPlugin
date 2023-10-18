using System;
using UnityEngine;

public class WeatherAppManager : MonoBehaviour
{
    private static WeatherAppManager _instance;
    private const string ANDROID_BRIDGE_OBJECT = "WeatherDetails";
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else if (_instance != this)
        {
            Destroy(this.gameObject);
        }
    }

    public static void Initialize()
    {
        if (_instance == null)
        {
            GameObject weatherDetails = new GameObject(ANDROID_BRIDGE_OBJECT);

            _instance = weatherDetails.AddComponent<WeatherAppManager>();
        }
    }

    public void ReceiveData(string data)
    {

    }

    public static void ShowTemperatureToast()
    {
    #if UNITY_ANDROID && !UNITY_EDITOR

        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass weatherAppBridge = new AndroidJavaClass("com.clevertap.demo.weatherapp.WeatherAppBridge"))
                {
                    weatherAppBridge.CallStatic("showTemperature", currentActivity);
                }
            }
        }

    #endif
        Debug.Log("TEMPERATURE***");
    }

  
    public static void FetchWeeklyTemperature(Action<double[]> callback)
    {

    }
    public static void FetchCurrentTemperature(Action<double> callback)
    {
       #if UNITY_ANDROID && !UNITY_EDITOR

        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass weatherAppBridge = new AndroidJavaClass("com.clevertap.demo.weatherapp.WeatherAppBridge"))
                {
                    weatherAppBridge.CallStatic("showTemperature", currentActivity);
                }
            }
        }

    #endif
        Debug.Log("TEMPERATURE***");
    }

    public enum ResponseStatus
    {
        SUCCESS, NO_CONNECTIVITY, PERMISSION_DECLINED, PERMISSION_MISSING, GPS_DISABLED, API_FAILURE
    }

}
