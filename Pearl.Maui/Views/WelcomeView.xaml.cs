using Pearl.Maui.ViewModels;

namespace Pearl.Maui.Views;

public sealed partial class WelcomeView : ContentPage
{
    public WelcomeView(WelcomeViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();
    }
}