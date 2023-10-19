# WeatherApp UnityPlugin

## Overview

The WeatherApp Plugin for Unity provides C# APIs for accessing various ways to access 
temperature using Weather API [https://api.open-meteo.com/v1/forecast?latitude=19.07&longitude=72.87&timezone=IST&daily=t] 
and fetching location details from the device. 

## Version support

These plugins support Unity 2018.4 or later and Android sdk above API LEVEL 24.

## Downloading the plugins

There are 2 different options for obtaining the plugins:

*   Download individual plugins as `.unitypackage` files or Unity Package
    Manager (`.tgz`) files git

*   `git clone` this repository into the **Assets** folder of your Unity project

## Installing the plugins

 For all cases except `git clone` follow the instructions to
[Install Google packages for Unity](//developers.google.com/unity/instructions).

#Dependencies:
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

