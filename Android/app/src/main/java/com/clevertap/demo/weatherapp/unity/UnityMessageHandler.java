package com.clevertap.demo.weatherapp.unity;

import com.unity3d.player.UnityPlayer;

public class UnityMessageHandler {
    private static final String UNITY_GAME_OBJECT = "WeatherDetails";
    private static final String UNITY_METHOD_SEND_DATA = "ReceiveData";

    public static void senMessage(String message){
        UnityPlayer.UnitySendMessage(UNITY_GAME_OBJECT,UNITY_METHOD_SEND_DATA,message);
    }
}
