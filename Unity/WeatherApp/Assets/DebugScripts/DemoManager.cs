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
}
