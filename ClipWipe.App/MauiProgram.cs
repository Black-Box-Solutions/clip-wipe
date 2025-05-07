using ClipWipe.App.PageModels;
using ClipWipe.App.Pages;
using ClipWipe.App.Services;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Toolkit.Hosting;

namespace ClipWipe.App;
//public static class MauiProgram
//{
//    public static MauiApp CreateMauiApp()
//    {
//        var builder = MauiApp.CreateBuilder();
//        builder
//            .UseMauiApp<App>()
//            .ConfigureFonts(fonts =>
//            {
//                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
//                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
//            });

//#if DEBUG
//        builder.Logging.AddDebug();
//#endif

//        return builder.Build();
//    }
//}

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        MauiAppBuilder builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionToolkit()
            // After initializing the Syncfusion .NET MAUI Toolkit, optionally add additional fonts
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Continue initializing your .NET MAUI App here
        // Register services
        builder.Services.AddSingleton<IClipboardService, ClipboardService>();
        builder.Services.AddSingleton<ISettingsService, SettingsService>();
        builder.Services.AddSingleton<ClipboardTimerService>();
        //TODO should i use this or something in MVVM toolkit (if it exists)
        //builder.Services.AddSingleton<IErrorHandler, ModalErrorHandler>();

        // Register PageModels
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddTransient<SettingsPageViewModel>();

        // Register Pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<SettingsPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
