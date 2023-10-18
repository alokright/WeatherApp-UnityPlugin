package com.clevertap.demo.weatherapp.callbacks;

import androidx.work.Data;

import org.json.JSONException;

public interface OnTemperatureReceived {
    public void onTemperatureReceived(boolean status, Data temperatureData) throws JSONException;
}
