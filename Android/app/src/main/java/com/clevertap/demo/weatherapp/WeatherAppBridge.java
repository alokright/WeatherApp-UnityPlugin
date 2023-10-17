package com.clevertap.demo.weatherapp;

import android.content.Context;
import android.util.Log;
import android.widget.Toast;

import com.clevertap.demo.weatherapp.callbacks.OnTemperatureReceived;


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
            public void onTemperatureReceived(boolean status, double temperature) {
                if(status){
                    showToast(context,String.format( "Current temperature at your location is %1$,.2f Celcius",temperature),Toast.LENGTH_SHORT);
                }
            }
        });
    }

    public static void fetchTemperature(Context context){

    }
    public static void debugLog(String message) {
        Log.d(WeatherAppBridge.class.getCanonicalName(), message);
    }
}
