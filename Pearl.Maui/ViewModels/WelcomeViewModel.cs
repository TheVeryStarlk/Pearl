using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Pearl.Maui.Messages;
using Pearl.Maui.Services;
using Pearl.Maui.Views;
using Plugin.ValidationRules;
using Plugin.ValidationRules.Rules;

namespace Pearl.Maui.ViewModels;

public sealed class WelcomeViewModel : ObservableObject
{
    public Validatable<string> Username { get; }

    public Validatable<string> Password { get; }

    public AsyncRelayCommand AuthenticateCommandAsync { get; }

    public bool IsReady
    {
        get => isReady;
        set => SetProperty(ref isReady, value);
    }

    private bool isReady = true;

    private readonly ValidationService validationService;
    private readonly AuthenticationService authenticationService;

    public WelcomeViewModel(ValidationService validationService, AuthenticationService authenticationService)
    {
        this.validationService = validationService;
        this.authenticationService = authenticationService;

        Username = new Validatable<string>();
        Username.Validations.Add(new NotEmptyRule<string>(string.Empty)
        {
            ValidationMessage = "Username can not be empty."
        });

        Password = new Validatable<string>();
        Password.Validations.Add(new NotEmptyRule<string>(string.Empty)
        {
            ValidationMessage = "Password can not be empty."
        });

        AuthenticateCommandAsync = new AsyncRelayCommand(AuthenticateAsync);
    }

    private async Task AuthenticateAsync()
    {
        if (!validationService.ValidateAll(Username, Password))
        {
            return;
        }

        IsReady = false;
        var response =
            await authenticationService.AuthenticateAsync(Username.Value, Password.Value);

        if (response is null)
        {
            WeakReferenceMessenger.Default.Send(new DialogMessage("Invalid Response", "An unknown error has occurred, please try again later."));
            IsReady = true;
            return;
        }

        if (response.IsSuccess)
        {
            Preferences.Set("AccessToken", response.Value!.AccessToken);
            Preferences.Set("RefreshToken", response.Value!.RefreshToken);

            await Shell.Current.GoToAsync(nameof(GroupsView));
            WeakReferenceMessenger.Default.Send<GroupsMessage>();
            IsReady = true;
            return;
        }

        WeakReferenceMessenger.Default.Send(new DialogMessage("Authentication Failure",
            string.Join(Environment.NewLine, response.Errors.Select(error => error.Message))));
        IsReady = true;
    }
}