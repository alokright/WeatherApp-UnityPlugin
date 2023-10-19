using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class WeatherAppManager : MonoBehaviour
{
    private static WeatherAppManager _instance;
    private const string ANDROID_BRIDGE_OBJECT = "WeatherDetails";
    private List<Action<double[]>> WeeklyTemperatureCallbacks = null;
    private List<Action<double>> CurrentTemperatureCallbacks = null;
    private List<Action<double,double>> LocationCallbacks = null;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            WeeklyTemperatureCallbacks = new List<Action<double[]>>();
            CurrentTemperatureCallbacks = new List<Action<double>>();
            LocationCallbacks = new List<Action<double,double>>();
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

    public void AddCurrentTemperatureCallback(Action<double> callback)
    {
        CurrentTemperatureCallbacks.Add(callback);
    }

    public void AddLocationCallback(Action<double,double> callback)
    {
        LocationCallbacks.Add(callback);
    }

    public void ReceiveData(string data)
    {
       Dictionary<string,object> json =  JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
        RequestType requestType = (RequestType)Enum.Parse(typeof(RequestType), json[Constants.REQUEST_TYPE_KEY].ToString());

        switch (requestType)
        {
            case RequestType.CURRENT_TEMPERATURE:
                double temperature = double.Parse(json[Constants.TEMPERATURE_KEY].ToString());
                foreach(var call in CurrentTemperatureCallbacks)
                {
                    if (call != null)
                        call.Invoke(temperature);
                }
                CurrentTemperatureCallbacks.Clear();
                break;
            case RequestType.DAILY_TEMPERATURE:
                JArray jArray = (JArray)json[Constants.DAILY_TEMPERATURE_KEY];
                double[] dailyTemperature = jArray.ToObject<double[]>();
                foreach (var call in WeeklyTemperatureCallbacks)
                {
                    if (call != null)
                        call.Invoke(dailyTemperature);
                }
                WeeklyTemperatureCallbacks.Clear();
                break;
            case RequestType.LOCATION:

                double lat = double.Parse(json[Constants.LATITUDE_KEY].ToString());
                double lo = double.Parse(json[Constants.LONGITUDE_KEY].ToString());
                foreach (var call in LocationCallbacks)
                {
                    if (call != null)
                        call.Invoke(lat,lo);
                }
                LocationCallbacks.Clear();
                break;
        }
    }


    public static void ShowToast(string message)
    {
#if UNITY_ANDROID && !UNITY_EDITOR

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
        _instance.WeeklyTemperatureCallbacks.Add(callback);
         #if UNITY_ANDROID && !UNITY_EDITOR

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
    public static void FetchCurrentTemperature(Action<double> callback)
    {
        _instance.CurrentTemperatureCallbacks.Add(callback);
#if UNITY_ANDROID && !UNITY_EDITOR

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

    public enum ResponseStatus
    {
        SUCCESS, NO_CONNECTIVITY, PERMISSION_DECLINED, PERMISSION_MISSING, GPS_DISABLED, API_FAILURE
    }
    public enum RequestType
    {
        CURRENT_TEMPERATURE, DAILY_TEMPERATURE, LOCATION
    }
}
