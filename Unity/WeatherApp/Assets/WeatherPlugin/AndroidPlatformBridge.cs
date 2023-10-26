using System;
using UnityEngine;

public class AndroidPlatformBridge : IPlatformBridge
{
    public void ShowToast(string message)
    {
#if UNITY_ANDROID 

        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass weatherAppBridge = new AndroidJavaClass("com.clevertap.demo.weatherapp.WeatherAppBridge"))
                {
                    weatherAppBridge.CallStatic("showToast", currentActivity,message,0);
                }
            }
        }

#endif
        Debug.Log("ShowToast***");
    }

    public void ShowTemperatureToast()
    {
#if UNITY_ANDROID 

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


    public void FetchWeeklyTemperature()
    {
#if UNITY_ANDROID 

        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass weatherAppBridge = new AndroidJavaClass("com.clevertap.demo.weatherapp.WeatherAppBridge"))
                {
                    weatherAppBridge.CallStatic("fetchWeeklyTemperature", currentActivity);
                }
            }
        }

#endif
    }
    public void FetchCurrentTemperature()
    {
#if UNITY_ANDROID 
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            using (AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                using (AndroidJavaClass weatherAppBridge = new AndroidJavaClass("com.clevertap.demo.weatherapp.WeatherAppBridge"))
                {
                    weatherAppBridge.CallStatic("fetchCurrentTemperature", currentActivity);
                }
            }
        }

#endif
        Debug.Log("TEMPERATURE***");
    }

}
