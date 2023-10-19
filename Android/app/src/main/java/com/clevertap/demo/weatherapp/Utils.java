package com.clevertap.demo.weatherapp;

import android.content.Context;
import android.content.Intent;
import android.net.Uri;
import android.provider.Settings;

public class Utils {
    public static void requestLocationPermission(Context context){
        Intent intent = new Intent(context, PermissionsActivity.class);
        context.startActivity(intent);
    }
    public static void openAppSettings(Context context){
        Intent intent = new Intent();
        intent.setAction(Settings.ACTION_APPLICATION_DETAILS_SETTINGS);
        Uri uri = Uri.fromParts("package", context.getPackageName(), null);
        intent.setData(uri);
        context.startActivity(intent);
    }
}
