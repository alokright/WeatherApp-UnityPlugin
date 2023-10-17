package com.clevertap.demo.weatherapp.callbacks;

import org.json.JSONException;

public interface OnTemperatureReceived {
    public void onTemperatureReceived(boolean status, double temperature) throws JSONException;
}
