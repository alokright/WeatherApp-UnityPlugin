# WeatherApp UnityPlugin

## Overview

The WeatherApp Plugin for Unity provides C# APIs for accessing various ways to access 
temperature using Weather API [https://api.open-meteo.com/v1/forecast?latitude=19.07&longitude=72.87&timezone=IST&daily=t] 
and fetching location details from the device. Permission flow is handled by the plugin and there is no need to change launcher activity. 

## Version support

These plugins support Unity 2018.4 or later and Android sdk above API LEVEL 24.

## Downloading the plugins

There are two different options for obtaining the plugins:

*   Download individual plugins as `.unitypackage` files or Unity Package
    Manager (`.tgz`) files git

*   `git clone` this repository into the **Assets** folder of your Unity project

## Installing the plugins

 For all cases except `git clone` follow the instructions to
[Install Google packages for Unity](//developers.google.com/unity/instructions).

**Dependencies:**
**AndroidX and Jetifier**

Need changes in build.gradle to enable them.

**EDM4U**

Developers using `git clone` must also install the [External Dependency Manager for Unity (EDM4U)](https://github.com/googlesamples/unity-jar-resolver) using either the .tgz or .unitypackage available on the [Google APIs for Unity archive page](https://developers.google.com/unity/archive#external_dependency_manager_for_unity).

If EDM4U is not installed, the project won't be able to fetch necessary Java dependencies such as the [Play Location and AppCompat depencies], resulting in runtime errors.

**Newtonsoft Json Unity Package**

You can add the Newtonsoft.Json library to your Unity project using Git URL, you can follow these steps:

1. Open the Unity Package Manager:

2. In Unity, go to Window > Package Manager.
3. Add a Git Dependency:

4. In the Unity Package Manager, click on the "+" (plus) button, then select "Add package from Git URL."
Enter the Git URL:

Enter the Git URL for the Newtonsoft.Json package. The URL should point to the Git repository where the package is hosted. For Newtonsoft.Json, you can use the official GitHub repository URL:
https://github.com/JamesNK/Newtonsoft.Json.git

5. Click "Add":

6. After entering the Git URL, click the "Add" button to initiate the package import process.
Select the Package Version (12.0.2):

Unity Package Manager will prompt you to select a version if the package has multiple versions. Choose the appropriate version and click "Install."


# WeatherAppManager Usage Guide

The `WeatherAppManager` class in Unity allows you to access temperature data from an Android application. This guide will walk you through the steps to use this manager effectively in your Unity project.

## Methods and Functionality

### `Initialize()`

- **Usage**: This method initializes the `WeatherAppManager`. You should call it once at the beginning of your application.

```csharp
WeatherAppManager.Initialize();
```

### `AddCurrentTemperatureCallback(Action<double> callback)`

- **Usage**: Register a callback function to receive the current temperature data.

- **Example**:

```csharp
WeatherAppManager.Instance.AddCurrentTemperatureCallback((temperature) => {
    // Handle the temperature data
    Debug.Log("Current Temperature: " + temperature);
});
```

### `AddLocationCallback(Action<double, double> callback)`

- **Usage**: Register a callback function to receive location data (latitude and longitude).

- **Example**:

```csharp
WeatherAppManager.Instance.AddLocationCallback((latitude, longitude) => {
    // Handle the location data
    Debug.Log("Latitude: " + latitude + ", Longitude: " + longitude);
});
```

### `ShowToast(string message)`

- **Usage**: Show a toast message on Android (device-specific).

- **Example**:

```csharp
WeatherAppManager.ShowToast("Hello, Unity!");
```

### `ShowTemperatureToast()`

- **Usage**: Show a temperature-related toast on Android (device-specific).

- **Example**:

```csharp
WeatherAppManager.ShowTemperatureToast();
```

### `FetchWeeklyTemperature(Action<double[]> callback)`

- **Usage**: Request weekly temperature data from Android and register a callback to receive the data.

- **Example**:

```csharp
WeatherAppManager.FetchWeeklyTemperature((weeklyTemperatures) => {
    // Handle the weekly temperature data
    Debug.Log("Weekly Temperatures: " + string.Join(", ", weeklyTemperatures));
});
```

### `FetchCurrentTemperature(Action<double> callback)`

- **Usage**: Request the current temperature from Android and register a callback to receive the data.

- **Example**:

```csharp
WeatherAppManager.FetchCurrentTemperature((temperature) => {
    // Handle the current temperature data
    Debug.Log("Current Temperature: " + temperature);
});
```

## Testing Module Usage

In the `WeatherAppPlugin` demo folder, you'll find a testing module named `WeatherAppTestingModule.cs`. This module allows you to test various aspects of your integration and permissions. Additionally, a prefab named `WeatherAppTestingModule` is provided in the demo folder that you can add to your scene to quickly test the plugin with various scenarios.

Here's how to use it:

1. **Add the Testing Module Prefab to Your Scene**:

   - In the Unity project, locate the `WeatherAppTestingModule` prefab in the demo folder.

   - Drag and drop this prefab into your Unity scene to quickly set up the testing module.

   ![Add Prefab](https://github.com/alokright/UnityAdaptiveIconsSupport/blob/master/prefab.png)

2. **Open the Testing Module Scene**:

   - Open the Unity scene that includes the `WeatherAppTestingModule` component. This component contains all the testing functionality.


3. **Toggle Testing Module**:

   - Use the "Toggle Test Module" button to show or hide the testing module's user interface.

   ![Toggle Test Module](https://github.com/alokright/UnityAdaptiveIconsSupport/blob/master/WhatsApp%20Image%202023-10-20%20at%2009.12.13%20(1).jpeg)

4. **Execute Integration Tests**:

   - Tap the "Execute Integration Tests" button to validate the integration of your app with the WeatherApp plugin. It checks permissions, GameObject presence, and class dependencies.


5. **Execute Permission Flow Test**:

   - Tap the "Execute Permission Flow Test" button to validate permission-related functionality, including requesting permissions. This is where you can simulate various permission scenarios, such as granted, denied, and never-ask-me-again.

   ![Permission Flow Test](https://github.com/alokright/UnityAdaptiveIconsSupport/blob/master/WhatsApp%20Image%202023-10-20%20at%2009.12.14.jpeg)
   ![Permission Flow Test](https://github.com/alokright/UnityAdaptiveIconsSupport/blob/master/WhatsApp%20Image%202023-10-20%20at%2009.12.13.jpeg)
 

6. **Execute Location Test**:

   - Tap the "Execute Location Test" button to validate location-related functionality. The module will compare the fetched location with the known location.

7. **Execute Temperature Test**:

   - Tap the "Execute Temperature Test" button to validate temperature-related functionality. The module will compare the fetched temperature with the known local temperature.

8. **Open App Settings**:

   - Tap the "Open App Settings" button to open the app settings on the Android device. This is useful for managing permissions and simulating different permission scenarios, including granted, denied, and never-ask-me-again.

   ![Open App Settings](https://github.com/alokright/UnityAdaptiveIconsSupport/blob/master/Frame%208.png)

Using the testing module and the provided prefab, you can easily and quickly test your plugin with various scenarios, including different permission settings. This allows you to ensure that your app functions as expected under different conditions.

## Known Issues:

### 1. Error Code Propagation

The current system does not propagate error codes back to Unity, which can make it challenging to identify and address specific issues. 

### 2. Lack of Request and Response IDs

Requests and responses lack unique identifiers (IDs), making it difficult to correlate and track them, especially when handling multiple requests simultaneously.


### 3. Challenges with Handling Multiple Requests

The current implementation faces issues when handling multiple requests concurrently. 
### 4. Lack of Internet connectivity check

