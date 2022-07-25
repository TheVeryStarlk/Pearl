using Pearl.Maui.Views;

namespace Pearl.Maui;

public sealed partial class App : Application
{
    public App(ShellView shellView)
    {
        MainPage = shellView;
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);
        window.Title = "Pearl";

        return window;
    }
}