using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherAppTestingModule : MonoBehaviour
{
   public void ValidateIntegration()
    {
        //Check Permissions entries in Manifest
        //Check if Plugin MessageReceiver gameobject is in the scene
        //Check if activity is there in Manifest or not
        //Check required classes like LocationListener, AppCompatActivity
    }

    public void ValidatePermissionFlow()
    {
        //Check Permission using Unity APIS
        //Check Permission using Native API

        //Compare both status

        //If both are false
        //Trigger Request

        //Use the this method to validate Positive, Negative & triggering Permissions Rationale flow 
    }

    public void ValidateLocation()
    {
        //Set device current Lattitude and Longitute
        //Fetch Location 
        //Use this in tandem with Validate Permission flow to check with/out permission
        //Compare fetched location and known location
    }

    public void ValidateTemperature()
    {
        //Set local temperature
        //set location in player location
        //Fetch temperature for player location
        //Compare fetched temperature with known local temperature
    }

    
    private void SetupScene()
    {

    }

    private void InitializeParameters()
    {

    }



    [SerializeField]
    private bool IsValidatingIntegration = false;
    [SerializeField]
    private bool IsValidatingPermissions = false;
    [SerializeField]
    private List<string> RequiredPermissions = new List<string>();
    [SerializeField]
    private bool IsValidatingLocation = false;
    [SerializeField]
    private bool IsValidatingTemperature = false;

    [SerializeField]
    private string Lattitude;
    [SerializeField]
    private string Longitute;

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
            ValidatePermissionFlow();
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

    }


}
