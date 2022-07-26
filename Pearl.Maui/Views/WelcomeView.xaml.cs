using CommunityToolkit.Mvvm.Messaging;
using Pearl.Maui.Extensions;
using Pearl.Maui.Messages;
using Pearl.Maui.ViewModels;

namespace Pearl.Maui.Views;

public sealed partial class WelcomeView : ContentPage
{
    public WelcomeView(WelcomeViewModel viewModel)
    {
        BindingContext = viewModel;
        InitializeComponent();

        UsernameLabel.RegisterVisibilityToggler();
        PasswordLabel.RegisterVisibilityToggler();

        WeakReferenceMessenger.Default.Register<DialogMessage>(this,
            async (_, message) => await DisplayAlert(message.Title, message.Value, "Close"));
    }
}