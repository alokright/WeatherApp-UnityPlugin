# WeatherApp UnityPlugin

## Overview

The WeatherApp Plugin for Unity provide C# APIs for accessing various ways to access 
temperature using Weather API [https://api.open-meteo.com/v1/forecast?latitude=19.07&longitude=72.87&timezone=IST&daily=t] 
and fetching locations details from device. 

## Version support

These plugins support Unity 2018.4 or later and android sdk above API LEVEL 24.

## Downloading the plugins

There are 3 different options for obtaining the plugins:

*   Download individual plugins as `.unitypackage` files or Unity Package
    Manager (`.tgz`) files git

*   `git clone` this repository into the **Assets** folder of your Unity project

## Installing the plugins

For all cases except `git clone` follow the instructions to
[Install Google packages for Unity](//developers.google.com/unity/instructions).

Developers using `git clone` must also install the [External Dependency Manager for Unity (EDM4U)](https://github.com/googlesamples/unity-jar-resolver) using either the .tgz or .unitypackage available on the [Google APIs for Unity archive page](https://developers.google.com/unity/archive#external_dependency_manager_for_unity).

If EDM4U is not installed, the project won't be able to fetch necessary Java dependencies such as the [Play Location and AppCompat depencies], resulting in runtime errors.
