using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Pearl.Maui.Services;
using Plugin.ValidationRules;
using Plugin.ValidationRules.Rules;

namespace Pearl.Maui.ViewModels;

public sealed class WelcomeViewModel : ObservableObject
{
    public Validatable<string> Username { get; }

    public Validatable<string> Password { get; }

    public AsyncRelayCommand AuthenticateCommandAsync { get; }

    private readonly ValidationService validationService;

    public WelcomeViewModel(ValidationService validationService)
    {
        this.validationService = validationService;

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
        validationService.ValidateAll(Username, Password);
    }
}