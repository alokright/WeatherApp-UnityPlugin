using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public class WeatherAppManager : MonoBehaviour
{
    private static WeatherAppManager _instance;
    private IPlatformBridge _platformBridge;
    private const string UNITY_BRIDGE_OBJECT = "WeatherDetails";
    private List<Action<bool,double[]>> WeeklyTemperatureCallbacks = null;
    private List<Action<bool,double>> CurrentTemperatureCallbacks = null;
    private List<Action<bool,double,double>> LocationCallbacks = null;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            WeeklyTemperatureCallbacks = new List<Action<bool,double[]>>();
            CurrentTemperatureCallbacks = new List<Action<bool,double>>();
            LocationCallbacks = new List<Action<bool,double,double>>();

#if UNITY_ANDROID && !UNITY_EDITOR
            _platformBridge = new AndroidBridge();
#elif UNITY_IOS && !UNITY_EDITOR
            _platformBridge = new iOSBridge();
#else
            _platformBridge = new EditorBridge();
#endif

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
            GameObject weatherDetails = new GameObject(UNITY_BRIDGE_OBJECT);
            _instance = weatherDetails.AddComponent<WeatherAppManager>();
        }
    }

    public void AddCurrentTemperatureCallback(Action<bool,double> callback)
    {
        CurrentTemperatureCallbacks.Add(callback);
    }

    public void AddLocationCallback(Action<bool,double,double> callback)
    {
        LocationCallbacks.Add(callback);
    }

    public void ReceiveData(string data)
    {
       Dictionary<string,object> json =  JsonConvert.DeserializeObject<Dictionary<string, object>>(data);
        RequestType requestType = (RequestType)Enum.Parse(typeof(RequestType), json[Constants.REQUEST_TYPE_KEY].ToString());
        bool requestStatus = false;
        bool.TryParse(json[Constants.RESPONSE_STATUS_KEY].ToString(), out requestStatus);
        switch (requestType)
        {
            case RequestType.CURRENT_TEMPERATURE:
                double temperature = 0;
                if(requestStatus)
                    temperature = double.Parse(json[Constants.TEMPERATURE_KEY].ToString());
                foreach(var call in CurrentTemperatureCallbacks)
                {
                    if (call != null)
                        call.Invoke(requestStatus,temperature);
                }
                CurrentTemperatureCallbacks.Clear();
                break;
            case RequestType.DAILY_TEMPERATURE:
                double[] dailyTemperature = null;
                if (requestStatus)
                {
                    JArray jArray = (JArray)json[Constants.DAILY_TEMPERATURE_KEY];
                    dailyTemperature = jArray.ToObject<double[]>();
                }
               
                foreach (var call in WeeklyTemperatureCallbacks)
                {
                    if (call != null)
                        call.Invoke(requestStatus,dailyTemperature);
                }
                WeeklyTemperatureCallbacks.Clear();
                break;
            case RequestType.LOCATION:

                double lat = int.MinValue;
                double lo = int.MinValue;
                if (requestStatus)
                {
                    lat = double.Parse(json[Constants.LATITUDE_KEY].ToString());
                    lo = double.Parse(json[Constants.LONGITUDE_KEY].ToString());
                }
               
                foreach (var call in LocationCallbacks)
                {
                    if (call != null)
                        call.Invoke(requestStatus,lat,lo);
                }
                LocationCallbacks.Clear();
                break;
        }
    }

    /// <summary>
    /// Show native toast with message
    /// </summary>
    /// <param name="message">Message to be shown</param>
    public static void ShowToast(string message)
    {
        _instance._platformBridge.ShowToast(message);
        Debug.Log("ShowToast***");
    }
    /// <summary>
    /// Show current temperature via native toast
    /// </summary>
    public static void ShowTemperatureToast()
    {
        _instance._platformBridge.ShowTemperatureToast();
        Debug.Log("TEMPERATURE***");
    }

   /// <summary>
   /// Fetch weekly temperature 
   /// </summary>
   /// <param name="callback"> Action with bool request status and double array with temeperature</param>
    public static void FetchWeeklyTemperature(Action<bool,double[]> callback)
    {
        _instance.WeeklyTemperatureCallbacks.Add(callback);
        _instance._platformBridge.FetchWeeklyTemperature();
    }
    /// <summary>
    /// Fetch cuurent temperature
    /// </summary>
    /// <param name="callback">Action with bool request status and temeperature</param>
    public static void FetchCurrentTemperature(Action<bool,double> callback)
    {
        _instance.CurrentTemperatureCallbacks.Add(callback);
        _instance._platformBridge.FetchCurrentTemperature();
        Debug.Log("TEMPERATURE***");
    }

    /// <summary>
    /// Native response code
    /// </summary>
    public enum ResponseStatus
    {
        /// <summary>
        /// Success
        /// </summary>
        SUCCESS,
        /// <summary>
        /// No internet connectivity
        /// </summary>
        NO_CONNECTIVITY,
        /// <summary>
        /// Location permission denied
        /// </summary>
        PERMISSION_DECLINED,
        /// <summary>
        /// Permission entry missing from manifest
        /// </summary>
        PERMISSION_MISSING,
        /// <summary>
        /// GPS disabled on the device
        /// </summary>
        GPS_DISABLED,
        /// <summary>
        /// Weather API unresponsive
        /// </summary>
        API_FAILURE

    }
    /// <summary>
    /// Native Request type
    /// </summary>
    public enum RequestType
    {
        CURRENT_TEMPERATURE, DAILY_TEMPERATURE, LOCATION
    }
}
