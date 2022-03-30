CSC2033: Engie Maintenance App - Team Project
====================

About:
-------------- 

This application can be run on both iOS and Android mobile devices.
It is a comprehensive asset maintenance application that allows engineers (public users) to scan a QR code to download a relevant form which can then be filled in and edited locally in the app. When forms are completed and submitted they are sent in emails as attachments to the correct engineer manager.
Users with roleType admin have additional features at their disposal, for example, they can upload new fillable pdf forms to the remote aws bucket, generate new qr codes for these forms. Admin users can also view and update the recipient email addresses for specific types of forms from the Admin Home Page within the application.

Links to Git repositories:
-------------- 
WebServices: https://nucode.ncl.ac.uk/scomp/stage-2/csc2033-software-engineering-team-project/teams/Team-24/engie_web_services/commit/f06b3b60ac30804ed3add2b4ccbf9600cd6ab191

Main Application: https://nucode.ncl.ac.uk/scomp/stage-2/csc2033-software-engineering-team-project/teams/Team-24/engie_maintenance_app/tree/1d594658708cfc7bf4462c0354b069b1a34bda8e

How to Install (Android):
-------------- 

1) Open up your chosen IDE, we recommend Visual Studio.
2) Install IDE extensions: Xamarin for Android and Xamarin for iOS using the Visual Studio Installer
3) Open engie_maintenance_app.sln in Visual Studio
4) Download and set up an Android Device Emulator within Visual Studio please choose one with android API 28 or higher
5) Select the engie_maintanance_app.Android project and build it. Then run it using the emulator, the app will install and run on the emulator device
6) Launch the app on the emulator

How to Install (iOS - Mac Computer Required!):
-------------- 
1) Ensure Xcode is installed and you have an apple id available to use
2) Open up your chosen IDE, we recommend Visual Studio.
3) Install IDE extensions: Xamarin for Android and Xamarin for iOS using the Visual Studio Installer
4) Follow these steps to ensure your app signing profile is set up correctly: https://docs.microsoft.com/en-us/xamarin/ios/get-started/installation/device-provisioning/free-provisioning?tabs=macos  the bundle identifier for the app which is used for signing is: "velocitysolutions.tk.engie-maintenance-app"
5) Open engie_maintenance_app.sln in Visual Studio
6) Select the engie_maintenance_app.iOS project and build it. Then run it using one of the iOS device emulators that were installed automatically by XCode, the app will install and run on the emulator device.