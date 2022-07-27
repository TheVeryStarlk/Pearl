using CommunityToolkit.Maui.Alerts;
using Pearl.Maui.ViewModels;

namespace Pearl.Maui.Views;

public sealed partial class GroupsView : ContentPage
{
    private readonly GroupsViewModel viewModel;

    public GroupsView(GroupsViewModel viewModel)
    {
        this.viewModel = viewModel;
        BindingContext = viewModel;

        InitializeComponent();

        LooksEmptyLabel.IsVisible = viewModel.Groups?.Length == 0;
        this.viewModel = viewModel;
    }

    private async void JoinGroupButtonAsync(object sender, EventArgs eventArgs)
    {
        var input = await DisplayPromptAsync("Join", $"A new group will be created if the provided group name does not already exist.{Environment.NewLine}", cancel: string.Empty, placeholder: "Name");

        if (string.IsNullOrWhiteSpace(input))
        {
            await Toast.Make("The provided group name was invalid.").Show();
            return;
        }

        var response = await viewModel.HubService.JoinGroupAsync(input);

        if (response.IsSuccess)
        {
            await Toast.Make(response.Value).Show();
            return;
        }

        await Toast.Make(response.Errors[0].Message).Show();
    }
}