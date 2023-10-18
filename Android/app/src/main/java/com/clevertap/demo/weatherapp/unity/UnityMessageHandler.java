package com.clevertap.demo.weatherapp.unity;

import android.location.Location;

import androidx.work.Data;

import com.clevertap.demo.weatherapp.Constants;
import com.unity3d.player.UnityPlayer;

import org.json.JSONException;
import org.json.JSONObject;

public class UnityMessageHandler {
    private static final String UNITY_GAME_OBJECT = "WeatherDetails";
    private static final String UNITY_METHOD_SEND_DATA = "ReceiveData";

    public static void senMessage(String message){
        UnityPlayer.UnitySendMessage(UNITY_GAME_OBJECT,UNITY_METHOD_SEND_DATA,message);
    }
    public static void sendLocationMessage(boolean status, Location location){
        JSONObject obj =  new JSONObject();
        try {
            if(status){
                obj.put(Constants.LATITUDE_KEY,location.getLatitude());
                obj.put(Constants.LONGITUDE_KEY,location.getLongitude());
            }
            obj.put(Constants.RESPONSE_STATUS_KEY,status);
            obj.put(Constants.REQUEST_TYPE_KEY,RequestType.LOCATION);
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }
        senMessage(obj.toString());
    }

    public static void sendWeeklyTemperatureMessage(boolean status, Data temperatureData) {
        JSONObject obj =  new JSONObject();
            try {
                if(status){
                    obj.put(Constants.TEMPERATURE_KEY,temperatureData.getDouble(Constants.TEMPERATURE_KEY,0));
                    obj.put(Constants.DAILY_TEMPERATURE_KEY,temperatureData.getDoubleArray(Constants.DAILY_TEMPERATURE_KEY));
                }
                obj.put(Constants.RESPONSE_STATUS_KEY,status);
                obj.put(Constants.REQUEST_TYPE_KEY,RequestType.DAILY_TEMPERATURE);
            } catch (JSONException e) {
                throw new RuntimeException(e);
            }
            senMessage(obj.toString());
    }
    public static void sendCurrentTemperatureMessage(boolean status, Data temperatureData) {
        JSONObject obj =  new JSONObject();
        try {
            if(status){
                obj.put(Constants.TEMPERATURE_KEY,temperatureData.getDouble(Constants.TEMPERATURE_KEY,0));
              }
            obj.put(Constants.RESPONSE_STATUS_KEY,status);
            obj.put(Constants.REQUEST_TYPE_KEY,RequestType.CURRENT_TEMPERATURE);
        } catch (JSONException e) {
            throw new RuntimeException(e);
        }
        senMessage(obj.toString());
    }
    public enum RequestType{
        CURRENT_TEMPERATURE,DAILY_TEMPERATURE,LOCATION
    }
}
