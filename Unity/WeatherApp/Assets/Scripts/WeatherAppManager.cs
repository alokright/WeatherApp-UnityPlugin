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
        // If an instance already exists, we don't need to create another
        if (_instance == null)
        {
            // Create the WeatherDetails GameObject
            GameObject weatherDetails = new GameObject(ANDROID_BRIDGE_OBJECT);

            // Attach the WeatherAppManager script
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
        Debug.Log("TEMPERATUE***");
    }
}