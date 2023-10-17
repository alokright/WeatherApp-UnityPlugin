package com.clevertap.demo.weatherapp.callbacks;

import android.location.Location;

public interface OnLocationReceived{
    public void onLocationStatus(boolean status, Location location);
}
