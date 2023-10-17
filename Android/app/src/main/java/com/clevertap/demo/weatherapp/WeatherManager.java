package com.clevertap.demo.weatherapp;

import android.content.Context;
import android.location.Location;

import androidx.annotation.NonNull;
import androidx.lifecycle.LiveData;
import androidx.lifecycle.Observer;
import androidx.work.Data;
import androidx.work.OneTimeWorkRequest;
import androidx.work.WorkInfo;
import androidx.work.WorkManager;

import com.clevertap.demo.weatherapp.callbacks.OnLocationReceived;
import com.clevertap.demo.weatherapp.callbacks.OnTemperatureReceived;

import org.json.JSONException;

public class WeatherManager {

    private static WeatherManager _manager = null;
    private UserLocationManager locationManager;

    private WeatherManager(Context context) {
        locationManager = new UserLocationManager();
    }

    public static WeatherManager getInstance(Context context) {
        if (_manager == null) {
            _manager = new WeatherManager(context);

        }
        return _manager;
    }

    public void fetchTemperature(Context context, OnTemperatureReceived onCallback) {
        locationManager.fetchUserLocation(context, new OnLocationReceived() {
            @Override
            public void onLocationStatus(boolean status, Location location) {
                if (status) {
                    getTemperature(context, location.getLatitude(), location.getLongitude(), onCallback);
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
                .setInputData(inputData)
                .build();

        WorkManager.getInstance(context).enqueue(fetchTemperatureRequest);

        LiveData<WorkInfo> liveData = WorkManager.getInstance(context).getWorkInfoByIdLiveData(fetchTemperatureRequest.getId());
        liveData.observeForever(new Observer<WorkInfo>() {
            @Override
            public void onChanged(WorkInfo workInfo) {
                try {
                    WeatherAppBridge.debugLog("workInfo.getState()" + workInfo.getState());
                    if (workInfo != null && workInfo.getState() == WorkInfo.State.SUCCEEDED) {
                        double temperature = workInfo.getOutputData().getDouble("temperature", 0);
                        temperatureCallback.onTemperatureReceived(true, temperature);
                    } else if (workInfo != null && workInfo.getState() == WorkInfo.State.FAILED) {
                        temperatureCallback.onTemperatureReceived(false, 0);
                    }
                } catch (JSONException e) {
                    throw new RuntimeException(e);
                }
            }
        });
    }


    public static void handleLocationPermissionsResult(Context context, int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        _manager.locationManager.handlePermissionsResult(context, requestCode, permissions, grantResults);
    }
}
