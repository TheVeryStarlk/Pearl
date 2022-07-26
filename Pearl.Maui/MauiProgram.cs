using Microsoft.Extensions.Configuration;
using Pearl.Maui.Models;
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
            .AddTransient<AuthenticationService>()
            .AddTransient<ValidationService>()
            .AddScoped<HttpClient>()
            .AddTransient(_ => new Settings()
            {
                Url = builder.Configuration.GetValue<string>($"{nameof(Settings)}:Url")
            });

        return builder.Build();
    }
}