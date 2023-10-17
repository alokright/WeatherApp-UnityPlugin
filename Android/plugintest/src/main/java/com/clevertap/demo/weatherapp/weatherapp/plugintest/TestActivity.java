package com.clevertap.demo.weatherapp.weatherapp.plugintest;

import androidx.annotation.NonNull;
import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.view.View;

import com.clevertap.demo.weatherapp.WeatherAppBridge;
import com.clevertap.demo.weatherapp.WeatherManager;

public class TestActivity extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_test);
        findViewById(R.id.check_temperature).setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                WeatherAppBridge.showTemperature(TestActivity.this);
            }
        });
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
        WeatherManager.getInstance(this).handleLocationPermissionsResult(TestActivity.this,requestCode,permissions,grantResults);
    }
}