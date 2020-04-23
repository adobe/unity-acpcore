# unity-acpcore

# Unity Plugin for V5 ACP Core sdk

This repository holds files necessary to create and distribute the Unity plugin for Adobe's V5 ACP Core SDK.

The Unity plugin supports iOS and Android.

## Getting Started

#### DOWNLOAD REQUIRED SOFTWARE
The `Unity Hub` application is required for development and testing. Inside of `Unity Hub`, you will be required to download the current version of the `Unity` app.

[Download the Unity Hub](http://unity3d.com/unity/download). The free version works for development and testing, but a Unity Pro license is required for distribution. See [Distribution](#distribution) below for details.

#### FOLDER STRUCTURE
Plugins for a Unity project use the following folder structure:

    {Project}/Assets/Plugins/{Platform}

#### OPENING THE PROJECT

1. From the menu of the `Unity` app, select __File > Open Project__
1. Navigate to the folder holding the Unity Project (*/unity-acpcore/ACPCore/*)
1. Hit __Open Project__

#### TESTING

To build and deploy changes:

###### Android
1. Make sure you have an Android device connected.
1. From the menu of the `Unity` app, select __File > Build Settings...__
1. Select `Android` from the __Platform__ window
1. If `Android` is not the active platform, hit the button that says __Switch Platform__ (it will only be available if you actually need to switch active platforms)
1. Press the __Build And Run__ button
1. You will be asked to provide a location to save the build. Make a new directory at */unity-acpcore/ACPCore/Builds* (this folder is in the .gitignore file)
1. Name build whatever you want and press __Save__
1. `Unity` will build an `apk` file and automatically deploy it to the connected device

###### iOS
1. From the menu of the `Unity` app, select __File > Build Settings...__
1. Select `iOS` from the __Platform__ window
1. If `iOS` is not the active platform, hit the button that says __Switch Platform__ (it will only be available if you actually need to switch active platforms)
1. Press the __Build And Run__ button
1. You will be asked to provide a location to save the build. Make a new directory at */unity-acpcore/ACPCore/Builds* (this folder is in the .gitignore file)
1. Name build whatever you want and press __Save__
1. `Unity` will create and open an `Xcode` project

## Distribution

#### Licensing

## Resources