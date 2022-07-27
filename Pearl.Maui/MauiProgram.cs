using CommunityToolkit.Maui;
using Pearl.Maui.Services;
using Pearl.Maui.ViewModels;
using Pearl.Maui.Views;

namespace Pearl.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // The default one, for debugging purposes.
        Preferences.Set("Url", "https://localhost:7197");

        builder.Services
            .AddTransient<ShellView>()
            .AddTransient<WelcomeView>()
            .AddTransient<WelcomeViewModel>()
            .AddTransient<GroupsView>()
            .AddTransient<GroupsViewModel>()
            .AddTransient<AuthenticationService>()
            .AddTransient<HubService>()
            .AddTransient<ValidationService>()
            .AddScoped<HttpClient>();

        return builder.Build();
    }
}