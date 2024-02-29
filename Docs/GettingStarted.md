# Getting Started

These are the steps for getting up and running quickly with MR-MUSK.

## Create Unity Project

First you'll need a Unity project. If you already have one, great! Feel free to use it. If you're starting from scratch we recommend Unity **2022.3.19f1**. Install [Unity Hub](https://unity.com/download) then click [here](unityhub://2022.3.19f1/244b723c30a6) to install the right editor version. During installation, be sure to include **Android Build Support** since Quest devices are based on Android.

<img src="Images/InstallAndroid.png" title="" alt="" width="600">

When creating your project we recommend **3D URP**, but standard **3D** is also fine.

<img src="Images/HubNewProject.png" title="" alt="HubNewProject.png" width="600">

## Add Normcore Registry

This starter kit uses [Normcore](https://normcore.io/) for networking, but Normcore distributes its packages through their own package server. We need to add their scoped registry to our project or Unity won't know how to find their packages.

1. Click on `Edit > Project Settings > Package Manager`

2. Under **Scoped Registries**, add the following information:

```
Name: Normal
URL: https://normcore-registry.normcore.io
Scope(s): 
com.normalvr
io.normcore
```

When you're done it should look like this:

<img src="Images/NormcoreRegistry.png" title="" alt="NormcoreRegistry.png" width="600">

## Add MRMUSK Package

Now we can add the starter kit to our project.

1. Go to `Window -> Package Manager`

2. Click the '+' dropdown in the top-left corner and choose **Add Package from git
   URL**.
   
   <img src="file:///C:/Code/SolerSoft/MRMUSK/Docs/Images/AddFromGitURL.png" title="" alt="AddFromGitURL.png" width="230">

3. In the URL field enter:
   
   ```
   https://github.com/SolerSoft/mr-musk.git?path=/Assets/Packages/com.solersoft.mrmusk
   ```

4. Click the **Add** button

> This process may take a minute or two since it's installing MR-MUSK, Normcore and all of the Oculus SDK. You may also be prompted to restart the Unity editor during installation.

## Oculus Setup

Now that the Oculus SDK is part of our project, we need to configure it.

1. Click on `File > Build Settings`

2. Select **Android** and click **Switch Platform** (this will take a minute)
   
   <img src="Images/AndroidSwitchPlatform.png" title="" alt="AndroidSwitchPlatform.png" width="600">

3. Click on `Edit > Project Settings > XR Plugin Management` then click the **Install XR Plugin Management** button. 
   
   <img title="" src="Images/InstallXRManagement.png" alt="InstallXRManagement.png" width="600">
   
   (This button won't exist if XR is already installed in your project.)

4. Under **XR Plugin Management**, check the box for **Oculus**.
   
   <img src="Images/XRPluginOculus.png" title="" alt="XRPluginOculus.png" width="600">

5. Under **XR Plugin Management > Oculus**, check the box for **Quest 3**.
   
   <img src="Images/OculusEnableQ3.png" title="" alt="OculusEnableQ3.png" width="600">

6. Under `Edit > Project Settings > Oculus`, click **Fix All** and **Apply All** to resolve any issues.
   
   <img src="Images/OculusFixAll.png" title="" alt="OculusFixAll.png" width="600">

## Import Samples

Sample Scenes

Though configuring
your initial scene isn't difficult, it's even easier to start from a working
sample.

Return to the
Package Manager

Select MRMUSK

Select the Samples
tab

Click the Import
button

In your project,
open Samples\MRMUSK\#.#.#\Starter Samples\Scenes

You can start from
either the VR scene or the MR scene.

## Configure Normcore

Normcore

Before you can hit
play or deploy to your device, you need to provide the App Key for Normcore.
This is how they know who to bill when you go over the free number of hours
included.

App Key

Room Name

## Print Controller Mount
