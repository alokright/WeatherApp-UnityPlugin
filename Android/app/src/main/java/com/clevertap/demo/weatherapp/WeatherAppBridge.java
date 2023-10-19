package com.clevertap.demo.weatherapp;

import android.Manifest;
import android.content.Context;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.location.Location;
import android.util.Log;
import android.widget.Toast;

import androidx.core.app.ActivityCompat;
import androidx.work.Data;

import com.clevertap.demo.weatherapp.callbacks.OnLocationReceived;
import com.clevertap.demo.weatherapp.callbacks.OnTemperatureReceived;
import com.clevertap.demo.weatherapp.unity.UnityMessageHandler;

import java.util.ArrayList;
import java.util.List;

/**
 * Created by Alok Kumar on 14/10/22.
 */

public class WeatherAppBridge {

    public static void showToast(Context context, String message, int duration) {
        Toast.makeText(context, message, duration == 0 ? Toast.LENGTH_SHORT : Toast.LENGTH_LONG).show();
    }

    public static void showTemperature(Context context) {
        WeatherManager.getInstance(context).fetchTemperature(context, new OnTemperatureReceived() {
            @Override
            public void onTemperatureReceived(boolean status, Data temperatureData) {
                if(status){
                    double temperature = temperatureData.getDouble(Constants.TEMPERATURE_KEY,0);
                    showToast(context,String.format( "Current temperature at your location is %1$,.2f Celcius",temperature),Toast.LENGTH_SHORT);
                }else
                    showToast(context,"Error while fetching data!",Toast.LENGTH_SHORT);
            }
        });
    }

    public static void fetchUserLocation(Context context){
        WeatherManager.getInstance(context);
        new UserLocationManager().fetchUserLocation(context, new OnLocationReceived() {
            @Override
            public void onLocationStatus(boolean status, Location location) {
                UnityMessageHandler.sendLocationMessage(status,location);
            }
        });
    }
    public static void fetchCurrentTemperature(Context context){
        WeatherManager.getInstance(context).fetchTemperature(context, new OnTemperatureReceived() {
            @Override
            public void onTemperatureReceived(boolean status, Data temperatureData) {
                UnityMessageHandler.sendCurrentTemperatureMessage(status,temperatureData);
            }
        });
    }

    public static void fetchWeeklyTemperature(Context context){
        WeatherManager.getInstance(context).fetchTemperature(context, new OnTemperatureReceived() {
            @Override
            public void onTemperatureReceived(boolean status, Data temperatureData) {
                UnityMessageHandler.sendWeeklyTemperatureMessage(status,temperatureData);
            }
        });
    }

    public static void fetchCurrentTemperatureAtLocation(Context context,double latitude, double longitude){
        WeatherManager.getInstance(context).getTemperature(context,latitude,longitude, new OnTemperatureReceived() {
            @Override
            public void onTemperatureReceived(boolean status, Data temperatureData) {
                UnityMessageHandler.sendCurrentTemperatureMessage(status,temperatureData);
            }
        });
    }

    public static void debugLog(String message) {
        Log.d(WeatherAppBridge.class.getCanonicalName(), message);
    }

    public static boolean checkPermissions(Context context, List<String> permissions) {
        boolean status = true;
        for( String permission : permissions){
            status &= (ActivityCompat.checkSelfPermission(context, permission)== PackageManager.PERMISSION_GRANTED);
        }
        return status;
    }


    public static boolean[] checkPermissionsWithStatus(Context context, List<String> permissions) {
        boolean[] status = new boolean[permissions.size()];
        for (int i = 0; i < permissions.size(); i++) {
            status[i] = ActivityCompat.checkSelfPermission(context, permissions.get(i)) == PackageManager.PERMISSION_GRANTED;
        }
        return status;
    }

}
