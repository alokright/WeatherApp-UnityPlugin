package com.clevertap.demo.weatherapp;

import android.content.Context;
import android.location.Location;

import androidx.annotation.NonNull;
import androidx.lifecycle.LifecycleOwner;
import androidx.lifecycle.Observer;
import androidx.work.Data;
import androidx.work.OneTimeWorkRequest;
import androidx.work.WorkInfo;
import androidx.work.WorkManager;

import com.clevertap.demo.weatherapp.callbacks.OnLocationReceived;
import com.clevertap.demo.weatherapp.callbacks.OnTemperatureReceived;

public class WeatherManager {

    private static WeatherManager _manager = null;
    private UserLocationManager locationManager;
    private WeatherManager(Context context){
        locationManager = new UserLocationManager();
    }

    public static WeatherManager getInstance(Context context){
        if(_manager == null){
            _manager = new WeatherManager(context);

        }
        return _manager;
    }

    public void fetchTemperature(Context context, OnTemperatureReceived onCallback){
        locationManager.fetchUserLocation(context, new OnLocationReceived() {
            @Override
            public void onLocationStatus(boolean status, Location location) {
                if(status){
                    getTemperature(context,location.getLatitude(),location.getLongitude(), onCallback);
                }
            }
        });
    }
    public void getTemperature(Context context, double latitude, double longitude, OnTemperatureReceived temperatureCallback) {
        Data inputData = new Data.Builder()
                .putDouble("latitude", latitude)
                .putDouble("longitude", longitude)
                .build();

        OneTimeWorkRequest fetchTemperatureRequest = new OneTimeWorkRequest.Builder(FetchTemperatureWorker.class)
                .build();

        WorkManager.getInstance(context).enqueue(fetchTemperatureRequest);

        WorkManager.getInstance(context).getWorkInfoByIdLiveData(fetchTemperatureRequest.getId())
                .observe((LifecycleOwner) context, new Observer<WorkInfo>() {
                    @Override
                    public void onChanged(WorkInfo workInfo) {
                        if (workInfo != null && workInfo.getState() == WorkInfo.State.SUCCEEDED) {
                            double temperature = workInfo.getOutputData().getDouble("temperature", 0);
                            WeatherAppBridge.debugLog("temperature :: " + temperature);
                            temperatureCallback.onTemperatureReceived(true,temperature);
                        } else if (workInfo != null && workInfo.getState() == WorkInfo.State.FAILED) {
                            temperatureCallback.onTemperatureReceived(false,0);
                        }
                    }
                });
    }

    public static void handleLocationPermissionsResult(Context context, int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        _manager.locationManager.handlePermissionsResult(context,requestCode,permissions,grantResults);
    }
}
