using CommunityToolkit.Mvvm.Messaging;
using Pearl.Maui.Messages;
using Pearl.Maui.Services;

namespace Pearl.Maui.Views;

public sealed partial class ShellView : Shell
{
    private bool backButtonRequested;

    private readonly AuthenticationService authenticationService;

    public ShellView(AuthenticationService authenticationService)
    {
        InitializeComponent();
        this.authenticationService = authenticationService;
    }

    protected override bool OnBackButtonPressed()
    {
        backButtonRequested = base.OnBackButtonPressed();
        return backButtonRequested;
    }

    protected override async void OnNavigated(ShellNavigatedEventArgs args)
    {
        if (backButtonRequested)
        {
            base.OnNavigated(args);
            return;
        }

        if (args.Current.Location.OriginalString.Contains(nameof(GroupsView)))
        {
            return;
        }

        var accessToken = Preferences.Get("AccessToken", null);
        if (accessToken is null)
        {
            return;
        }

        var response = await authenticationService.RefreshAsync(accessToken);

        if (response.IsSuccess)
        {
            await Current.GoToAsync(nameof(GroupsView));
            WeakReferenceMessenger.Default.Send<GroupsMessage>();
        }

        base.OnNavigated(args);
    }
}