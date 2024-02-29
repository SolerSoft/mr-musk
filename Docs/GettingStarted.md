# Getting Started

These are the steps for getting up and running quickly with MR-MUSK.



## Create a Unity Project

First you'll need a Unity project. If you already have one, great! Feel free to use it. If you're starting from scratch we recommend Unity **2022.3.19f1**. Install [Unity Hub](https://unity.com/download) then click [here](unityhub://2022.3.19f1/244b723c30a6) to install the right editor version. During installation, be sure to include **Android Build Support** since Quest devices are based on Android.

![](Images/InstallAndroid.png)

When creating your project we recommend **3D URP**, but standard **3D** is also fine.

![HubNewProject.png](Images/HubNewProject.png)

## Add Scoped Registry

This starter kit uses [Normcore](https://normcore.io/) for networking, but Normcore distributes its packages through their own package server. We need to add their scoped registry to our project or Unity won't know how to find their packages.

1. Click on `Edit > Project Settings > Package Manager`

2. Under **Scoped Registries**, add the following information:

```
Name: Normal
URL: https://normcore-registry.normcore.io
Scope(s): com.normalvr, io.normcore
```

When you're done it should look like this:

![NormcoreRegistry.png](Images/NormcoreRegistry.png)



## Add

Package

Now we can go ahead
and add the starter kit to our project.

Window -> Package
Manager

+ dropdown in the
  top-left corner

Add Package from Git
URL

Package URL:

https://github.com/SolerSoft/mr-musk.git?path=/Assets/Packages/com.solersoft.mrmusk

## Setup

Oculus XR

If you created a new
project, now we need to set it up for Oculus Mixed Reality.

File -> Build
Settings -> Select Android and click Switch Platform

Edit -> Project
Settings -> XR Plugin Management -> Install XR Plugin Management

Under XR Plugin
Management, check Oculus

Under XR Plugin
Management -> Oculus, check Quest 3.

Under Edit ->
Project Settings -> Oculus, resolve any outstanding issues.

## Import

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

## Configure

Normcore

Before you can hit
play or deploy to your device, you need to provide the App Key for Normcore.
This is how they know who to bill when you go over the free number of hours
included.

App Key

Room Name

## Print

Controller Mount

?
