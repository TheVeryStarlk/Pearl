using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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

        builder.Configuration.AddJsonFile("appsettings.json");

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