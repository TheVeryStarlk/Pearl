using Pearl.Maui.Views;

namespace Pearl.Maui;

public sealed partial class App : Application
{
    public App(ShellView shellView)
    {
        MainPage = shellView;
        InitializeComponent();
    }
}