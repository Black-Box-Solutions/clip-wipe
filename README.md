# ClipWipe for Android, iOS

A .NET MAUI application for managing clipboard content on mobile devices
manually or at a specified interval.

<!--TOC-->
  - [Features](#features)
  - [Requirements](#requirements)
  - [Implementation Details](#implementation-details)
  - [d access alerts, this applicat](#d-access-alerts-this-applicat)
  - [nager.cspr](#nager.cspr)
  - [s
├── MauiProgra](#s-mauiprogra)
<!--/TOC-->

## Features

- View current clipboard content
- Clear clipboard content manually with one tap
- Auto-clear clipboard based on a configurable timer
- Start application automatically on device boot (optional)
- Simple and intuitive user interface

## Requirements

- Android 6.0 (API level 23) or higher
- iOS 15.0 or higher
- Samsung devices running One UI(tested on Galaxy S25+)

## Implementation Details

The application is structured using the MVVM(Model-View-ViewModel) pattern:

- ** Pages**: UI components(MainPage, SettingsPage)
- ** PageModels**: Business logic and UI state(MainPageViewModel, SettingsPageViewModel)
- ** Services**: Core functionality(ClipboardService, SettingsService, ClipboardTimerService)

The application uses platform-specific code to interact with the Android clipboard API, and implements proper async patterns throughout the codebase.

## Samsung OneUI Compatibility

The application has been tested on Samsung devices running OneUI.
While Samsung does implement some clipboard access alerts, this application is designed to work with these constraints.

## License

Copyright (c) 2025 Black Box Solutions, Inc. All rights reserved.

## File Structure

```
ClipWipe.App/
├── ClipWipe.App.csproj
├── App.xaml
├── App.xaml.cs
├── AppShell.xaml
├── AppShell.xaml.cs
├── MauiProgram.cs
├── Converters/
│   ├── InvertedBoolConverter.cs
│   └── NullToBoolConverter.cs
├── Services/
│   ├── IClipboardService.cs
│   ├── ClipboardService.cs
│   ├── ClipboardTimerService.cs
│   ├── ISettingsService.cs
│   └── SettingsService.cs
├── PageModels/
│   ├── MainPageViewModel.cs
│   └── SettingsPageViewModel.cs
├── Pages/
│   ├── MainPage.xaml
│   ├── MainPage.xaml.cs
│   ├── SettingsPage.xaml
│   └── SettingsPage.xaml.cs
├── Platforms/
│   └── Android/
│       ├── AndroidManifest.xml
│       ├── BootReceiver.cs
│       ├── ClipboardService.cs
│       ├── MainActivity.cs
│       └── Resources/
│           └── values/
│               └── colors.xml
└── Resources/
    ├── AppIcon/
    │   ├── appicon.svg
    │   └── appiconfg.svg
    ├── Fonts/
    │   ├── OpenSans-Regular.ttf
    │   └── OpenSans-Semibold.ttf
    ├── Images/
    │   ├── clipboard.png
    │   └── settings.png
    ├── Splash/
    │   └── splash.svg
    └── Styles/
        ├── Colors.xaml
        └── Styles.xaml
```
