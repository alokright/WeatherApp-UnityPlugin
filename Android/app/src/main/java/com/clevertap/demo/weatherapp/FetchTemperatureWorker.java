package com.clevertap.demo.weatherapp;
import android.content.Context;

import androidx.annotation.NonNull;
import androidx.work.Data;
import androidx.work.Worker;
import androidx.work.WorkerParameters;

import org.json.JSONArray;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.Locale;

public class FetchTemperatureWorker extends Worker {

    public FetchTemperatureWorker(
            @NonNull Context context,
            @NonNull WorkerParameters workerParams) {
        super(context, workerParams);
    }

    @NonNull
    @Override
    public Result doWork() {
        WeatherAppBridge.debugLog("On Do Work");
        double latitude = getInputData().getDouble("latitude", 0);
        double longitude = getInputData().getDouble("longitude", 0);

        String urlString = "https://api.open-meteo.com/v1/forecast?latitude=" + latitude + "&longitude=" + longitude + "&timezone=IST&daily=temperature_2m_max";
        try {
            URL url = new URL(urlString);
            HttpURLConnection connection = (HttpURLConnection) url.openConnection();
            connection.setRequestMethod("GET");
            int responseCode = connection.getResponseCode();

            if (responseCode == HttpURLConnection.HTTP_OK) {
                StringBuilder responseString = new StringBuilder();
                BufferedReader reader = new BufferedReader(new InputStreamReader(connection.getInputStream()));
                String line;
                while ((line = reader.readLine()) != null) {
                    responseString.append(line);
                }
                reader.close();

                JSONObject responseObject = new JSONObject(responseString.toString());
                JSONArray datesArray = responseObject.getJSONObject("daily").getJSONArray("time");
                JSONArray temperatureArray = responseObject.getJSONObject("daily").getJSONArray("temperature_2m_max");

                String todayDate = new SimpleDateFormat("yyyy-MM-dd", Locale.getDefault()).format(new Date());
                double temperature = 0f;
                boolean isFound = false;
                double[] dailyTemp = new double[datesArray.length()];
                for (int i = 0; i < datesArray.length(); i++) {
                    if (datesArray.getString(i).equals(todayDate)) {
                       temperature = temperatureArray.getDouble(i);
                       isFound = true;
                    }
                    dailyTemp[i] =  temperatureArray.getDouble(i);
                }
                Data outputData = new Data.Builder()
                        .putDouble(Constants.TEMPERATURE_KEY, temperature)
                        .putDoubleArray(Constants.DAILY_TEMPERATURE_KEY,dailyTemp)
                        .build();

                if(isFound){
                    WeatherAppBridge.debugLog("On Result Success!");
                    return Result.success(outputData);
                }
               else
                    return Result.failure();
            } else {
                return Result.failure();
            }
        } catch (Exception e) {
            return Result.failure();
        }
    }
}


