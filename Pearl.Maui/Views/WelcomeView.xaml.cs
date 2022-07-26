using CommunityToolkit.Mvvm.Messaging;
using Pearl.Maui.Extensions;
using Pearl.Maui.Messages;
using Pearl.Maui.ViewModels;

namespace Pearl.Maui.Views;

public sealed partial class WelcomeView : ContentPage
{
    private readonly WelcomeViewModel viewModel;

    public WelcomeView(WelcomeViewModel viewModel)
    {
        this.viewModel = viewModel;
        BindingContext = viewModel;
        InitializeComponent();

        WeakReferenceMessenger.Default.Register<DialogMessage>(this,
            async (_, message) => await DisplayAlert(message.Title, message.Value, "Close"));
    }

    private void AuthenticationButton(object sender, EventArgs eventArgs)
    {
        UsernameLabel.RegisterValidation(UsernameEntry, viewModel.Username);
        PasswordLabel.RegisterValidation(PasswordEntry, viewModel.Password);
    }
}