using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        WeatherAppManager.Initialize();
    }
   
   public void ShowTemperature()
    {
        WeatherAppManager.ShowTemperatureToast();
    }

    public void FetchCurrentTemperature()
    {
        WeatherAppManager.FetchCurrentTemperature((status,temperature)=> {
            WeatherAppManager.ShowToast(string.Format("Temperature received from Native {0} Celcius",temperature));
        });
    }

    public void FetchDailyTemperature()
    {
        WeatherAppManager.FetchWeeklyTemperature((status,temperature) => {
            WeatherAppManager.ShowToast(string.Format("Weekly temperature received from Native {0} Celcius", JsonUtility.ToJson(temperature)));
        });
    }

}
